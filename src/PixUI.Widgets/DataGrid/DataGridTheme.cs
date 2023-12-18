using System;

namespace PixUI;

public sealed class DataGridTheme //TODO: rename to DataGridStyles
{
    private static readonly Lazy<DataGridTheme> _default = new(() => new DataGridTheme());
    public static DataGridTheme Default => _default.Value;

    public DataGridTheme(CellStyle? headerCellStyle = null, CellStyle? rowCellStyle = null)
    {
        DefaultHeaderCellStyle = headerCellStyle ?? new CellStyle
        {
            TextColor = Colors.Black,
            FillColor = 0xFFF5F7FA,
            HorizontalAlignment = HorizontalAlignment.Center,
            FontWeight = FontWeight.Bold,
        };

        DefaultRowCellStyle = rowCellStyle ?? new CellStyle { TextColor = Colors.Black, FillColor = Colors.White };
    }

    public readonly CellStyle DefaultHeaderCellStyle;
    public readonly CellStyle DefaultRowCellStyle;

    public float RowHeight = 28f;
    public float CellPadding = 5.0f;

    public Color BorderColor = 0xFFEBEEF5;

    public bool StripeRows = true;
    public Color StripeFillColor = 0xFFFAFAFA;

    public bool HighlightingCurrentCell = false;
    public bool HighlightingCurrentRow = true;

    public Color HighlightRowFillColor = 0x30263238;
}