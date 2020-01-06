using System;

namespace OverRay.Hook.Mod
{
    public class MenuItem
    {
        public MenuItem(string name, Action action)
        {
            Name = name;
            Action = action;
            IsSubmenu = false;
        }

        public MenuItem(string name, Menu submenu)
        {
            Name = name;
            Submenu = submenu;
            IsSubmenu = true;
        }

        public string Name { get; }
        public bool IsSubmenu { get; }

        public Action Action { get; }
        public Menu Submenu { get; }
    }
}