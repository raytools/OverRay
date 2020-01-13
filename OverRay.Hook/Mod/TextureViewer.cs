using System;
using System.Collections.Generic;
using OverRay.Hook.Structs;
using OverRay.Hook.Types;
using OverRay.Hook.Utils;

namespace OverRay.Hook.Mod
{
    public class TextureViewer
    {
        public TextureViewer(GameManager manager)
        {
            Manager = manager;
            Items = TextureLoader.GetTextures(); //.Where(texture => (int) texture.Pointer > 0x00500000 && (int) texture.Pointer < 0x20000000).ToList();
        }

        private string Id { get; } = Guid.NewGuid().ToString();
        private GameManager Manager { get; }
        private Vector3 Pos1 { get; } = new Vector3(5, 5, 0);
        private Vector3 Pos2 { get; } = new Vector3(95, 92, 0);
        private List<Texture> Items { get; set; }

        private int _selected;
        private int Selected
        {
            get => _selected;
            set
            {
                if (value >= 0 && value < Items.Count)
                    _selected = value;
            }
        }

        private int Pointer => (int)Items[Selected].Pointer;

        public void Show()
        {
            Selected = 0;

            Manager.Input.DisableGameInput();
            Manager.Input.ExclusiveInput = ProcessInput;

            Manager.Hud.Hide();

            lock (Manager.Engine.EngineActions) Manager.Engine.EngineActions[Id] = DrawGraphics;
            lock (Manager.Text.TextActions) Manager.Text.TextActions[Id] = DrawText;
        }

        public void Hide()
        {
            lock (Manager.Engine.EngineActions) Manager.Engine.EngineActions[Id] = () =>
            {
                DrawEmptyParticle();
                Manager.Engine.EngineActions.Remove(Id);
            };
            lock (Manager.Text.TextActions) Manager.Text.TextActions.Remove(Id);

            Manager.Input.ExclusiveInput = null;
            Manager.Input.EnableGameInput();

            Manager.Hud.Show();
        }

        private void ProcessInput(char ch, KeyCode code)
        {
            if (code == KeyCode.Backspace)
            {
                Hide();
            }
            else if (code == KeyCode.Left)
            {
                Selected--;
            }
            else if (code == KeyCode.Right)
            {
                Selected++;
            }
            else if (code == KeyCode.Up)
            {
                Selected += 10;
            }
            else if (code == KeyCode.Down)
            {
                Selected -= 10;
            }
            else if (ch == 'c')
            {
                Detour.Interface.Log(Items[Selected].Name);
            }
            else if (ch == 'r')
            {
                Items = TextureLoader.GetTextures();
                Selected = 0;
            }
        }

        private void DrawGraphics()
        {
            using (StructPtr pos1 = new StructPtr(Pos1), pos2 = new StructPtr(Pos2))
            {
                Manager.Graphics.VAddParticle.Call(110, pos1, pos2, TexturePointers.blueSparkTexture, 190);
            }

            Vector3 rpos1 = new Vector3(Pos1.X + 15, Pos1.Y + 6, 0);
            Vector3 rpos2 = new Vector3(rpos1.X + 60, rpos1.Y + 65, 255);

            if (Pointer > 0x00500000 && Pointer < 0x20000000)
            {
                using (StructPtr pos1 = new StructPtr(rpos1), pos2 = new StructPtr(rpos2))
                {
                    Manager.Graphics.VAddParticle.Call(125, pos1, pos2, Pointer, 12);
                }
            }
            else DrawEmptyParticle();
        }

        private void DrawText()
        {
            Manager.Text.CustomText("TEXTURE VIEWER".Red(), 11, (Pos1.X + 2) * 10, (Pos1.Y + 1) * 10);
            Manager.Text.CustomText($"{Selected + 1} of {Items.Count}", 11, (Pos1.X + 50) * 10, (Pos1.Y + 1) * 10);
            Manager.Text.CustomText(Items[Selected].Name.Replace("_", " "), 6, (Pos1.X + 2) * 10, (Pos2.Y - 13) * 10);
            Manager.Text.CustomText("Arrows".Yellow() + " - browse", 9, (Pos1.X + 2) * 10, (Pos2.Y - 7) * 10);
            Manager.Text.CustomText("Backspace".Yellow() + " - exit", 9, (Pos1.X + 42) * 10, (Pos2.Y - 7) * 10);
            Manager.Text.CustomText("C".Yellow() + " - log texture name", 9, (Pos1.X + 2) * 10, (Pos2.Y - 4) * 10);
            Manager.Text.CustomText("R".Yellow() + " - reload textures", 9, (Pos1.X + 42) * 10, (Pos2.Y - 4) * 10);

            Manager.Text.CustomText("", 11, (Pos1.X + 2) * 10, (Pos1.Y + 5) * 10);

            if (Pointer < 0x00500000 || Pointer > 0x20000000)
            {
                Manager.Text.CustomText($"invalid pointer 0x{Convert.ToString(Pointer, 16)}".Red(), 9, (Pos1.X + 15) * 10, (Pos1.Y + 15) * 10);
            }
        }

        private void DrawEmptyParticle()
        {
            using (StructPtr zero = new StructPtr(Vector3.Zero))
            {
                Manager.Graphics.VAddParticle.Call(125, zero, zero, 0, 12);
            }
        }
    }
}