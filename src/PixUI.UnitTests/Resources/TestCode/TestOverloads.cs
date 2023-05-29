using PixUI;

public class Function
{
    // private string? _name;
    // public Function() {}
    // [TSRename("MakeByName")]
    // public Function(string name) { _name = name; }
    
    public void SayHello() {}
    [TSRename("SayHelloByName")]
    public void SayHello(string name) {}
}

[TSRename("Function1")]
public class Function<T1> {}

[TSRename("Function2")]
public class Function<T1, T2> {}

public class TestClass
{

    public void Test()
    {
        var func = new Function();
        Function<string> func1 = new();
        var func2 = new Function<string, int>();

        func.SayHello();
        func.SayHello("Rick");
    }
    
}

//以下不注释报错，类型名称重复
// namespace Demo
// {
//     public class Function { }
// }