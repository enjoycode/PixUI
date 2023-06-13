namespace PixUI;

internal readonly struct DataGridHitTestResult<T>
{
    public DataGridHitTestResult(DataGridColumn<T> column, int rowIndex,
        float scrollDeltaX = 0, float scrollDeltaY = 0, bool isColumnResizer = false)
    {
        Column = column;
        RowIndex = rowIndex;
        ScrollDeltaX = scrollDeltaX;
        ScrollDeltaY = scrollDeltaY;
        IsColumnResizer = isColumnResizer;
    }

    internal readonly DataGridColumn<T> Column;
    internal readonly int RowIndex; //-1 == hit in header
    internal readonly float ScrollDeltaX;
    internal readonly float ScrollDeltaY;
    internal readonly bool IsColumnResizer;
}