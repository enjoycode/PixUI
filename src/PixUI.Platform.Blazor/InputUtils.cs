namespace PixUI.Platform.Blazor;

public static class InputUtils
{
    internal static PointerButtons ConvertButtons(int buttons)
    {
        return buttons switch
        {
            1 => PointerButtons.Left,
            2 => PointerButtons.Right,
            3 => PointerButtons.Middle,
            _ => PointerButtons.None
        };
    }

    internal static Keys ConvertKeys(string key, string code, bool alt, bool control, bool shift)
    {
        var keys = Keys.None;
        if (alt) keys |= Keys.Alt;
        if (control) keys |= Keys.Control;
        if (shift) keys |= Keys.Shift;

        //TODO: numbers
        if (key.Length == 1)
        {
            var keyValue = key[0];
            if (keyValue >= 0x41 && keyValue <= 0x5A) //A-Z
                return keys | (Keys)keyValue;

            if (keyValue >= 0x61 && keyValue <= 0x7A) //a-z
                return keys | (Keys)(keyValue - 32);
        }

        //TODO: others and use map
        return code switch
        {
            "Backspace" => keys | Keys.Back,
            "Tab" => keys | Keys.Tab,
            "Enter" => keys | Keys.Return,
            "ArrowLeft" => keys | Keys.Left,
            "ArrowRight" => keys | Keys.Right,
            "ArrowUp" => keys | Keys.Up,
            "ArrowDown" => keys | Keys.Down,
            _ => keys
        };
    }
}