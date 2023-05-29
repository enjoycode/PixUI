using System;

namespace PixUI;

/// <summary>
/// 右键菜单
/// </summary>
public static class ContextMenu
{
    public static void Show(MenuItem[] menuItems)
    {
        var controller = new MenuController();
        controller.ShowContextMenu(menuItems);
    }
}