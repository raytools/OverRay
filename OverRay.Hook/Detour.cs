using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using EasyHook;
using OverRay.Hook.Mod;

namespace OverRay.Hook
{
    public class Detour : IEntryPoint
    {
        public Detour(RemoteHooking.IContext context, string channelName)
        {
            //Debugger.Launch();
            Interface = RemoteHooking.IpcConnectClient<RemoteInterface>(channelName);
            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());
        }

        internal static RemoteInterface Interface { get; set; }

        internal static Dictionary<string, LocalHook> Hooks = new Dictionary<string, LocalHook>();

        private GameManager Game;

        public void Run(RemoteHooking.IContext inContext, string inChannelName)
        {
            try
            {
                // Make sure the game is fully loaded before initializing hooks
                while (true)
                {
                    // Engine state: 1 - loading game, 5 - loading level, 9 - loaded
                    if (Marshal.ReadByte((IntPtr)0x500380) > 8)
                        break;
                }

                Game = new GameManager();

                RemoteHooking.WakeUpProcess();
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            while (true) Thread.Sleep(100);
        }

        ~Detour()
        {
            Interface.GameClosed();
        }
    }
}
