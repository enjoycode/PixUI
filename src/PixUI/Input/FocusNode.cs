using System;

namespace PixUI;

public sealed class FocusNode
{
    public event Action<KeyEvent>? KeyDown;
    public event Action<KeyEvent>? KeyUp;
    public event Action<bool>? FocusChanged;
    public event Action<string>? TextInput;

    internal void RaiseKeyDown(KeyEvent theEvent) => KeyDown?.Invoke(theEvent);

    internal void RaiseKeyUp(KeyEvent theEvent) => KeyUp?.Invoke(theEvent);

    internal void RaiseFocusChanged(bool focused) => FocusChanged?.Invoke(focused);

    internal void RaiseTextInput(string text) => TextInput?.Invoke(text);
}