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

    /// <summary>
    /// Removes the "perspective" component from `transform`.
    /// </summary>
    /// <remarks>
    /// When applying the resulting transform matrix to a point with a
    /// z-coordinate of zero (which is generally assumed for all points
    /// represented by an [Offset]), the other coordinates will get transformed as
    /// before, but the new z-coordinate is going to be zero again. This is
    /// achieved by setting the third column and third row of the matrix to "0, 0, 1, 0".
    /// </remarks>
    public static Matrix4 RemovePerspectiveTransform(Matrix4 transform)
    {
        var vector = new Vector4(0, 0, 1, 0);
        transform.SetColumn(2, vector);
        transform.SetRow(2, vector);
        return transform;
    }
}