using System;

namespace PixUI;

[Flags]
public enum PointerButtons
{
    None = 0x00000000,
    Left = 0x00100000,
    Right = 0x00200000,
    Middle = 0x00400000,
    XButton1 = 0x00800000,
    XButton2 = 0x01000000
}

public sealed class PointerEvent : PropagateEvent
{
    private static readonly PointerEvent Default = new(PointerButtons.None, 0, 0, 0, 0);

    public PointerButtons Buttons { get; private set; }
    public float X { get; private set; }
    public float Y { get; private set; }

    public float DeltaX { get; private set; }
    public float DeltaY { get; private set; }

    public PointerEvent(PointerButtons buttons, float x, float y, float dx, float dy)
    {
        Buttons = buttons;
        X = x;
        Y = y;
        DeltaX = dx;
        DeltaY = dy;
    }

    public static PointerEvent UseDefault(PointerButtons buttons, float x, float y, float dx, float dy)
    {
        Default.Buttons = buttons;
        Default.X = x;
        Default.Y = y;
        Default.DeltaX = dx;
        Default.DeltaY = dy;
        Default.ResetHandled();
        return Default;
    }

    public void Reset(PointerButtons buttons, float x, float y, float dx, float dy)
    {
        Buttons = buttons;
        X = x;
        Y = y;
        DeltaX = dx;
        DeltaY = dy;
        IsHandled = false;
    }

    /// <summary>
    /// 仅用于向上传播事件时转换为Widget的本地坐标
    /// </summary>
    internal void SetPoint(float x, float y)
    {
        X = x;
        Y = y;
    }
}