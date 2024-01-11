namespace CodeEditor;

internal static class TextUtils
{
    private const char ZwjUtf16 = (char)0x200D;

    /// <summary>
    /// Returns true iff the given value is a valid UTF-16 surrogate. The value
    /// must be a UTF-16 code unit, meaning it must be in the range 0x0000-0xFFFF.
    /// </summary>
    private static bool IsUtf16Surrogate(char value)
    {
        return (value & 0xF800) == 0xD800;
    }

    /// <summary>
    /// Checks if the glyph is either [Unicode.RLM] or [Unicode.LRM]. These values take
    /// up zero space and do not have valid bounding boxes around them.
    /// </summary>
    private static bool IsUnicodeDirectionality(char value)
    {
        return value == 0x200F || value == 0x200E;
    }

    /// <summary>
    /// Check for multi-code-unit glyphs such as emojis or zero width joiner
    /// </summary>
    public static bool IsMultiCodeUnit(char codeUnit)
    {
        return IsUtf16Surrogate(codeUnit) ||
               codeUnit == ZwjUtf16 ||
               IsUnicodeDirectionality(codeUnit);
    }
}