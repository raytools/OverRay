﻿using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct StandardGame
    {
        public int field_0;
        public int field_4;
        public int personalType;
        public IntPtr superObjectPtr;
    }
}