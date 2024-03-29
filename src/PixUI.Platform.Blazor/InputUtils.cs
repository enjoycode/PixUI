namespace PixUI.Platform.Blazor;

public static class InputUtils
{
    /// <summary>
    /// 转换mousedown & mouseup时的button属性值
    /// </summary>
    internal static PointerButtons ConvertButton(int button)
    {
        return button switch
        {
            0 => PointerButtons.Left,
            1 => PointerButtons.Middle,
            2 => PointerButtons.Right,
            _ => PointerButtons.None
        };
    }

    /// <summary>
    /// 转换mousemove时的buttons属性值
    /// </summary>
    internal static PointerButtons ConvertButtons(int buttons)
    {
        //TODO: | 合并
        return buttons switch
        {
            1 => PointerButtons.Left,
            2 => PointerButtons.Right,
            3 => PointerButtons.Middle,
            _ => PointerButtons.None
        };
    }

    internal static Keys ConvertKeys(string key, string code, bool alt, bool control, bool shift, bool meta)
    {
        // Console.WriteLine($"ConvertKeys: key={key} code={code} alt={alt} ctrl={control}, shift={shift}");
        var keys = Keys.None;
        if (alt) keys |= Keys.Alt;
        if (control) keys |= Keys.Control;
        if (shift) keys |= Keys.Shift;
        if (meta) keys |= Keys.Meta;

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