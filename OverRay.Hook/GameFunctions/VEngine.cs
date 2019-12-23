using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte VEngine();

        internal static readonly IntPtr VEnginePtr = (IntPtr)0x40ADA0;
        internal readonly VEngine OVEngine = Marshal.GetDelegateForFunctionPointer<VEngine>(VEnginePtr);

        public List<Texture> textures = new List<Texture>();

        private int test1;
        private int frameCounter;

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
                //using (StructPtr pos1 = new StructPtr(vpos1), pos2 = new StructPtr(vpos2))
                //{
                //    OVAddParticle(110, pos1, pos2, blueSparkTexture, 190);
                //}

                // Text box icon
                //Vector3 rpos1 = new Vector3 { X=4, Y=80, Z=0 };
                //Vector3 rpos2 = new Vector3 { X=14, Y=92, Z=200 };
                //using (StructPtr pos1 = new StructPtr(rpos1), pos2 = new StructPtr(rpos2))
                //{
                //    OVAddParticle(125, pos1, pos2, rayIcon, 5);
                //}

                
                if (test1 + 1 < textures.Count)
                {
                    if (frameCounter > 3)
                    {
                        frameCounter = 0;
                        test1++;
                    }

                    int pointer = (int) textures[test1].Pointer;

                    if (pointer > 0x00500000 && pointer < 0x20000000)
                    {
                        Vector3 rpos1 = new Vector3 { X=10, Y=10, Z=0 };
                        Vector3 rpos2 = new Vector3 { X=40, Y=40, Z=255 };
                        using (StructPtr pos1 = new StructPtr(rpos1), pos2 = new StructPtr(rpos2))
                        {
                            OVAddParticle(125, pos1, pos2, (int)textures[test1].Pointer, 11);
                            //Interface.WriteLog(textures[test1].Name);
                            //Interface.WriteLog("0x"+Convert.ToString(pointer, 16));
                        }
                    }
                    else
                    {
                        Interface.WriteLog($"Skipping 0x{Convert.ToString(pointer, 16)}: {textures[test1].Name}");
                    }
                    
                    frameCounter++;
                }
                
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return engine;
        }
    }
}