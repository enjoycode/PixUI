using System;

namespace PixUI;

public sealed class Form : MultiChildWidget<FormItem>
{
    public Form() { }

    private int _columns = 1;
    private HorizontalAlignment _labelAlignment = HorizontalAlignment.Right;
    private float _labelWidth = 120;
    private EdgeInsets _padding = EdgeInsets.All(5);
    private float _horizontalSpacing = 5f;
    private float _verticalSpacing = 5f;

    public int Columns
    {
        get => _columns;
        set
        {
            if (_columns == value) return;
            _columns = value;
            if (IsMounted) Invalidate(InvalidAction.Relayout);
        }
    }

    public EdgeInsets Padding
    {
        get => _padding;
        set
        {
            if (_padding == value) return;
            _padding = value;
            if (IsMounted) Invalidate(InvalidAction.Relayout);
        }
    }

    public HorizontalAlignment LabelAlignment
    {
        get => _labelAlignment;
        set
        {
            if (_labelAlignment == value) return;
            _labelAlignment = value;
            if (IsMounted) Invalidate(InvalidAction.Relayout);
        }
    }

    public float LabelWidth
    {
        get => _labelWidth;
        set
        {
            if (_labelWidth == value) return;
            _labelWidth = value;
            if (IsMounted) Invalidate(InvalidAction.Relayout);
        }
    }

    #region ====Widget Overrides====

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        //单列可用宽度
        var columnWidth = (width - (_columns - 1) * _horizontalSpacing
                                 - _padding.Left - _padding.Right) / _columns;

        var y = _padding.Top;
        var colIndex = 0;
        var rowHeight = 0f; //同一行中的最高的那个子组件
        for (var i = 0; i < _children.Count; i++)
        {
            var leftHeight = height - y;
            if (leftHeight <= 0) break;

            var child = _children[i];
            var span = Math.Min(child.ColumnSpan, _columns - colIndex);
            child.Layout(columnWidth * span + (span - 1) * _horizontalSpacing, leftHeight);
            child.SetPosition(
                _padding.Left + colIndex * _horizontalSpacing + colIndex * columnWidth, y);
            rowHeight = Math.Max(rowHeight, child.H);

            colIndex += span;
            if (colIndex == _columns)
            {
                y += _verticalSpacing + rowHeight;
                colIndex = 0;
                rowHeight = 0f;
            }
            else if (i == _children.Count - 1)
            {
                //eg:  | 1 | 2 |
                //     | 3 | 没有了
                y += rowHeight;
            }
        }

        SetSize(width, Math.Min(y + _padding.Bottom, height));
    }

    #endregion
}