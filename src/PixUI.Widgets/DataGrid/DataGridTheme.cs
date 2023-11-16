using System;

namespace PixUI;

public sealed class DataGridTheme
{
    private static readonly Lazy<DataGridTheme> _default = new(() => new DataGridTheme());
    public static DataGridTheme Default => _default.Value;

    public DataGridTheme(CellStyle? headerCellStyle = null, CellStyle? rowCellStyle = null)
    {
        DefaultHeaderCellStyle = headerCellStyle ?? new CellStyle
        {
            Color = Colors.Black, BackgroundColor = new Color(0xFFF5F7FA),
            HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeight.Bold,
        };

        DefaultRowCellStyle = rowCellStyle ?? new CellStyle { Color = Colors.Black };
    }

    public readonly CellStyle DefaultHeaderCellStyle;
    public readonly CellStyle DefaultRowCellStyle;

    public float RowHeight = 28f;
    public float CellPadding = 5.0f;

    public Color BorderColor = new(0xFFEBEEF5);

    public bool StripeRows = true;
    public Color StripeBgColor = new(0xFFFAFAFA);

    public bool HighlightingCurrentCell = false;
    public bool HighlightingCurrentRow = true;

    public Color HighlightRowBgColor = new(0x30263238);
}