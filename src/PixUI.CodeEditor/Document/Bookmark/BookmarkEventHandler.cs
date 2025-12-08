using System;

namespace CodeEditor;

public delegate void BookmarkEventHandler(object sender, BookmarkEventArgs e);

/// <summary>
/// Description of BookmarkEventHandler.
/// </summary>
public sealed class BookmarkEventArgs : EventArgs
{
    public Bookmark Bookmark { get; }

    public BookmarkEventArgs(Bookmark bookmark)
    {
        Bookmark = bookmark;
    }
}