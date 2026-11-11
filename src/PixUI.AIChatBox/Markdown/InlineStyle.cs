namespace PixUI.AIChatBox;

[Flags]
internal enum InlineStyle
{
    None,
    Bold = 1,
    Italic = 2,
    Strike = 4,
    Underline = 8,
    Link = 16,
    Code = 32,
}