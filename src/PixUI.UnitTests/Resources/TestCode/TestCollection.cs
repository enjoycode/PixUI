using System.Collections.Generic;

class TestClass
{
    void Test()
    {
        var list1 = new List<int>();
        IList<int> list2 = list1;
        var count1 = list1.Count; //list1.length
        var count2 = list2.Count; //list2.length
    }

    void TestArray()
    {
        var a1 = new double[] { 1, 2, 3 };
        int[] a2 = new[] { 1, 2, 3 };
        var a3 = new[] { 1, 2, 3 };
    }
}