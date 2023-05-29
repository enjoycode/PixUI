using System.Collections.Generic;

namespace PixUI;

internal readonly struct CellCache<T>
{
    internal readonly int RowIndex;
    internal readonly T? CachedItem;

    internal CellCache(int rowIndex, T? item)
    {
        RowIndex = rowIndex;
        CachedItem = item;
    }
}

internal sealed class CellCacheComparer<T> : IComparer<CellCache<T>>
{
    public int Compare(CellCache<T> x, CellCache<T> y)
        => x.RowIndex.CompareTo(y.RowIndex);
}