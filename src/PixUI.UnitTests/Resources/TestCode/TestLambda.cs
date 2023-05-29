using System;

class Person
{
    public void Call()
    {
        Func<int, string> func1 = n => n.ToString();
        Func<int, string> func2 = (n) => n.ToString();
        Func<int, string> func3 = (int n) => n.ToString();
        Func<int, string> func4 = (n) =>
        {
            return n.ToString();
        };
        
        func1(180);
    }
}