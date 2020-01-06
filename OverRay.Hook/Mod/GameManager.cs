using System;
using System.Runtime.InteropServices;
using OverRay.Hook.GameFunctions;
using OverRay.Hook.Utils;

namespace OverRay.Hook.Mod
{
    public class GameManager
    {
        public GameManager()
        {
            InitHooks();

            Hud = new Hud(this);
            Hud.InitializeHud();

            // menu test - do not delete until menu system is finished

            IntPtr purpleFistPtr = Memory.GetPointerAtOffset((IntPtr) 0x4B730C, 0x274, 0x790, 0x0, 0x4, 0x574);
            IntPtr glowFistPtr = Memory.GetPointerAtOffset((IntPtr) 0x4B730C, 0x274, 0x790, 0x0, 0x4, 0x578);

            TestMenu = new Menu(this,
                new MenuItem("go to learn30", () => Engine.AskToChangeLevel.Call("learn_30", 0)),
                new MenuItem("purple lum power", new Menu(this,
                    new MenuItem("on", () => Marshal.WriteByte(purpleFistPtr, 0x40)),
                    new MenuItem("off", () => Marshal.WriteByte(purpleFistPtr, 0))
                )),
                new MenuItem("glowfist power", new Menu(this,
                    new MenuItem("on", () => Marshal.WriteInt32(glowFistPtr, 0x400000)),
                    new MenuItem("off", () => Marshal.WriteInt32(glowFistPtr, 0))
                ))
            );

            Input.InputActions.Add('m', () => TestMenu.Show());
        }

        public EngineFunctions Engine { get; } = new EngineFunctions();
        public GfxFunctions Graphics { get; } = new GfxFunctions();
        public TextFunctions Text { get; } = new TextFunctions();
        public InputFunctions Input { get; } = new InputFunctions();

        private Hud Hud { get; }

        private Menu TestMenu { get; }

        private void InitHooks()
        {
            // Create all hooks here
            Engine.VEngine.CreateHook();
            Text.DrawsTexts.CreateHook();
            Input.VirtualKeyToAscii.CreateHook();
        }
    }
}