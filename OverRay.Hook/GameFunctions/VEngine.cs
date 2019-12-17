using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;

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
                IntPtr coordinatesPtr = Memory.GetPointerAtOffset((IntPtr)0x500560, 0x224, 0x310, 0x34, 0x0, 0x1ac);
                Vector3 coordinates = Marshal.PtrToStructure<Vector3>(coordinatesPtr);

                IntPtr glmPtr = Memory.GetPointerAtOffset((IntPtr)0x500298, 0x234, 0x10, 0xC, 0xB0);
                Vector3 glm = Marshal.PtrToStructure<Vector3>(glmPtr);

                string levelName = OGetCurrentLevelName();

                string coordinatesString = "X".KeyValue(coordinates.X.Float()).NL() +
                                           "Y".KeyValue(coordinates.Y.Float()).NL() +
                                           "Z".KeyValue(coordinates.Z.Float());

                string glmString = "GLM".Yellow() + ":X".KeyValue(glm.X.Float()).NL() +
                                   "::::Y".KeyValue(glm.Y.Float()).NL() +
                                   "::::Z".KeyValue(glm.Z.Float());

                CustomText(coordinatesString, 10, 5, 5);
                CustomText(glmString, 10, 300, 5);
                CustomText("Level".KeyValue(levelName), 10, 5, 970);

                OVCreatePart(24576, glmPtr, 0, 2, 2, 2, sparkTexture);
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }
            return engine;
        }
    }
}