using System;
using PixUI;

public class ObjectList
{
    
    public string this[int index]
    {
        get => "Hello";
        [TSIndexerSetToMethod]
        set
        {
            if (value == "Hello")
                Console.WriteLine("Hello");
            else
                Console.WriteLine("Done");
        }
    }

    public void Test()
    {
        var obj = new ObjectList();
        obj[3] = "Hello";
    }
    
}