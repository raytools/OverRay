using System.Runtime.InteropServices;

namespace OverRay.Hook.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;
    }
}