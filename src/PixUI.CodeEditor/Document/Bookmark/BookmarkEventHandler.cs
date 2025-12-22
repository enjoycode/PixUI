using System;

namespace CodeEditor;

public delegate void BookmarkEventHandler(BookmarkEventArgs e);

/// <summary>
/// Description of BookmarkEventHandler.
/// </summary>
public readonly struct BookmarkEventArgs
{
    public Bookmark Bookmark { get; }

    public BookmarkEventArgs(Bookmark bookmark)
    {
        Bookmark = bookmark;
    }
}