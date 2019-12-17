using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Text2D
    {
        public string text;
        public float positionX;
        public float positionY;
        public float size;
        public byte alphaByte;
        public byte gap11;
        public byte highlight;
        public byte options; // bit 4 = highlighting color , bit 5 = black background
        public int dword14;
        public byte flag3;
    }
}