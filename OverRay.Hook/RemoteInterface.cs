using System;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public class RemoteInterface : MarshalByRefObject
    {
        public void IsInstalled(int pid)
        {
            Log.Add($"DLL injected, PID: {pid}");
        }

        public void ReceivedMessage(string msgPacket)
        {
            Log.Add(msgPacket);
        }
 
        public void HandleError(Exception e)
        {
            Log.Add(e.Message);
        }
    }

}