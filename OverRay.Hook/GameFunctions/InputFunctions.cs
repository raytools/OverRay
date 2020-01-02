using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OverRay.Hook.Structs;
using OverRay.Hook.Utils;

namespace OverRay.Hook.GameFunctions
{
    public class InputFunctions
    {
        public InputFunctions()
        {
            InputActions = new Dictionary<char, Action>();
            InputCodeActions = new Dictionary<KeyCodes, Action>();

            VirtualKeyToAscii = new GameFunction<FVirtualKeyToAscii>(0x496110, HVirtualKeyToAscii);
            VReadInput = new GameFunction<FVReadInput>(0x496510, HVReadInput);
        }

        public Dictionary<char, Action> InputActions { get; }
        public Dictionary<KeyCodes, Action> InputCodeActions { get; }

        public Action<char, byte> ExclusiveInput { get; set; }

        #region VirtualKeyToAscii

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate short FVirtualKeyToAscii(byte ch, int a2);

        public GameFunction<FVirtualKeyToAscii> VirtualKeyToAscii { get; }

        private short HVirtualKeyToAscii(byte ch, int a2)
        {
            short result = VirtualKeyToAscii.Call(ch, a2);

            Detour.Interface.WriteLog($"VirtualKeyToAscii result: {(char)result}, char: {ch}, a2: {a2}");

            if (ExclusiveInput == null)
            {
                lock (InputActions)
                {
                    lock (InputCodeActions)
                    {
                        if (InputActions.TryGetValue((char) result, out Action action) ||
                            InputCodeActions.TryGetValue((KeyCodes) ch, out action))
                            action.Invoke();
                    }
                }
            }
            else ExclusiveInput.Invoke((char)result, ch);

            return result;
        }

        #endregion

        #region VReadInput

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate short FVReadInput(int a1);

        public GameFunction<FVReadInput> VReadInput { get; }

        private short HVReadInput(int a1)
        {
            short result = VReadInput.Call(a1);

            Detour.Interface.WriteLog($"VReadInput Output: {result}, Pointer:");
            Detour.Interface.WriteLog($"0x{Convert.ToString(a1, 16)}");

            return result;
        }

        #endregion
    }
}