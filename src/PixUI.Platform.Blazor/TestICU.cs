using System.Globalization;
using System.Runtime.InteropServices;

namespace PixUI.Platform.Blazor;

public static unsafe class TestICU
{
    [DllImport("skia", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr ubrk_open(int brkType, byte* local, void* text, int textLen, int* status);

    [DllImport("skia", CallingConvention = CallingConvention.Cdecl)]
    private static extern byte* uloc_getDefault();

    [DllImport("skia", CallingConvention = CallingConvention.Cdecl)]
    private static extern byte* u_errorName(int status);

    [DllImport("skia", CallingConvention = CallingConvention.Cdecl)]
    private static extern void udata_setFileAccess(int type, int* status);

    [DllImport("libSystem.Globalization.Native", EntryPoint = "GlobalizationNative_GetICUVersion")]
    private static extern int GetICUVersion();

    [DllImport("libSystem.Globalization.Native")]
    private static extern int GlobalizationNative_LoadICU();

    public static unsafe void Test()
    {
        Console.WriteLine($"ICU Version = {GetICUVersion() >> 24}");
        //Console.WriteLine($"LoadICU = {GlobalizationNative_LoadICU()}");

        int status;
        // udata_setFileAccess(2, &status);
        // Console.WriteLine(GetErrorName(status));

        Console.WriteLine("当前文化:" + CultureInfo.CurrentCulture.Name);
        var local = uloc_getDefault(); //local = "en_US"
        var localName = string.Empty;
        var index = 0;
        while (local[index] != 0)
        {
            localName += (char)local[index];
            index++;
        }
        
        Console.WriteLine(localName);
        status = 0;
        var breakIterator = ubrk_open(2, local, null, 0, &status);
        Console.WriteLine($"{GetErrorName(status)} and Handle = {breakIterator}");
    }

    private static string GetErrorName(int status)
    {
        var ptr = u_errorName(status);
        var name = string.Empty;
        var index = 0;
        while (ptr[index] != 0)
        {
            name += (char)ptr[index];
            index++;
        }

        return name;
    }
}