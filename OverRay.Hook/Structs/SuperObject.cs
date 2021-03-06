﻿using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct SuperObject
    {
        [FieldOffset(0x0)]
        public int type;
        [FieldOffset(0x4)]
        public IntPtr engineObjectPtr;
        [FieldOffset(0x24)]
        public IntPtr matrixPtr;
    }
}