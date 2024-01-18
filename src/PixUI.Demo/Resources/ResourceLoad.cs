using System.IO;
using System.Reflection;

namespace PixUI.Demo;

internal static class ResourceLoad
{
    private static readonly Assembly ResAssembly = typeof(ResourceLoad).Assembly;

    public static Stream LoadStream(string res)
    {
        return ResAssembly.GetManifestResourceStream("PixUI.Demo." + res)!;
    }

    public static byte[] LoadBytes(string res)
    {
        using var stream = LoadStream(res);
        var data = new byte[stream.Length];
        stream.Read(data, 0, data.Length);
        return data;
    }
}