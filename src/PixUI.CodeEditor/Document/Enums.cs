namespace CodeEditor
{
    public enum BracketMatchingStyle
    {
        Before,
        After
    }

    /// Describes the caret marker
    public enum LineViewerStyle
    {
        /// No line viewer will be displayed
        None,

        /// The row in which the caret is will be marked
        FullRow
    }

    /// Describes the indent style
    public enum IndentStyle
    {
        /// No indentation occurs
        None,

        /// The indentation from the line above will be
        /// taken to indent the curent line
        Auto,

        /// Inteligent, context sensitive indentation will occur
        Smart
    }

    /// Describes the bracket highlighting style
    public enum BracketHighlightingStyle
    {
        /// Brackets won't be highlighted
        None,

        /// Brackets will be highlighted if the caret is on the bracket
        OnBracket,

        /// Brackets will be highlighted if the caret is after the bracket
        AfterBracket
    }

    /// Describes the selection mode of the text area
    public enum DocumentSelectionMode
    {
        /// The 'normal' selection mode.
        Normal,

        /// Selections will be added to the current selection or new
        /// ones will be created (multi-select mode)
        Additive
    }
}