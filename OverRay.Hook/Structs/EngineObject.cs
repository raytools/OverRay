using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EngineObject
    {
        public int p3dData;
        public IntPtr stdGamePtr;
        public int dynam;
        public int mind;
    }
}