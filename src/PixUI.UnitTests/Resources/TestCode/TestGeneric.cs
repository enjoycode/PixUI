using System;

//class NotSupported<T> where T: new() { }

interface IFactory { }

class Item: IDisposable, IFactory
{
    public void Dispose() {}
}

class GenericType<T1, T2> where T2: class, IDisposable, IFactory
{
    public void Add(T1 key, T2 value) {}

    public void GenericMethod<R>(R item) where R : IDisposable
    {
        item.Dispose();
    }
    
    public static void GenericStaticMethod<R>(R item) {}
    
    public void Method2(GenericType<string, T2> map) {}
}

class TestClass
{
    void Test()
    {
        GenericType<Item, Item>.GenericStaticMethod<string>("aa");
    }
}