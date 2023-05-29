using System.Collections.Generic;

class Person
{
    public string Name { get; set; } = "";
    public int Age { get; set; }
    
    public Person() {}

    public void Test()
    {
        var obj = new object();
        var p1 = new Person() { Name = "Rick", Age = 100 };
        var p2 = new Person()
        {
            Name = "Eric",
            Age = 100
        };
        var list = new List<int>() { 1, 2, 3 };
        Person[] array1 = new[] { new Person(), new Person() };
        Person[] array2 = { new Person() };
        //Dictionary<int, int> dic = new() { { 1, 3 }, { 2, 4 } }; //TODO:暂不支持
    }
}