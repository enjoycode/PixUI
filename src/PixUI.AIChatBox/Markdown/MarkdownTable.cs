using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownTable : MarkdownBlock
{
    private readonly List<ColumnInfo> _columns = [];
    private readonly List<RowInfo> _rows = [];
    private float[] _maxColumnWidths = [];

    internal override float Height => _rows.Sum(r => r.Height) +
                                      _rows.Count * ChatBoxTheme.TableCellPadding * 2;

    internal override void Build(Block node, float availableWidth)
    {
        var table = (Table)node;

        _columns.Clear();
        foreach (var column in table.ColumnDefinitions)
            _columns.Add(new ColumnInfo() { Alignment = column.Alignment });

        _rows.Clear();
        _maxColumnWidths = new float[_columns.Count];
        foreach (var rowNode in table)
        {
            var row = (TableRow)rowNode;
            var rowInfo = new RowInfo();
            _rows.Add(rowInfo);
            var columnIndex = 0;
            foreach (var cellNode in row)
            {
                var cell = (TableCell)cellNode;
                if (cell.Count == 0) continue;
                if (cell[0] is ParagraphBlock paragraphNode)
                {
                    var cellParagraph = new MarkdownParagraph();
                    cellParagraph.Build(paragraphNode, availableWidth); //先按最大宽度布局
                    if (columnIndex < _maxColumnWidths.Length)
                        _maxColumnWidths[columnIndex] = Math.Max(_maxColumnWidths[columnIndex], cellParagraph.Width);
                    rowInfo.Cells.Add(cellParagraph);
                }

                columnIndex++;
            }
        }

        //重新计算合适的列宽
        CalcColumnWidths(availableWidth);
    }

    internal override void Relayout(float availableWidth)
    {
        // var totalWidth = _columns.Sum(r => r.Width) +
        //                  _columns.Count * ChatBoxTheme.TableCellPadding * 2;
        // if (totalWidth <= availableWidth) return;

        foreach (var column in _columns)
            column.Reset();

        CalcColumnWidths(availableWidth);
    }

    private void CalcColumnWidths(float availableWidth)
    {
        var totalWidth = availableWidth - ChatBoxTheme.TableCellPadding * 2 * _columns.Count;
        var avgColumnWidth = totalWidth / _columns.Count;
        var totalFixed = 0f;
        var fixedCount = 0;
        for (int i = 0; i < _columns.Count; i++)
        {
            if (_maxColumnWidths[i] <= avgColumnWidth)
            {
                _columns[i].Width = _maxColumnWidths[i];
                _columns[i].IsFixed = true;
                totalFixed += _maxColumnWidths[i];
                fixedCount++;
            }
        }

        if (fixedCount == _columns.Count) return;

        var leftWidth = totalWidth - totalFixed;
        var leftCount = _columns.Count - fixedCount;
        avgColumnWidth = leftWidth / leftCount;
        foreach (var column in _columns)
        {
            if (!column.IsFixed)
                column.Width = avgColumnWidth;
        }

        foreach (var row in _rows)
        {
            for (int j = 0; j < row.Cells.Count; j++)
            {
                row.Cells[j]?.Relayout(avgColumnWidth);
            }
        }
    }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var cellPadding = ChatBoxTheme.TableCellPadding;
        var offsetY = cellPadding;
        for (int i = 0; i < _rows.Count; i++)
        {
            var offsetX = cellPadding;
            var rowHeight = _rows[i].Height;
            for (int j = 0; j < _columns.Count; j++)
            {
                var cellRect = Rect.FromLTWH(offsetX - cellPadding, offsetY - cellPadding,
                    _columns[j].Width + cellPadding * 2,
                    rowHeight + cellPadding * 2);
                canvas.DrawRect(cellRect, Paint.Shared(Colors.Gray, PaintStyle.Stroke));

                if (_rows[i].Cells.Count - 1 < j) continue;
                var cell = _rows[i].Cells[j];
                if (cell == null) continue;
                canvas.Translate(offsetX, offsetY);
                cell.OnPaint(canvas, controller);
                canvas.Translate(-offsetX, -offsetY);
                offsetX += _columns[j].Width + cellPadding * 2;
            }

            offsetY += rowHeight + cellPadding * 2;
        }
    }

    private sealed class ColumnInfo
    {
        public TableColumnAlign? Alignment { get; set; }
        public float Width { get; set; }
        public bool IsFixed { get; set; }

        public void Reset()
        {
            Width = 0;
            IsFixed = false;
        }
    }

    private readonly struct RowInfo
    {
        public RowInfo() { }
        public List<MarkdownParagraph?> Cells { get; } = [];
        public float Height => Cells.Count > 0 ? Cells.Max(c => c?.Height ?? 0) : 0;
    }
}