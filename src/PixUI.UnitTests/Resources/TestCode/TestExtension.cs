using System.Linq;

public static class TestExtensions
{

    public static string ToExt(this string s, string arg)
    {
        return s + arg;
    }
    
}

public class TestCallee
{
    private string GetName() => "Hello";
    
    public void Test()
    {
        var s = "Hello";
        s = s.ToExt("World");           //to: TestExtensions.ToExt(s, "World")
        s = GetName().ToExt("World");   //to: TestExtensions.ToExt(GetName(), "World")
    }

    public object Test2()
    {
        var list = new string[] { "aa", "bb", "cc" };
        return list.Select(s => s == "bb").ToArray(); //这里不转换
    }
}