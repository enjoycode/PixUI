using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CodeEditor;

public interface IBookmarkFactory
{
    Bookmark CreateBookmark(Document document, TextLocation location);
}

/// <summary>
/// This class handles the bookmarks for a buffer
/// </summary>
public sealed class BookmarkManager
{
    internal BookmarkManager(Document document, LineManager lineManager)
    {
        _document = document;
    }

    private readonly Document _document;
    private readonly List<Bookmark> _bookmarks = new();

    /// <value>
    /// Contains all bookmarks
    /// </value>
    public ReadOnlyCollection<Bookmark> Marks => new(_bookmarks);

    public Document Document => _document;

    /// <summary>
    /// Gets/Sets the bookmark factory used to create bookmarks for "ToggleMarkAt".
    /// </summary>
    public IBookmarkFactory? Factory { get; set; }

    /// <summary>
    /// Sets the mark at the line <code>location.Line</code> if it is not set, if the
    /// line is already marked the mark is cleared.
    /// </summary>
    public void ToggleMarkAt(TextLocation location)
    {
        var newMark = CreateMark(location);
        var newMarkType = newMark.GetType();

        for (var i = 0; i < _bookmarks.Count; ++i)
        {
            var mark = _bookmarks[i];
            if (mark.LineNumber == location.Line && mark.CanToggle && mark.GetType() == newMarkType)
            {
                _bookmarks.RemoveAt(i);
                OnRemoved(new BookmarkEventArgs(mark));
                return;
            }
        }

        AddMark(newMark);
    }

    public Bookmark CreateMark(TextLocation location)
    {
        return Factory != null ? Factory.CreateBookmark(_document, location) : new Bookmark(_document, location);
    }

    public void AddMark(Bookmark mark)
    {
        _bookmarks.Add(mark);
        OnAdded(new BookmarkEventArgs(mark));
    }

    public void RemoveMark(Bookmark mark)
    {
        _bookmarks.Remove(mark);
        OnRemoved(new BookmarkEventArgs(mark));
    }

    public void RemoveMarks(Predicate<Bookmark> predicate)
    {
        for (var i = 0; i < _bookmarks.Count; ++i)
        {
            var bm = _bookmarks[i];
            if (predicate(bm))
            {
                _bookmarks.RemoveAt(i--);
                OnRemoved(new BookmarkEventArgs(bm));
            }
        }
    }

    /// <returns>
    /// true, if a mark at mark exists, otherwise false
    /// </returns>
    public bool IsMarked(int lineNr)
    {
        for (var i = 0; i < _bookmarks.Count; ++i)
        {
            if (_bookmarks[i].LineNumber == lineNr)
            {
                return true;
            }
        }

        return false;
    }

    /// <remarks>
    /// Clears all bookmark
    /// </remarks>
    public void Clear()
    {
        foreach (var mark in _bookmarks)
        {
            OnRemoved(new BookmarkEventArgs(mark));
        }

        _bookmarks.Clear();
    }

    /// <value>
    /// The lowest mark, if no marks exists it returns -1
    /// </value>
    public Bookmark? GetFirstMark(Predicate<Bookmark> predicate)
    {
        if (_bookmarks.Count < 1)
            return null;

        Bookmark? first = null;
        for (var i = 0; i < _bookmarks.Count; ++i)
        {
            if (predicate(_bookmarks[i]) && _bookmarks[i].IsEnabled &&
                (first == null || _bookmarks[i].LineNumber < first.LineNumber))
            {
                first = _bookmarks[i];
            }
        }

        return first;
    }

    /// <value>
    /// The highest mark, if no marks exists it returns -1
    /// </value>
    public Bookmark? GetLastMark(Predicate<Bookmark> predicate)
    {
        if (_bookmarks.Count < 1)
            return null;

        Bookmark? last = null;
        for (var i = 0; i < _bookmarks.Count; ++i)
        {
            if (predicate(_bookmarks[i]) && _bookmarks[i].IsEnabled &&
                (last == null || _bookmarks[i].LineNumber > last.LineNumber))
            {
                last = _bookmarks[i];
            }
        }

        return last;
    }

    private bool AcceptAnyMarkPredicate(Bookmark mark)
    {
        return true;
    }

    public Bookmark? GetNextMark(int curLineNr)
    {
        return GetNextMark(curLineNr, AcceptAnyMarkPredicate);
    }

    /// <remarks>
    /// returns first mark higher than <code>lineNr</code>
    /// </remarks>
    /// <returns>
    /// returns the next mark > cur, if it not exists it returns FirstMark()
    /// </returns>
    public Bookmark? GetNextMark(int curLineNr, Predicate<Bookmark> predicate)
    {
        if (_bookmarks.Count == 0)
            return null;

        var next = GetFirstMark(predicate);
        foreach (var mark in _bookmarks)
        {
            if (predicate(mark) && mark.IsEnabled && mark.LineNumber > curLineNr)
            {
                if (mark.LineNumber < next.LineNumber || next.LineNumber <= curLineNr)
                {
                    next = mark;
                }
            }
        }

        return next;
    }

    public Bookmark? GetPrevMark(int curLineNr)
    {
        return GetPrevMark(curLineNr, AcceptAnyMarkPredicate);
    }

    /// <remarks>
    /// returns first mark lower than <code>lineNr</code>
    /// </remarks>
    /// <returns>
    /// returns the next mark lower than cur, if it not exists it returns LastMark()
    /// </returns>
    public Bookmark? GetPrevMark(int curLineNr, Predicate<Bookmark> predicate)
    {
        if (_bookmarks.Count == 0)
            return null;

        var prev = GetLastMark(predicate);

        foreach (var mark in _bookmarks)
        {
            if (predicate(mark) && mark.IsEnabled && mark.LineNumber < curLineNr)
            {
                if (mark.LineNumber > prev.LineNumber || prev.LineNumber >= curLineNr)
                {
                    prev = mark;
                }
            }
        }

        return prev;
    }

    #region ====Events====

    public event BookmarkEventHandler? Removed;
    public event BookmarkEventHandler? Added;

    private void OnRemoved(BookmarkEventArgs e) => Removed?.Invoke(this, e);

    private void OnAdded(BookmarkEventArgs e) => Added?.Invoke(this, e);

    #endregion
}