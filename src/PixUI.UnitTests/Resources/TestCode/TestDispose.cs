using System;

class Resource : IDisposable
{
    public void Dispose() {}

    public Resource(char type = 'A')
    {
        using var res1 = new Resource();
        if (type == 'A')
            return; //will insert res1.Dispose() before return
        
        if (type == 'B')
        {
            return; //will insert res1.Dispose() before return
        }

        Func<bool> func = () =>
        {
            using var res3 = new Resource();
            return true; //will insert res3.Dispose() before return
        };

        Console.Write(type); // will add res1.Dispose() after
    }

    public static void Test(bool condition)
    {
        using var res1 = new Resource();
        if (condition)
        {
            using var res2 = new Resource();
            Console.Write(res2); // will add res2.Dispose() after
        }
        Console.Write(res1); // will add res1.Dispose() after
    }
}