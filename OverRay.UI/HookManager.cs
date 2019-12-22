using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Ipc;
using EasyHook;
using OverRay.Hook;

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

        private string[] ProcessNames { get; }
        private string InjectionLib { get; }
        public string ExceptionMessage { get; set; }

        public InjectorResponse Inject()
        {
            _channelName = null;
            int processId = GetProcessId();

            if (processId == 0)
                return InjectorResponse.ProcessNotFound;

            try
            {
                ipc = RemoteHooking.IpcCreateServer<RemoteInterface>(ref _channelName,
                    WellKnownObjectMode.Singleton, Interface);

                RemoteHooking.Inject(processId, InjectionOptions.DoNotRequireStrongName,
                    InjectionLib, InjectionLib, _channelName);

                return InjectorResponse.Success;
            }
            catch (Exception e)
            {
                ExceptionMessage = e.Message;
                return InjectorResponse.OtherError;
            }
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

    public enum InjectorResponse
    {
        Success,
        ProcessNotFound,
        OtherError
    }
}