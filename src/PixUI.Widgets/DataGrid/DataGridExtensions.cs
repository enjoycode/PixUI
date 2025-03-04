using System;

namespace PixUI;

public static class DataGridExtensions
{
    public static DataGrid<T> AddRowNumColumn<T>(this DataGrid<T> grid, string label, ColumnWidth? width = null,
        bool frozen = true)
    {
        var column = new DataGridRowNumColumn<T>(label) { Frozen = frozen };
        if (width != null)
            column.Width = width;
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddTextColumn<T>(this DataGrid<T> grid, string label, Func<T, string> textGetter,
        ColumnWidth? width = null, CellStyle? cellStyle = null, bool autoMergeCell = false)
    {
        var column = new DataGridTextColumn<T>(label, textGetter) { AutoMergeCells = autoMergeCell };
        if (width != null)
            column.Width = width;
        if (cellStyle != null)
            column.CellStyle = cellStyle;
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddCheckboxColumn<T>(this DataGrid<T> grid, string label,
        Func<T, bool> cellValueGetter, Action<T, bool>? cellValueSetter = null, ColumnWidth? width = null)
    {
        var column = new DataGridCheckboxColumn<T>(label, cellValueGetter, cellValueSetter);
        if (width != null)
            column.Width = width;
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddIconColumn<T>(this DataGrid<T> grid, string label, Func<T, IconData?> cellValueGetter,
        ColumnWidth? width = null, Func<T, int, CellStyle>? cellStyleGetter = null)
    {
        var column = new DataGridIconColumn<T>(label, cellValueGetter, width);
        if (cellStyleGetter != null)
            column.CellStyleGetter = cellStyleGetter;
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddHostColumn<T>(this DataGrid<T> grid, string label, Func<T, int, Widget> cellBuilder,
        ColumnWidth? width = null)
    {
        var column = new DataGridHostColumn<T>(label, cellBuilder);
        if (width != null)
            column.Width = width;
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddButtonColumn<T>(this DataGrid<T> grid, string label, Func<T, int, Button> cellBuilder,
        ColumnWidth? width = null, bool frozen = true)
    {
        var column = new DataGridButtonColumn<T>(label, cellBuilder, width) { Frozen = frozen };
        grid.Columns.Add(column);
        return grid;
    }

    public static DataGrid<T> AddGroupColumn<T>(this DataGrid<T> grid, string label,
        out DataGridGroupColumn<T> groupColumn)
    {
        groupColumn = new DataGridGroupColumn<T>(label);
        grid.Columns.Add(groupColumn);
        return grid;
    }

    public static DataGrid<T> AddGroupColumnTo<T>(this DataGrid<T> grid, DataGridGroupColumn<T> group, string label,
        out DataGridGroupColumn<T> groupColumn)
    {
        groupColumn = new DataGridGroupColumn<T>(label);
        group.Add(groupColumn);
        return grid;
    }

    public static DataGrid<T> AddTextColumnTo<T>(this DataGrid<T> grid, DataGridGroupColumn<T> group, string label,
        Func<T, string> textGetter, ColumnWidth? width = null, CellStyle? cellStyle = null, bool autoMergeCell = false)
    {
        var column = new DataGridTextColumn<T>(label, textGetter) { AutoMergeCells = autoMergeCell };
        if (width != null)
            column.Width = width;
        if (cellStyle != null)
            column.CellStyle = cellStyle;

        group.Add(column);
        return grid;
    }

    public static DataGrid<T> AddIconColumnTo<T>(this DataGrid<T> grid, DataGridGroupColumn<T> group,
        string label, Func<T, IconData?> cellValueGetter,
        ColumnWidth? width = null, Func<T, int, CellStyle>? cellStyleGetter = null)
    {
        var column = new DataGridIconColumn<T>(label, cellValueGetter, width);
        if (cellStyleGetter != null)
            column.CellStyleGetter = cellStyleGetter;

        group.Add(column);
        return grid;
    }

    public static DataGrid<T> AddCheckboxColumnTo<T>(this DataGrid<T> grid, DataGridGroupColumn<T> group, string label,
        Func<T, bool> cellValueGetter, Action<T, bool>? cellValueSetter = null, ColumnWidth? width = null)
    {
        var column = new DataGridCheckboxColumn<T>(label, cellValueGetter, cellValueSetter);
        if (width != null)
            column.Width = width;
        group.Add(column);
        return grid;
    }
}