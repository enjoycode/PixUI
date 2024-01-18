using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PixUI.UnitTests.CS2TS;

public class LinqTest
{
    IEnumerable<int> GenNums()
    {
        int num = 0;
        while (true)
        {
            num++;
            if (num > 10) yield break;
            else yield return num;
        }
    }

    [Test]
    public void SumTest()
    {
        var list = GenNums();
        foreach (var item in list)
        {
            Console.WriteLine(item);
        }
        Assert.True(list.Sum() == 55);
    }

    [Test]
    public void FirstTest()
    {
        var list = GenNums();
        var n1 = list.First();
        var n2 = list.First();
        Assert.True(n1 == n2);
    }
}