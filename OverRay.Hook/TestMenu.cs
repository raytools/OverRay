using System;
using System.Collections.Generic;
using System.Linq;
using OverRay.Hook.Structs;
using OverRay.Hook.Utils;

namespace OverRay.Hook
{
    public class TestMenu
    {
        public TestMenu(GameFunctions game)
        {
            Game = game;
            Items = new List<MenuItem>();
            BasePosition = new Vector3 { X=3, Y=10, Z=0 };
            SelectedItem = 0;

            for (int i = 0; i < 5; i++)
                Items.Add(new MenuItem($"Item {i + 1}"));
        }

        private GameFunctions Game { get; }
        private List<MenuItem> Items { get; }
        private Vector3 BasePosition { get; }
        public bool MenuShown { get; set; }

        public int SelectedItem { get; set; }

        public void ToggleMenu()
        {
            if (MenuShown) HideMenu();
            else ShowMenu();
        }

        public void NextItem()
        {
            if (SelectedItem < Items.Count) SelectedItem++;
        }

        public void PreviousItem()
        {
            if (SelectedItem > 0) SelectedItem--;
        }

        private void ShowMenu()
        {
            Game.EngineActions["testMenu"] = () =>
            {
                Vector3 vpos2 = new Vector3 { X=BasePosition.X + 18, Y=BasePosition.Y + 2 + Items.Count * 5, Z=0 };
                using (StructPtr pos1 = new StructPtr(BasePosition), pos2 = new StructPtr(vpos2))
                {
                    Game.OVAddParticle(110, pos1, pos2, Game.blueSparkTexture, 190);
                }
            };

            Game.TextActions["testMenu"] = () =>
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    Game.CustomText(SelectedItem == i ? Items[i].Name.Yellow() : Items[i].Name, 12, (BasePosition.X + 1) * 10, (BasePosition.Y + 2 + i * 5) * 10);
                }
            };

            MenuShown = true;
        }

        private void HideMenu()
        {
            Game.EngineActions.Remove("testMenu");
            Game.TextActions.Remove("testMenu");

            MenuShown = false;
        }

        private class MenuItem
        {
            public MenuItem(string name)
            {
                Name = name;
            }

            public string Name { get; }
            public Action Action { get; set; }
        }
    }
}