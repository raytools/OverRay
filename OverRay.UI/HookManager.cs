using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using EasyHook;
using OverRay.Hook;
using OverRay.Hook.Utils;

namespace OverRay.UI
{
    public class HookManager
    {
        public HookManager(string libraryName, params string[] processNames)
        {
            InjectionLib = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), libraryName);
            ProcessNames = processNames;

            Interface = new RemoteInterface();
        }

        private string _channelName;
        private IpcServerChannel ipc;

        public RemoteInterface Interface { get; set; }
        public bool IsHookAttached { get; set; }

        private string[] ProcessNames { get; }
        private string InjectionLib { get; }
        public string ExceptionMessage { get; set; }

        public void Inject()
        {
            _channelName = null;

            ipc = RemoteHooking.IpcCreateServer<RemoteInterface>(ref _channelName,
                WellKnownObjectMode.Singleton, Interface);

            Thread injectionThread = new Thread(() =>
            {
                Log.Add("Attempting to inject...");
                while (!IsHookAttached)
                {
                    int processId = GetProcessId();

                    if (processId == 0)
                    {
                        Log.Add("Cannot find process, retrying in 5s...");
                        Thread.Sleep(5000);
                        continue;
                    }

                    try
                    {
                        RemoteHooking.Inject(processId, InjectionOptions.DoNotRequireStrongName,
                            InjectionLib, InjectionLib, _channelName);

                        IsHookAttached = true;
                        Log.Add("Injection finished.");
                    }
                    catch (Exception e)
                    {
                        Log.Add("Injection error:");
                        ExceptionMessage = e.Message;
                        Log.Add("Retrying in 5s...");

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