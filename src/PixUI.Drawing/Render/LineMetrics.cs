using System.Runtime.InteropServices;

namespace PixUI;

[StructLayout(LayoutKind.Sequential)]
public struct LineMetrics
{
    /// <summary>
    /// The indexes in the text buffer the line begins.
    /// </summary>
    public ulong StartIndex;

    /// <summary>
    /// The indexes in the text buffer the line ends.
    /// </summary>
    public ulong EndIndex;

    /// <summary>
    /// The height of the current line is `Round(Ascent + Descent)`.
    /// </summary>
    public double Height;

    /// <summary>
    /// Width of the line.
    /// </summary>
    public double Width;

    /// <summary>
    /// The left edge of the line. The right edge can be obtained with `Left + Width`
    /// </summary>
    public double Left;
}