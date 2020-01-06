using System;
using System.Collections.Generic;
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

        public static Detour Instance;

        public void Run(RemoteHooking.IContext inContext, string inChannelName)
        {
            try
            {
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
