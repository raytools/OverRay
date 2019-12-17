using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, int a6);

        internal static readonly IntPtr VAddParticlePtr = new IntPtr(0x463390);
        internal readonly VAddParticle OVAddParticle = Marshal.GetDelegateForFunctionPointer<VAddParticle>(VAddParticlePtr);

        private int sparkTexture = Marshal.ReadInt32(Memory.GetPointerAtOffset(new IntPtr(0x502810), 0xCC, 0x4C));

        internal void HVAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, int a6)
        {
            try
            {
                OVAddParticle(particleType, vector, vector2, texture, a6);
                Interface.ReceivedMessage($"Particle type: {particleType}, texture: {texture}");
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }
        }
    }
}