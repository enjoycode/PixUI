using System;

namespace PixUI;

public readonly struct FocusChangedEvent
{
    public FocusChangedEvent(bool isFocused, Widget? oldFocusedWidget, Widget? newFocusedWidget, UIWindow window)
    {
        IsFocused = isFocused;
        OldFocusedWidget = oldFocusedWidget;
        NewFocusedWidget = newFocusedWidget;
        Window = window;
    }

    public readonly bool IsFocused;
    public readonly Widget? OldFocusedWidget;
    public readonly Widget? NewFocusedWidget;
    public readonly UIWindow Window;
}

public sealed class FocusNode
{
    public event Action<KeyEvent>? KeyDown;
    public event Action<KeyEvent>? KeyUp;
    public event Action<FocusChangedEvent>? FocusChanged;
    public event Action<string>? TextInput;

    internal void RaiseKeyDown(KeyEvent theEvent) => KeyDown?.Invoke(theEvent);

    internal void RaiseKeyUp(KeyEvent theEvent) => KeyUp?.Invoke(theEvent);

    internal void RaiseFocusChanged(FocusChangedEvent theEvent) => FocusChanged?.Invoke(theEvent);

    internal void RaiseTextInput(string text) => TextInput?.Invoke(text);
}