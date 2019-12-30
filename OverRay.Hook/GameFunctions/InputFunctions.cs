using System;
using System.Runtime.InteropServices;
using OverRay.Hook.Utils;

namespace OverRay.Hook.GameFunctions
{
    public class InputFunctions
    {
        public InputFunctions()
        {
            VirtualKeyToAscii = new GameFunction<FVirtualKeyToAscii>(0x496110, HVirtualKeyToAscii);
            VReadInput = new GameFunction<FVReadInput>(0x496510, HVReadInput);
        }

        #region VirtualKeyToAscii

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate short FVirtualKeyToAscii(byte ch, int a2);

        public GameFunction<FVirtualKeyToAscii> VirtualKeyToAscii { get; }

        internal short HVirtualKeyToAscii(byte ch, int a2)
        {
            short result = VirtualKeyToAscii.Call(ch, a2);

            Detour.Interface.WriteLog($"VirtualKeyToAscii result: {(char)result}, char: {ch}, a2: {a2}");

            /*
            if ((char)result == 'm')
            {
                if (TestMenu.MenuShown || Marshal.ReadByte((IntPtr) 0x500faa) == 0)
                {
                    TestMenu.ToggleMenu();
                    Marshal.WriteByte((IntPtr)0x500faa, (byte)(TestMenu.MenuShown ? 1 : 0));
                }
            }

            if (TestMenu.MenuShown)
            {
                switch ((SpecialKeys)ch)
                {
                    case SpecialKeys.Up:
                        TestMenu.PreviousItem();
                        break;
                    case SpecialKeys.Down:
                        TestMenu.NextItem();
                        break;
                }
            }
            */

            return result;
        }

        #endregion

        #region VReadInput

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate short FVReadInput(int a1);

        public GameFunction<FVReadInput> VReadInput { get; }

        internal short HVReadInput(int a1)
        {
            short result = VReadInput.Call(a1);

            Detour.Interface.WriteLog($"VReadInput Output: {result}, Pointer:");
            Detour.Interface.WriteLog($"0x{Convert.ToString(a1, 16)}");

            return result;
        }

        #endregion
    }
}