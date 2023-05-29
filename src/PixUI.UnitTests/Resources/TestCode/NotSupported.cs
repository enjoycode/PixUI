using System;

class NotSupportedClass
{
    void Test(string? name)
    {
        _ = name ?? throw new Exception();
    }
}