using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int WriteToViewportFinished(int _, short word5004D0);

        internal static readonly IntPtr WriteToViewportFinishedPtr = (IntPtr)0x420BB0;
        internal readonly WriteToViewportFinished OWriteToViewportFinished = Marshal.GetDelegateForFunctionPointer<WriteToViewportFinished>(WriteToViewportFinishedPtr);
    }
}