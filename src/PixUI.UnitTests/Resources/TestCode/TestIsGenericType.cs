using PixUI;

namespace PixUI
{
    public class GenericType<T>{}
}

class TestClass<T>
{
    void Test()
    {
        object obj = new GenericType<T>();
        if (obj is null) {}
        if (obj is not null) {}
        // if (obj is GenericType<string>) {} //不支持
        if (obj is GenericType<T> genericType){}
        if (obj is GenericType<T>) {}
        if (obj is PixUI.GenericType<T>) {}
    }
}