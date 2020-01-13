using System;
using System.Runtime.InteropServices;
using EasyHook;

namespace OverRay.Hook.Types
{
    public class GameFunction<T> where T : Delegate
    {
        public GameFunction(int pointer)
        {
            Name = Guid.NewGuid().ToString();
            Pointer = (IntPtr) pointer;
            Call = Marshal.GetDelegateForFunctionPointer<T>(Pointer);
        }

        public GameFunction(int pointer, T hook) : this(pointer)
        {
            Hook = hook;
        }

        private string Name { get; }
        public IntPtr Pointer { get; }
        public T Call { get; }
        private T Hook { get; }

        public void CreateHook()
        {
            Detour.Hooks[Name] = LocalHook.Create(Pointer, Hook, this);
            Detour.Hooks[Name].ThreadACL.SetExclusiveACL(new[] {0});
            Detour.Interface.Log($"Attached hook:\n{typeof(T).FullName}");
        }

        public void DeleteHook()
        {
            Detour.Hooks[Name].Dispose();
            Detour.Hooks.Remove(Name);
        }
    }
}