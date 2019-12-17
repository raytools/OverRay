using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string GetCurrentLevelName();

        internal static readonly IntPtr GetCurrentLevelNamePtr = new IntPtr(0x404DA0);
        internal readonly GetCurrentLevelName OGetCurrentLevelName = Marshal.GetDelegateForFunctionPointer<GetCurrentLevelName>(GetCurrentLevelNamePtr);
    }
}