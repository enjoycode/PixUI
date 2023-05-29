using System;

class TestDelegate
{

    public Action<string>? NameChanged;

    public void PassDelegate(Action<string> callback)
    {
        NameChanged = callback;
    }

}

class TestClass
{
    void Test()
    {
        var d = new TestDelegate();
        d.PassDelegate(d == null ? s => Console.Write(s) : OnNameChanged);
        d.PassDelegate(StaticNameChanged);
        d.PassDelegate(this.OnNameChanged);
        d.PassDelegate(s => Console.Write(s)); //lambda不处理
        
        d.NameChanged = OnNameChanged;
        d.NameChanged = (s) => Console.Write(s); //lambda不处理
        d.NameChanged = d == null ? s => Console.Write(s) : OnNameChanged;

        OnNameChanged("aa");

        //以下不支持
        //d.NameChanged = new TestClass().OnNameChanged;
    }

    private void OnNameChanged(string name) { }
    
    private static void StaticNameChanged(string name) {}
}