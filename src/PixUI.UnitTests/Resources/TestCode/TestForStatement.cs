class Person
{
    public void SayHello()
    {
        var array = new int[] { 1, 2, 3 };
        System.Console.Write(array.Length);
        foreach (var item in array)
        {
            System.Console.WriteLine(item);
        }
        
        for (var i = 0; i < array.Length; i++)
        {
            System.Console.WriteLine(array[i]);
        }
    }
}