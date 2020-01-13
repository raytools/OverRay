using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Types;

namespace OverRay.Hook.GameFunctions
{
    public class TextFunctions
    {
        public TextFunctions()
        {
            TextActions = new Dictionary<string, Action>();

            DrawsTexts = new GameFunction<FDrawsTexts>(0x460670, HDrawsTexts);
            DrawText = new GameFunction<FDrawText>(0x4660B0, HDrawText);
        }

        public Dictionary<string, Action> TextActions { get; }

        #region DrawsTexts

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte FDrawsTexts(int context);

        public GameFunction<FDrawsTexts> DrawsTexts { get; }

        private byte HDrawsTexts(int context)
        {
            try
            {
                lock (TextActions)
                {
                    foreach (KeyValuePair<string, Action> action in TextActions)
                        action.Value.Invoke();
                }
            }
            catch (Exception e)
            {
                Detour.Interface.HandleError(e);
            }

            return DrawsTexts.Call(context);
        }

        #endregion

        #region DrawText

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FDrawText(int a1, IntPtr textStruct);

        public GameFunction<FDrawText> DrawText { get; }

        private int HDrawText(int a1, IntPtr textStruct)
        {
            try
            {
                Text2D text = Marshal.PtrToStructure<Text2D>(textStruct);
                Detour.Interface.Log($"Text: {text.text} alphaByte: {text.alphaByte} gap11: {text.gap11}");
                Detour.Interface.Log($"highlight: {text.highlight} options: {text.options}");
                Detour.Interface.Log($"dword14: {text.dword14} flag3: {text.flag3}");
            }
            catch (Exception e)
            {
                Detour.Interface.HandleError(e);
            }

            return DrawText.Call(a1, textStruct);
        }

        #endregion

        public void CustomText(string text, float size, float x, float y, byte alpha = 255)
        {
            Text2D textStruct = CreateTextStruct(text, size, x, y, alpha);
            DrawTextStruct(textStruct);
        }

        public Text2D CreateTextStruct(string text, float size, float x, float y, byte alpha = 255)
        {
            return new Text2D
            {
                text = text,
                size = size,
                positionX = x,
                positionY = y,
                alphaByte = alpha
            };
        }

        public void DrawTextStruct(Text2D textObject)
        {
            using (StructPtr textPtr = new StructPtr(textObject))
            {
                DrawText.Call(0x5004D4, textPtr);
            }
        }
    }
}