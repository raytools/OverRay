using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte DrawsTexts(int context);

        internal static readonly IntPtr DrawsTextsPtr = (IntPtr)0x460670;
        internal readonly DrawsTexts ODrawsTexts = Marshal.GetDelegateForFunctionPointer<DrawsTexts>(DrawsTextsPtr);

        internal byte HDrawsTexts(int context)
        {
            try
            {
                // HUD text
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

                int textSize = 9;

                CustomText(coordinatesString, textSize, 5, 5);
                CustomText(glmString, textSize, 280, 5);
                CustomText("Level".KeyValue(levelName), textSize, 640, 5);
                CustomText($"OverRay:v{OtherUtils.GetVersion}".Yellow(), textSize, 5, 970);

            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return ODrawsTexts(context);
        }
    }
}