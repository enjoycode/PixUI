using System.Runtime.CompilerServices;

abstract class TestMethodArgs
{
    TestMethodArgs(int? num = null) {}
    
    public void TestDefaultValue(int num = 10) {}

    public abstract void AbstractWithDefaultValue(int? num = null);
    
    public void ParamsArgs1(params int[] args) { }
    
    public void ParamsArgs2(params string[] args) {}
    
    public void ParamsArgs3(params int[]? args) {}

    public void TestParamsArgs()
    {
        var args1 = new string[] { "a", "b" };
        ParamsArgs2(args1); //ParamsArgs2(...args1);

        var args2 = new int[] { 1, 2 };
        ParamsArgs1(args2); //ParamsArgs1(...args2.ToArray())

        ParamsArgs3(1, 2); //不转换
        ParamsArgs3(null); //转换时移除null参数
    }
    
    public void RefArg1(ref int arg)
    {
        RefArg2(ref arg); //这里不用转换
        arg++;
    }

    public void RefArg2(ref int arg)
    {
        arg = 32;
    }

    public void TestRefArg()
    {
        int v = 3;
        RefArg1(ref v);
    }

    public bool OutArg(out int arg)
    {
        arg = 32;
        return true;
    }
    
    public void TestOutArg()
    {
        if (OutArg(out var value))
            System.Console.WriteLine(value);
    }
    
    public void WithCallerMemberNameParameter(int v = 0, [CallerMemberName] string? name = null) {}

    public void TestCallerMemberNameParameter()
    {
        WithCallerMemberNameParameter(); //WithCallerMemberNameParameter(undefined, "TestCallerMemberNameParameter");
    }
}