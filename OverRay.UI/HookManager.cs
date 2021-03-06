﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using EasyHook;
using OverRay.Hook;

namespace OverRay.UI
{
    public class HookManager
    {
        public HookManager(string libraryName, RemoteInterface remote, params string[] processNames)
        {
            InjectionLib = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), libraryName);
            ProcessNames = processNames;

            Remote = remote;
        }

        private string _channelName;
        private IpcServerChannel ipc;
        private RemoteInterface Remote { get; }
        private string[] ProcessNames { get; }
        private string InjectionLib { get; }

        public bool IsHookAttached { get; set; }

        public void Inject()
        {
            _channelName = null;

            if (ipc == null)
            {
                ipc = RemoteHooking.IpcCreateServer<RemoteInterface>(ref _channelName,
                    WellKnownObjectMode.Singleton, Remote);
            }

            Thread injectionThread = new Thread(() =>
            {
                Remote.Log("Attempting to inject...");
                while (!IsHookAttached)
                {
                    int processId = GetProcessId();

                    if (processId == 0)
                    {
                        Remote.Log("Cannot find process, retrying in 5s...");
                        Thread.Sleep(5000);
                        continue;
                    }

                    try
                    {
                        RemoteHooking.Inject(processId, InjectionOptions.DoNotRequireStrongName,
                            InjectionLib, InjectionLib, _channelName);

                        IsHookAttached = true;
                        Remote.Log("Injection finished.");
                    }
                    catch (Exception e)
                    {
                        Remote.Log("Injection error:");
                        Remote.Log(e.ToString());
                        Remote.Log("Retrying in 5s...");

                        IsHookAttached = false;
                        Thread.Sleep(5000);
                    }
                }
            });
            injectionThread.IsBackground = true;
            injectionThread.Start();
        }

        private int GetProcessId()
        {
            foreach (string name in ProcessNames)
            {
                Process[] processes = Process.GetProcessesByName(name);

                if (processes.Length > 0)
                    return processes[0].Id;
            }

            return 0;
        }
    }
}