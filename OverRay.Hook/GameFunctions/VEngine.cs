using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte VEngine();

        internal static readonly IntPtr VEnginePtr = (IntPtr)0x40ADA0;
        internal readonly VEngine OVEngine = Marshal.GetDelegateForFunctionPointer<VEngine>(VEnginePtr);

        internal byte HVEngine()
        {
            byte engine = OVEngine();

            try
            {
                // NOTE: All text should be drawn in HDrawsTexts()

                // GLM point particle
                IntPtr glmPtr = Memory.GetPointerAtOffset((IntPtr)0x500298, 0x234, 0x10, 0xC, 0xB0);
                OVCreatePart(24576, glmPtr, 0, 2, 2, 2, sparkTexture);

                // Text box background
                //Vector3 vpos1 = new Vector3 { X=14, Y=78, Z=0 };
                //Vector3 vpos2 = new Vector3 { X=94, Y=95, Z=0 };
                //using (Ptr pos1 = new Ptr(vpos1), pos2 = new Ptr(vpos2))
                //{
                //    OVAddParticle(110, pos1, pos2, blueSparkThatIsAlsoABoxTexture, 190);
                //}

            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return engine;
        }
    }
}