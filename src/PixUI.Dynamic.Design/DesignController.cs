using System.Collections.Generic;

namespace PixUI.Dynamic.Design;

public sealed class DesignController
{
    /// <summary>
    /// 设计画布缩放百分比
    /// </summary>
    public readonly State<int> Zoom = 100;

    private readonly List<DesignElement> _selection = new();

    public DesignElement? FirstSelected => _selection.Count > 0 ? _selection[0] : null;

    internal void Select(DesignElement element)
    {
        _selection.ForEach(o => o.IsSelected = false);
        _selection.Clear();

        _selection.Add(element);
        element.IsSelected = true;
    }
}