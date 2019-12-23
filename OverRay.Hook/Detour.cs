using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;

namespace OverRay.Hook
{
    public class Detour : IEntryPoint
    {
        public Detour(RemoteHooking.IContext context, string channelName)
        {
            //Debugger.Launch();
            Interface = RemoteHooking.IpcConnectClient<RemoteInterface>(channelName);
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            Game = new GameFunctions(Interface);

            GetTextures();
        }

        private RemoteInterface Interface { get; }
        private GameFunctions Game { get; }

        private static Dictionary<string, LocalHook> Hooks = new Dictionary<string, LocalHook>();

        public void Run(RemoteHooking.IContext inContext, string inChannelName)
        {
            try
            {
                Hooks["VEngine"] = LocalHook.Create(GameFunctions.VEnginePtr, new GameFunctions.VEngine(Game.HVEngine), this);
                Hooks["DrawsTexts"] = LocalHook.Create(GameFunctions.DrawsTextsPtr, new GameFunctions.DrawsTexts(Game.HDrawsTexts), this);
                //Hooks["VAddParticle"] = LocalHook.Create(GameFunctions.VAddParticlePtr, new GameFunctions.VAddParticle(Game.HVAddParticle), this);

                foreach (KeyValuePair<string, LocalHook> hook in Hooks)
                    hook.Value.ThreadACL.SetExclusiveACL(new[] { 0 });

                RemoteHooking.WakeUpProcess();


            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            while (true) Thread.Sleep(100);

        }

        public void GetTextures()
        {
            IntPtr tPtr = (IntPtr)0x502680;
            IntPtr tMemChannelsPtr = (IntPtr)0x501660;


            uint[] tMemChannels = new uint[1024];
            for (int i = 0; i < tMemChannels.Length; i++)
            {
                tMemChannels[i] = (uint) Marshal.ReadInt32(tMemChannelsPtr + 4 * i);
            }

            List<Texture> textures = new List<Texture>();
            for (int i = 0; i < tMemChannels.Length; i++)
            {
                IntPtr textureStructPtr = Marshal.ReadIntPtr(tPtr + 4 * i);
                if (textureStructPtr != IntPtr.Zero && tMemChannels[i] != 0xC0DE0005)
                {
                    Texture texture = new Texture(textureStructPtr);
                    Game.textures.Add(texture);
                    if (texture.Name == @"textures_personnages\divers\gra03_lum_my_ad.tga")
                    {
                        Interface.WriteLog(Convert.ToString((int)texture.Pointer, 16));
                    }
                }
            }
        }
    }
}
