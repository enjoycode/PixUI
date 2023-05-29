namespace PixUI;

public sealed class DataGridTheme
{
    private static DataGridTheme? _default;
    public static DataGridTheme Default => _default ??= new DataGridTheme();

    public DataGridTheme()
    {
        DefaultHeaderCellStyle = new CellStyle()
        {
            Color = Colors.Black, BackgroundColor = new Color(0xFFF5F7FA),
            HorizontalAlignment = HorizontalAlignment.Center, FontWeight = FontWeight.Bold,
        };

        DefaultRowCellStyle = new CellStyle() { Color = Colors.Black };
    }

    public readonly CellStyle DefaultHeaderCellStyle;
    public readonly CellStyle DefaultRowCellStyle;

    public float RowHeight = 28f;
    public float CellPadding = 5.0f;

    public Color BorderColor = new Color(0xFFEBEEF5);

    public bool StripeRows = true;
    public Color StripeBgColor = new Color(0xFFFAFAFA);

    public bool HighlightingCurrentCell = false;
    public bool HighlightingCurrentRow = true;

    public Color HighlightRowBgColor = new Color(0x30263238);
}