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
                Interface.ReceivedMessage($"Text: {text.text}");
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

            using (Ptr textPtr = new Ptr(textObject))
            {
                ODrawText(0x5004d4, textPtr);
            }
        }
    }
}