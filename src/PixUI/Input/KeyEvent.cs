namespace PixUI;

public sealed class KeyEvent : PropagateEvent
{
    private static readonly KeyEvent Default = new KeyEvent(Keys.None);

    public static KeyEvent UseDefault(Keys keysData)
    {
        Default.KeyData = keysData;
        return Default;
    }

    public Keys KeyData { get; private set; }

    public Keys KeyCode => KeyData & Keys.KeyCode;

    public bool Control => (KeyData & Keys.Control) == Keys.Control;

    public bool Shift => (KeyData & Keys.Shift) == Keys.Shift;

    public bool Alt => (KeyData & Keys.Alt) == Keys.Alt;

    public KeyEvent(Keys keyData)
    {
        KeyData = keyData;
    }
}