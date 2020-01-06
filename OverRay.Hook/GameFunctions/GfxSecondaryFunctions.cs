using System.Runtime.InteropServices;
using OverRay.Hook.Types;

namespace OverRay.Hook.GameFunctions
{
    public class GfxSecondaryFunctions
    {
        public GfxSecondaryFunctions()
        {
            ClearZBufferRegion = new GameFunction<FClearZBufferRegion>(0x421FB0);
            SwapSceneBuffer = new GameFunction<FSwapSceneBuffer>(0x420F50);
            WriteToViewportFinished = new GameFunction<FWriteToViewportFinished>(0x420BB0);
        }

        #region ClearZBufferRegion

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FClearZBufferRegion(int unk1, int unk2, int unk3, int unk4);

        public GameFunction<FClearZBufferRegion> ClearZBufferRegion { get; }

        #endregion

        #region SwapSceneBuffer

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FSwapSceneBuffer();

        public GameFunction<FSwapSceneBuffer> SwapSceneBuffer { get; }

        #endregion

        #region WriteToViewportFinished

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int FWriteToViewportFinished(int _, short word5004D0);

        public GameFunction<FWriteToViewportFinished> WriteToViewportFinished;

        #endregion
    }
}