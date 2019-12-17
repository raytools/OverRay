using System;
using System.Threading;
using EasyHook;

namespace OverRay.Hook
{
    public class Detour : IEntryPoint
    {
        public Detour(RemoteHooking.IContext context, string channelName)
        {
            Interface = RemoteHooking.IpcConnectClient<RemoteInterface>(channelName);
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
            Game = new GameFunctions(Interface);
        }


        private RemoteInterface Interface { get; }
        private GameFunctions Game { get; }

        private static LocalHook VEngineHook { get; set; }
//        private static LocalHook VAddParticleHook { get; set; }
//        private static LocalHook DrawTextHook { get; set; }

        public void Run(RemoteHooking.IContext inContext, string inChannelName)
        {
            try
            {
                VEngineHook = LocalHook.Create(GameFunctions.VEnginePtr, new GameFunctions.VEngine(Game.HVEngine), this);
                VEngineHook.ThreadACL.SetExclusiveACL(new[] { 0 });

//                DrawTextHook = LocalHook.Create(GameFunctions.DrawTextPtr, new GameFunctions.DrawText(Game.HDrawText), this);
//                DrawTextHook.ThreadACL.SetExclusiveACL(new[] { 0 });

//                VAddParticleHook = LocalHook.Create(GameFunctions.VAddParticlePtr, new GameFunctions.VAddParticle(Game.HVAddParticle), this);
//                VAddParticleHook.ThreadACL.SetExclusiveACL(new[] { 0 });

                RemoteHooking.WakeUpProcess();
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            while (true) Thread.Sleep(100);

        }
    }
}
