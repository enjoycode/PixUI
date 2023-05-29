using System;
using System.Collections.Generic;

namespace PixUI;

public enum MenuItemType
{
    MenuItem,
    SubMenu,
    Divider,
}

public sealed class MenuItem
{
    public MenuItemType Type { get; private set; }
    public readonly IconData? Icon;
    public readonly string? Label;
    public bool Enabled { get; set; }
    internal readonly Action? Action; //only for menu item

    public IList<MenuItem>? Children { get; private set; }
    //public readonly string? Shortcut;

    public static MenuItem Item(string label, IconData? icon = null, Action? action = null)
        => new MenuItem(MenuItemType.MenuItem, label, icon, action);

    public static MenuItem SubMenu(string label, IconData? icon, MenuItem[] children)
        => new MenuItem(MenuItemType.SubMenu, label, icon, null, children);

    public static MenuItem Divider() => new MenuItem(MenuItemType.Divider);

    private MenuItem(MenuItemType type, string? label = null, IconData? icon = null,
        Action? action = null, MenuItem[]? children = null, bool enabled = true)
    {
        Type = type;
        Label = label;
        Icon = icon;
        Action = action;
#if __WEB__
            Children = new List<MenuItem>(children);
#else
        Children = children;
#endif

        Enabled = enabled;
    }
}