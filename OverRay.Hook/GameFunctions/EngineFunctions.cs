using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OverRay.Hook.Utils;

namespace OverRay.Hook.GameFunctions
{
    public class EngineFunctions
    {
        public EngineFunctions()
        {
            EngineActions = new Dictionary<string, Action>();

            VEngine = new GameFunction<FVEngine>(0x40ADA0, HVEngine);
            GetCurrentLevelName = new GameFunction<FGetCurrentLevelName>(0x404DA0);
            Code4PersoLePlusProche = new GameFunction<FCode4PersoLePlusProche>(0x476960);
        }

        public Dictionary<string, Action> EngineActions { get; }

        #region VEngine

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte FVEngine();

        public GameFunction<FVEngine> VEngine { get; }

        public byte HVEngine()
        {
            byte engine = VEngine.Call();

            try
            {
                foreach (KeyValuePair<string, Action> pair in EngineActions)
                    pair.Value.Invoke();
            }
            catch (Exception e)
            {
                Detour.Interface.HandleError(e);
            }

            return engine;
        }

        #endregion

        #region GetCurrentLevelName

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate string FGetCurrentLevelName();

        public GameFunction<FGetCurrentLevelName> GetCurrentLevelName { get; }

        #endregion

        #region Code4PersoLePlusProche

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FCode4PersoLePlusProche(int a, int b, int c);

        public GameFunction<FCode4PersoLePlusProche> Code4PersoLePlusProche { get; }

        #endregion
    }
}