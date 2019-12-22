using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, float a6);

        internal static readonly IntPtr VAddParticlePtr = new IntPtr(0x463390);
        internal readonly VAddParticle OVAddParticle = Marshal.GetDelegateForFunctionPointer<VAddParticle>(VAddParticlePtr);

        private int sparkTexture = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr) 0x502810, 0xCC, 0x4C));
        private int blueSparkThatIsAlsoABoxTexture = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr) 0x502738, 0x7E4));

        internal void HVAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, float a6)
        {
            try
            {
                OVAddParticle(particleType, vector, vector2, texture, a6);
                Interface.WriteLog($"VAddPart, Particle type: {particleType}, texture: 0x{Convert.ToString(texture, 16)}, a6: {a6}");
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }
        }
    }
}