using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int VCreatePart(int particleBehavior, IntPtr position, int a3, float a4, float a5, float a6, int texture);

        internal static readonly IntPtr VCreatePartPtr = (IntPtr)0x4600C0;
        internal readonly VCreatePart OVCreatePart = Marshal.GetDelegateForFunctionPointer<VCreatePart>(VCreatePartPtr);

        internal int HVCreatePart(int particleBehavior, IntPtr position, int a3, float a4, float a5, float a6, int texture)
        {
            try
            {
                int part = OVCreatePart(particleBehavior, position, a3, a4, a5, a6, texture);
                Interface.WriteLog($"VCreatePart, Particle behavior: {particleBehavior}, texture: 0x{Convert.ToString(texture, 16)}");

                return part;
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return 0;
        }
    }
}