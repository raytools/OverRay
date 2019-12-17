using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int VCreatePart(int particleBehavior, IntPtr position, int a3, float a4, float a5, float a6, int texture);

        internal static readonly IntPtr VCreatePartPtr = (IntPtr)0x4600C0;
        internal readonly VCreatePart OVCreatePart = Marshal.GetDelegateForFunctionPointer<VCreatePart>(VCreatePartPtr);
    }
}