using System;

namespace PixUI;

public sealed class Form : MultiChildWidget<FormItem>
{
    private int _columns = 1;
    private float _labelWidth = 120;
    private EdgeInsets _padding = EdgeInsets.All(5);
    private float _horizontalSpacing = 5f;
    private float _verticalSpacing = 5f;
    private Color? _textColor;
    private float? _fontSize;

    public int Columns
    {
        get => _columns;
        set
        {
            if (_columns == value) return;
            _columns = value;
            Relayout();
        }
    }

    public EdgeInsets Padding
    {
        get => _padding;
        set
        {
            if (_padding == value) return;
            _padding = value;
            Relayout();
        }
    }

    public float LabelWidth
    {
        get => _labelWidth;
        set
        {
            if (_labelWidth == value) return;
            _labelWidth = value;
            Relayout();
        }
    }

    public Color? TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            foreach (var child in _children)
            {
                if (!child.TextColor.HasValue)
                {
                    child.ClearCache();
                    child.Repaint();
                }
            }
        }
    }

    public float? FontSize
    {
        get => _fontSize;
        set
        {
            _fontSize = value;
            foreach (var child in _children)
            {
                if (!child.FontSize.HasValue)
                {
                    child.ClearCache();
                    child.Repaint();
                }
            }
        }
    }

    #region ====Widget Overrides====

    protected override void OnLayout(Size maxSize)
    {
        //单列可用宽度
        var columnWidth = (maxSize.Width - (_columns - 1) * _horizontalSpacing
                                     - _padding.Left - _padding.Right) / _columns;

        var y = _padding.Top;
        var colIndex = 0;
        var rowHeight = 0f; //同一行中的最高的那个子组件
        for (var i = 0; i < _children.Count; i++)
        {
            var leftHeight = maxSize.Height - y;
            if (leftHeight <= 0) break;

            var child = _children[i];
            var span = Math.Min(child.ColumnSpan, _columns - colIndex);
            child.PerformLayout(new(columnWidth * span + (span - 1) * _horizontalSpacing, leftHeight));
            child.SetLayoutLocation(_padding.Left + colIndex * _horizontalSpacing + colIndex * columnWidth, y);
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

        SetLayoutSize(maxSize.Width, Math.Min(y + _padding.Bottom, maxSize.Height));
    }

    #endregion
}