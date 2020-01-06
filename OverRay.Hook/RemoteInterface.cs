using System;
using System.Threading;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public class RemoteInterface : MarshalByRefObject
    {
        public void IsInstalled(int pid)
        {
            Log.Add($"DLL injected, PID: {pid}");
        }

        public void WriteLog(string msgPacket)
        {
            Log.Add(msgPacket);
        }

        public void HandleError(Exception e)
        {
            Log.Add(e.Message);
        }

        public void GameClosed()
        {
            // lazy hack to make sure IPC channel is closed
            new Thread(() =>
            {
                Thread.Sleep(500);
                System.Environment.Exit(0);
            }).Start();
        }
    }

}