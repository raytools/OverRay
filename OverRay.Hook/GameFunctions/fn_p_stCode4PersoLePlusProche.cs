using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook
{
    public partial class GameFunctions
    {
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int fn_p_stCode4PersoLePlusProche(int a, int b, int c);

        internal static readonly IntPtr fn_p_stCode4PersoLePlusProchePtr = new IntPtr(0x476960);
        internal readonly fn_p_stCode4PersoLePlusProche Ofn_p_stCode4PersoLePlusProche = Marshal.GetDelegateForFunctionPointer<fn_p_stCode4PersoLePlusProche>(fn_p_stCode4PersoLePlusProchePtr);
    }
}