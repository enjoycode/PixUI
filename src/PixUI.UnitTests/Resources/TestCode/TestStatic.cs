static class Consts
{
    public static readonly string Name = "Rick";

    public static void SayHello()
    {
        System.Console.WriteLine(Name);
        System.Console.WriteLine(Consts.Name);
    }
}

public class Person
{

    public void Test()
    {
        Consts.SayHello();
    }
    
}