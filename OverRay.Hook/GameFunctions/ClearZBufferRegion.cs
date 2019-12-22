using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int ClearZBufferRegion(int unk1, int unk2, int unk3, int unk4);

        internal static readonly IntPtr ClearZBufferRegionPtr = (IntPtr)0x421FB0;
        internal readonly ClearZBufferRegion OClearZBufferRegion = Marshal.GetDelegateForFunctionPointer<ClearZBufferRegion>(ClearZBufferRegionPtr);
    }
}