namespace PixUI.Drawing.Skia;

internal static class HashCodeExtensions
{
    public static unsafe void Add(this ref HashCode hashCode, void* value)
    {
        hashCode.Add(value == null ? 0 : ((IntPtr)value).GetHashCode());
    }
}