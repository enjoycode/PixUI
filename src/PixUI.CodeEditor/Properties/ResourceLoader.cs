using System.IO;
using System.Reflection;

namespace PixUI.CodeEditor;

internal static class ResourceLoad
{
    private static readonly Assembly ResAssembly = typeof(ResourceLoad).Assembly;

    public static Stream LoadStream(string res)
    {
        return ResAssembly.GetManifestResourceStream("PixUI.CodeEditor." + res)!;
    }

    public static string LoadString(string res)
    {
        using var stream = LoadStream(res);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public static byte[] LoadBytes(string res)
    {
        using var stream = LoadStream(res);
        var data = new byte[stream.Length];
        _ = stream.Read(data, 0, data.Length);
        return data;
    }
}