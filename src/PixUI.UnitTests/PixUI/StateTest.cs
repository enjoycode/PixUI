using System;
using NUnit.Framework;

namespace PixUI.UnitTests;

public class StateTest
{
    [Test]
    public void MakeEmptyTest()
    {
        State<int?> s = State<int?>.Default();
        Assert.True(!s.Value.HasValue);

        State<object?> s2 = State<int>.Default();
        Console.WriteLine(s2.Value);
    }
}