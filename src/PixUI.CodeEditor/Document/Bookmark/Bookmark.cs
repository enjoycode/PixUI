using System;
using PixUI;

namespace CodeEditor;

public class Bookmark
{
    public Bookmark(Document document, TextLocation location, bool isEnabled = true)
    {
        _document = document;
        _isEnabled = isEnabled;
        Location = location;
    }

    private Document _document;
    private TextAnchor? _anchor;
    private TextLocation _location;
    private bool _isEnabled;
    private bool _isHighlighted;

    public Document Document
    {
        get => _document;
        set
        {
            if (_document != value)
            {
                if (_anchor != null)
                {
                    _location = _anchor.Location;
                    _anchor = null;
                }

                _document = value;
                CreateAnchor();
                //OnDocumentChanged(EventArgs.Empty);
            }
        }
    }

    /// <summary>
    /// Gets the TextAnchor used for this bookmark.
    /// Is null if the bookmark is not connected to a document.
    /// </summary>
    public TextAnchor? Anchor => _anchor;

    public TextLocation Location
    {
        get => _anchor?.Location ?? _location;
        set
        {
            _location = value;
            CreateAnchor();
        }
    }

    public int LineNumber => _anchor?.LineNumber ?? _location.Line;

    public int ColumnNumber => _anchor?.ColumnNumber ?? _location.Column;

    /// <summary>
    /// Gets if the bookmark can be toggled off using the 'set/unset bookmark' command.
    /// </summary>
    public virtual bool CanToggle => true;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                Document.BookmarkManager.OnIsEnabledChanged(new BookmarkEventArgs(this));
            }
        }
    }

    public bool IsHighlighted
    {
        get => _isHighlighted;
        set
        {
            if (_isHighlighted != value)
            {
                _isHighlighted = value;
                Document.BookmarkManager.OnIsHighlightedChanged(new BookmarkEventArgs(this));
            }
        }
    }

    private void CreateAnchor()
    {
        if (_document != null!)
        {
            var line = _document.GetLineSegment(Math.Max(0,
                Math.Min(_location.Line, _document.TotalNumberOfLines - 1)));
            _anchor = line.CreateAnchor(Math.Max(0, Math.Min(_location.Column, line.Length)));
            // after insertion: keep bookmarks after the initial whitespace (see DefaultFormattingStrategy.SmartReplaceLine)
            _anchor.MovementType = AnchorMovementType.AfterInsertion;
            _anchor.Deleted += AnchorDeleted;
        }
    }

    private void AnchorDeleted()
    {
        _document.BookmarkManager.RemoveMark(this);
    }

    #region ====Events====

    // public event EventHandler? DocumentChanged;
    // protected virtual void OnDocumentChanged(EventArgs e) => DocumentChanged?.Invoke(this, e);

    #endregion

    public virtual bool Click(TextEditor textEditor, Point position, PointerButtons buttons)
    {
        if (buttons == PointerButtons.Left && CanToggle)
        {
            _document.BookmarkManager.RemoveMark(this);
            return true;
        }

        return false;
    }

    public virtual void Draw(Canvas canvas, Rect bounds)
    {
        var paint = Paint.Shared(Colors.Red);
        paint.AntiAlias = true;
        canvas.DrawCircle(9.5f, bounds.Y + bounds.Height / 2 + 1, 6, paint);
    }
}