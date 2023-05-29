using System;
using System.Collections.Generic;

class TestDictionary
{
    void Test1()
    {
        var dic = new Dictionary<int, int>();
        dic[1] = 2;
        var v = dic[1];
        if (dic.TryGetValue(1, out var value))
            Console.WriteLine(value);
    }

    void Test2()
    {
        var dic = new Dictionary<int, int>()
        {
            {1, 2},
            {2, 3},
        };
    }
}