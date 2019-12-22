using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int DrawText(int a1, IntPtr textStruct);

        internal static readonly IntPtr DrawTextPtr = (IntPtr)0x4660B0;
        internal readonly DrawText ODrawText = Marshal.GetDelegateForFunctionPointer<DrawText>(DrawTextPtr);

        internal int HDrawText(int a1, IntPtr textStruct)
        {
            try
            {
                Text2D text = Marshal.PtrToStructure<Text2D>(textStruct);
                Interface.WriteLog($"Text: {text.text} alphaByte: {text.alphaByte} gap11: {text.gap11}");
                Interface.WriteLog($"highlight: {text.highlight} options: {text.options}");
                Interface.WriteLog($"dword14: {text.dword14} flag3: {text.flag3}");
                Marshal.StructureToPtr(text, textStruct, true);
            }
            catch (Exception e)
            {
                Interface.HandleError(e);
            }

            return ODrawText(a1, textStruct);
        }

        internal void CustomText(string text, float size, float x, float y, byte alpha = 255)
        {
            Text2D textObject = new Text2D
            {
                text = text,
                size = size,
                positionX = x,
                positionY = y,
                alphaByte = alpha
            };

            using (StructPtr textPtr = new StructPtr(textObject))
            {
                ODrawText(0x5004D4, textPtr);
            }
        }
    }
}