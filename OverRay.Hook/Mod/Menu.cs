using System;
using System.Collections.Generic;
using OverRay.Hook.Structs;
using OverRay.Hook.Types;
using OverRay.Hook.Utils;

namespace OverRay.Hook.Mod
{
    public class Menu
    {
        public Menu(GameManager manager, params MenuItem[] items)
        {
            Manager = manager;
            Items = new List<MenuItem>(items);

            foreach (MenuItem item in items)
            {
                int itemWidth = item.Name.Length * 2;
                if (itemWidth > Width)
                    Width = itemWidth;
            }
        }

        public Menu(GameManager manager, Vector3 position, float width, params MenuItem[] items) : this(manager, items)
        {
            Position = position;
            Width = width;
        }

        private string Id { get; } = Guid.NewGuid().ToString();
        private GameManager Manager { get; }
        private Menu ParentMenu { get; set; }
        private Vector3 Position { get; } = new Vector3 { X=3, Y=10, Z=0 };
        private float Width { get; }

        public List<MenuItem> Items { get; }

        private int _selected;
        public int Selected
        {
            get => _selected;
            set
            {
                if (value >= 0 && value < Items.Count)
                    _selected = value;
            }
        }

        public void Show(Menu parentMenu = null)
        {
            ParentMenu = parentMenu;
            Selected = 0;

            Manager.Input.DisableGameInput();
            Manager.Input.ExclusiveInput = ProcessInput;

            Manager.Engine.EngineActions[Id] = DrawGraphics;
            Manager.Text.TextActions[Id] = DrawText;
        }

        public void Hide()
        {
            Manager.Engine.EngineActions.Remove(Id);
            Manager.Text.TextActions.Remove(Id);

            Manager.Input.ExclusiveInput = null;
            Manager.Input.EnableGameInput();
        }

        private void ProcessInput(char ch, KeyCode code)
        {
            if (code == KeyCode.Enter)
            {
                Hide();
                if (Items[Selected].IsSubmenu) Items[Selected].Submenu.Show(this);
                else Items[Selected].Action.Invoke();
            }
            else if (code == KeyCode.Backspace)
            {
                Hide();
                ParentMenu?.Show();
            }
            else if (code == KeyCode.Up)
            {
                Selected--;
            }
            else if (code == KeyCode.Down)
            {
                Selected++;
            }
        }

        private void DrawGraphics()
        {
            Vector3 vpos2 = new Vector3 { X=Position.X + Width + 2, Y=Position.Y + 2 + Items.Count * 4, Z=0 };
            using (StructPtr pos1 = new StructPtr(Position), pos2 = new StructPtr(vpos2))
            {
                Manager.Graphics.VAddParticle.Call(110, pos1, pos2, TexturePointers.blueSparkTexture, 190);
            }
        }

        private void DrawText()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                Manager.Text.CustomText(i == Selected ? Items[i].Name.Yellow(): Items[i].Name, 9, (Position.X + 1) * 10, (Position.Y + 2 + i * 4) * 10);
            }
        }
    }
}