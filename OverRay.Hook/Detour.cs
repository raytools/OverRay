using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        }

        private RemoteInterface Interface { get; }
        private GameFunctions Game { get; }

        private static Dictionary<string, LocalHook> Hooks = new Dictionary<string, LocalHook>();

        public void Run(RemoteHooking.IContext inContext, string inChannelName)
        {
            try
            {
                Hooks["VEngine"] = LocalHook.Create(GameFunctions.VEnginePtr, new GameFunctions.VEngine(Game.HVEngine), this);
                Hooks["VEngine"].ThreadACL.SetExclusiveACL(new[] { 0 });
                
                Hooks["DrawsTexts"] = LocalHook.Create(GameFunctions.DrawsTextsPtr, new GameFunctions.DrawsTexts(Game.HDrawsTexts), this);
                Hooks["DrawsTexts"].ThreadACL.SetExclusiveACL(new[] { 0 });

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
