using System;
using System.Linq;

class TestClass
{
    void TestSelect()
    {
        var ar = new[] { 1, 2, 3 };
        var list = ar.Select(i =>
        {
            var name = "Rick";
            return new
            {
                name,
                index = i
            };
        })
            .OrderBy(t => { return t.index; });
        Console.WriteLine(list.First().name);
    }

    void TestOrderBy()
    {
        var ar = new[] { 1, 2, 3 };
        var res = ar.OrderBy(t => t);
    }
}