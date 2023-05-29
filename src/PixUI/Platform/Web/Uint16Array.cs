#if __WEB__
using System;
using PixUI;

[TSType("Uint16Array")]
public sealed class Uint16Array
{
    [TSTemplate("System.StringToUint16Array({1})")]
    public static Uint16Array FromString(string str) => throw new Exception();

    public Uint16Array(int length) { }

    public char this[int index]
    {
        get => '0';
        set { }
    }

    public int length => 0;

    public void set(Uint16Array source, int? targetOffset = null) { }

    public Uint16Array subarray(int begin, int? end = null) => throw new Exception();
}
#endif