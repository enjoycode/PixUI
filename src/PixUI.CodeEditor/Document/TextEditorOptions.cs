namespace CodeEditor;

public sealed class TextEditorOptions
{
    public int TabIndent = 4;
    public int IndentationSize = 4;
    public IndentStyle IndentStyle = IndentStyle.Smart;
    public DocumentSelectionMode DocumentSelectionMode = DocumentSelectionMode.Normal;
    public BracketMatchingStyle BracketMatchingStyle = BracketMatchingStyle.After;
    public bool AllowCaretBeyondEOL = false;
    public bool CaretLine = false;
    public bool ShowMatchingBracket = true;
    public bool ShowLineNumbers = true;
    public bool ShowSpaces = false;
    public bool ShowTabs = false;
    public bool ShowEOLMarker = false;
    public bool ShowInvalidLines = false;
    public bool IsIconBarVisible = false;
    public bool EnableFolding = true;
    public bool ShowHorizontalRuler = false;
    public bool ShowVerticalRuler = false;
    public bool ConvertTabsToSpaces = false;
    public bool MouseWheelScrollDown = true;
    public bool MouseWheelTextZoom = true;
    public bool HideMouseCursor = false;
    public bool CutCopyWholeLine = true;
    public int VerticalRulerRow = 80;
    public LineViewerStyle LineViewerStyle = LineViewerStyle.None;
    public string LineTerminator = "\n";
    public bool AutoInsertCurlyBracket = true;
    public bool SupportReadOnlySegments = false;
}