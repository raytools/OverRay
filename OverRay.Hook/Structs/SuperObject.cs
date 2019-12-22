using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SuperObject
    {
        public int type;
        public IntPtr engineObjectPtr;
    }
}