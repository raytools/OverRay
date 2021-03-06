﻿using OverRay.Hook.GameFunctions;
using OverRay.Hook.Utils;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OverRay.Hook.Mod
{
    public class GameManager
    {
        public GameManager()
        {
            InitHooks();

            Hud = new Hud(this);
            Hud.InitializeHud();

            InitMainMenu();
        }

        public EngineFunctions Engine { get; } = new EngineFunctions();
        public GfxFunctions Graphics { get; } = new GfxFunctions();
        public TextFunctions Text { get; } = new TextFunctions();
        public InputFunctions Input { get; } = new InputFunctions();

        public Hud Hud { get; }

        private Menu MainMenu { get; set; }
        private Menu LevelMenu { get; set; }

        private void InitHooks()
        {
            // Create all hooks here
            Engine.VEngine.CreateHook();
            Text.DrawsTexts.CreateHook();
            Input.VirtualKeyToAscii.CreateHook();
        }

        private void InitMainMenu()
        {
            IntPtr purpleFistPtr = Memory.GetPointerAtOffset((IntPtr)0x4B730C, 0x274, 0x790, 0x0, 0x4, 0x574);
            IntPtr glowFistPtr = purpleFistPtr + 4;

            InitLevelMenu();

            MainMenu = new Menu(this,
                new MenuItem("Change Level", LevelMenu),
                new MenuItem("Reload Level", () => Engine.AskToChangeLevel.Call(Engine.GetCurrentLevelName.Call(), 0)),
                new MenuItem("Purple Lum Power", new Menu(this,
                    new MenuItem("on", () => Marshal.WriteByte(purpleFistPtr, 0x40)),
                    new MenuItem("off", () => Marshal.WriteByte(purpleFistPtr, 0))
                )),
                new MenuItem("Glowfist Power", new Menu(this,
                    new MenuItem("on", () => Marshal.WriteInt32(glowFistPtr, 0x400000)),
                    new MenuItem("off", () => Marshal.WriteInt32(glowFistPtr, 0))
                )),
                new MenuItem("Texture Viewer", () =>
                {
                    TextureViewer viewer = new TextureViewer(this);
                    viewer.Show();
                })
            );

            Input.Actions.Set('m', () => MainMenu.Show());
            Text.Actions.Set("menutip", () => Text.CustomText("M".Yellow() + " - menu", 9, 850, 970));
        }

        private void InitLevelMenu()
        {
            List<MenuItem> items = new List<MenuItem>();

            foreach (KeyValuePair<string, Dictionary<string, string>> level in OtherUtils.Levels)
            {
                List<MenuItem> subItems = new List<MenuItem>();

                foreach (KeyValuePair<string, string> section in level.Value)
                    subItems.Add(new MenuItem(section.Key, () => Engine.AskToChangeLevel.Call(section.Value, 0)));

                items.Add(
                    subItems.Count == 1
                    ? new MenuItem(level.Key, subItems[0].Action)
                    : new MenuItem(level.Key, new Menu(this, subItems.ToArray()))
                    );
            }

            LevelMenu = new Menu(this, items.ToArray());
        }
    }
}