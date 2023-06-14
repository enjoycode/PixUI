using System;
using System.Runtime.CompilerServices;
using IOPath = System.IO.Path;

namespace PixUI;

public static class Log
{
    [System.Diagnostics.Conditional("DEBUG")]
    public static void Debug(string msg, [CallerFilePath] string file = "", [CallerMemberName] string method = "",
        [CallerLineNumber] int line = 0)
    {
        Write('D', msg, file, method, line);
    }

    public static void Info(string msg, [CallerFilePath] string file = "", [CallerMemberName] string method = "",
        [CallerLineNumber] int line = 0)
    {
        Write('I', msg, file, method, line);
    }

    public static void Warn(string msg, [CallerFilePath] string file = "", [CallerMemberName] string method = "",
        [CallerLineNumber] int line = 0)
    {
        Write('W', msg, file, method, line);
    }

    public static void Error(string msg, [CallerFilePath] string file = "", [CallerMemberName] string method = "",
        [CallerLineNumber] int line = 0)
    {
        Write('E', msg, file, method, line);
    }

    private static void Write(char level, string msg, string file, string method, int line)
    {
        Console.WriteLine($"[{level}{DateTime.Now:hh:mm:ss} {IOPath.GetFileNameWithoutExtension(file)}.{method}:{line}]: {msg}");
    }
}