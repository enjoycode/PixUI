public class Person
{
    public string Name {get; set;} = "Rick";

    public int Age = 180, Level = 100;

    public void SayHello()
    {
        System.Console.WriteLine($"Hello {Name} {GetAge()}");
    }

    public int GetAge() => (Age + 2) * 3 ;
    
    //public static implicit operator EdgeInsets(float value) => new(value, value, value, value);

}