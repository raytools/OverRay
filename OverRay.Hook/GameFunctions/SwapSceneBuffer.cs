using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SwapSceneBuffer();

        internal static readonly IntPtr SwapSceneBufferPtr = (IntPtr)0x420F50;
        internal readonly SwapSceneBuffer OSwapSceneBuffer = Marshal.GetDelegateForFunctionPointer<SwapSceneBuffer>(SwapSceneBufferPtr);
    }
}