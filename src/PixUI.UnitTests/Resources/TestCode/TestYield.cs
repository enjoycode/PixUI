using System.Collections.Generic;

class TestClass
{
    private int c = 0;

    IEnumerable<int> YieldMethod(int count)
    {
        c++;
        if (c > count)
            yield break;
        else
            yield return c;
    }
}