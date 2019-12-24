using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void VAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, float a6);

        internal static readonly IntPtr VAddParticlePtr = new IntPtr(0x463390);
        internal readonly VAddParticle OVAddParticle = Marshal.GetDelegateForFunctionPointer<VAddParticle>(VAddParticlePtr);

        private int sparkTexture = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr)0x502810, 0x120));
        private int blueSparkTexture = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr)0x502750, 0x2BC));

        private int lumIcon = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr)0x502790, 0xCC));
        private int cageIcon = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr)0x502790, 0x400));
        private int rayIcon = Marshal.ReadInt32(Memory.GetPointerAtOffset((IntPtr)0x5027D0, 0x228));

        internal void HVAddParticle(uint particleType, IntPtr vector, IntPtr vector2, int texture, float a6)
        {
            try
            {
                OVAddParticle(particleType, vector, vector2, texture, a6);

                Vector3 s1 = Marshal.PtrToStructure<Vector3>(vector);
                Vector3 s2 = Marshal.PtrToStructure<Vector3>(vector2);

                Interface.WriteLog($"VAddPart, Particle type: {particleType}, texture: 0x{Convert.ToString(texture, 16)}, a6: {a6}");
                Interface.WriteLog($"Vector1: {s1.X} {s1.Y} {s1.Z}, Vector2: {s2.X} {s2.Y} {s2.Z}");
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }
        }
    }
}