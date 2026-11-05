using System.Runtime.Intrinsics.X86;
using Markdig.Extensions.Tables;
using Markdig.Syntax;

namespace PixUI.AIChatBox;

internal sealed class MarkdownTable : MarkdownBlock
{
    private readonly List<ColumnInfo> _columns = [];
    private readonly List<RowInfo> _rows = [];

    internal override float Height => _rows.Sum(r => r.Height) + _rows.Count * CELL_PADDING * 2;
    private const float CELL_PADDING = 2f;

    internal override void Layout(Block node, ChatBoxController controller)
    {
        var table = (Table)node;

        _columns.Clear();
        foreach (var column in table.ColumnDefinitions)
            _columns.Add(new ColumnInfo() { Alignment = column.Alignment });

        _rows.Clear();
        var maxColumnWidths = new float[_columns.Count];
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
                    cellParagraph.Layout(paragraphNode, controller); //先按最大宽度布局
                    if (columnIndex < maxColumnWidths.Length)
                        maxColumnWidths[columnIndex] = Math.Max(maxColumnWidths[columnIndex], cellParagraph.Width);
                    rowInfo.Cells.Add(cellParagraph);
                }

                columnIndex++;
            }
        }

        //重新计算合适的列宽
        var totalWidth = controller.Width - CELL_PADDING * (_columns.Count + 1);
        var avgColumnWidth = totalWidth / _columns.Count;
        var totalFixed = 0f;
        var fixedCount = 0;
        for (int i = 0; i < _columns.Count; i++)
        {
            if (maxColumnWidths[i] <= avgColumnWidth)
            {
                _columns[i].Width = maxColumnWidths[i];
                _columns[i].IsFixed = true;
                totalFixed += maxColumnWidths[i];
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

    internal override void Relayout(float availableWidth) { }

    internal override void OnPaint(ICanvas canvas, ChatBoxController controller)
    {
        var offsetY = CELL_PADDING;
        for (int i = 0; i < _rows.Count; i++)
        {
            var offsetX = CELL_PADDING;
            var rowHeight = _rows[i].Height;
            for (int j = 0; j < _columns.Count; j++)
            {
                var cellRect = Rect.FromLTWH(offsetX - CELL_PADDING, offsetY - CELL_PADDING,
                    _columns[j].Width + CELL_PADDING * 2, rowHeight + CELL_PADDING * 2);
                canvas.DrawRect(cellRect, Paint.Shared(Colors.Gray, PaintStyle.Stroke));

                var cell = _rows[i].Cells[j];
                if (cell == null) continue;
                canvas.Translate(offsetX, offsetY);
                cell.OnPaint(canvas, controller);
                canvas.Translate(-offsetX, -offsetY);
                offsetX += _columns[j].Width + CELL_PADDING * 2;
            }

            offsetY += rowHeight + CELL_PADDING * 2;
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
        public float Height => Cells.Max(c => c?.Height ?? 0);
    }
}