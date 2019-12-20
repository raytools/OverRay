using System;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Utils
{
    public class Ptr : IDisposable
    {
        public Ptr(object obj)
        {
            if (obj != null)
            {
                Pointer = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
                Marshal.StructureToPtr(obj, Pointer, false);
            }
            else Pointer = IntPtr.Zero;
        }

        public IntPtr Pointer { get; private set; }

        public static implicit operator IntPtr(Ptr ptr) => ptr.Pointer;


        private void ReleaseUnmanagedResources()
        {
            if (Pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(Pointer);
                Pointer = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~Ptr() => ReleaseUnmanagedResources();
    }
}