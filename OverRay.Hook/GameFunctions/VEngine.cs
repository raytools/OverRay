using OverRay.Hook.Structs;
using OverRay.Hook.Utils;
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

                DrawTarget();
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return engine;
        }

        private void DrawTarget()
        {
            IntPtr raymanPtr = Marshal.ReadIntPtr((IntPtr)0x500578);
            SuperObject raymanSPO = Marshal.PtrToStructure<SuperObject>(raymanPtr);
            EngineObject engineObject = Marshal.PtrToStructure<EngineObject>(raymanSPO.engineObjectPtr);
            int stdGame = (int)engineObject.stdGamePtr;
            
            int[] interp = new int[] {0x00000071, 0x03020000, // Func_CibleLaPlusProcheavecAngles(, 1, 1, 10, Func_GetPersoSighting(), 160f, 40f);
                                          0x00000020, 0x0C030000, // 32,
                                          0x00000014, 0x0C030000, // 20,
                                          0x00000002, 0x0C030000, // 2,
                                          0x00000001, 0x0C030000, // 1,
                                          0x000000A3, 0x03030000, // Func_GetPersoSighting(),
                                          0x420C0000, 0x0D030000, // 35f
                                          0x420C0000, 0x0D030000, // 35f
                                          0x00000000, 0x00010000,
                };

            IntPtr interpArray = Marshal.AllocHGlobal(interp.Length * 4);
            for (int i = 0; i < interp.Length; i++) {
                Marshal.WriteInt32(interpArray, i * 4, interp[i]);
            }

            int[] param = new int[0x20]; // 0x20
            IntPtr paramArray = Marshal.AllocHGlobal(param.Length * 4);

            IntPtr interpPtrStart = interpArray + 0x8; // we start at the second node of the interpreter tree

            Ofn_p_stCode4PersoLePlusProche((int)raymanPtr, (int)interpPtrStart, (int)paramArray);

            int result = Marshal.ReadInt32(paramArray, 0);
            if (result != 0) {

                IntPtr resultPtr = (IntPtr)result;

                EngineObject targetEngineObject = Marshal.PtrToStructure<EngineObject>(resultPtr);
                StandardGame targetStdGame = Marshal.PtrToStructure<StandardGame>(targetEngineObject.stdGamePtr);
                SuperObject targetSpo = Marshal.PtrToStructure<SuperObject>(targetStdGame.superObjectPtr);
                Matrix targetSpoMatrix = Marshal.PtrToStructure<Matrix>(targetSpo.matrixPtr);

                Vector3 targetPos = new Vector3 { X = targetSpoMatrix.x, Y = targetSpoMatrix.y, Z = targetSpoMatrix.z };
                using (Ptr targetPosPtr = new Ptr(targetPos)) {
                    OVCreatePart(24576, targetPosPtr, 0, 2, 2, 2, sparkTexture);
                }
            }

            Marshal.FreeHGlobal(interpArray);
            Marshal.FreeHGlobal(paramArray);
        }
    }
}