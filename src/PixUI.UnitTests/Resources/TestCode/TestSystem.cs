using System;
using System.Collections.Generic;

enum Level
{
    Level1, Level2
}

class TestString
{
    void Test()
    {
        var ch = '\'';
        char ch1 = (char)0x66;
        var str1 = new string('A', 3);
        var str2 = new string(ch, 3);
        var ch2 = str1[1];
        var len = str1.Length;
        var empty = string.Empty;
        var starts = empty.StartsWith("some");
        var array1 = str1.Split('/');
        var array2 = str1.Split(ch);

        EqualityComparer<int>.Default.Equals(1, 2); //will to System.Equals(1, 2)
    }
}

class TestNumber
{
    void Test()
    {
        float a = 10f;
        int num = (int)a;
        int num2 = (int)Level.Level1;
        var res = num.CompareTo(11);
    }
}

class TestWeakReference
{
    private WeakReference _target = null!;

    object? GetObject()
    {
        _target = new WeakReference(_target);
        return _target.Target;
    }
}

class TestDateTime
{
    void Subtract()
    {
        var date1 = DateTime.UtcNow;
        var date2 = DateTime.UtcNow;
        var span1 = date2 - date1;
        var span2 = date2.Subtract(date1);
        Console.Write(span1.TotalSeconds + span2.TotalSeconds);
    }
}

class InvokeDelegate
{
    private Action<int> _action;

    void Test(Action action)
    {
        action();
        action.Invoke();
    }
}

class TestTryCatch
{
    void TryDoSomething()
    {
        try { /*do something with exception*/ }
        // catch (NotSupportedException ex) {} //不支持多个Catch Statement
        catch (Exception e) { throw; }
    }
}