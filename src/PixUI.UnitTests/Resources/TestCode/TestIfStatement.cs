class Person
{
    void IfTest1()
    {
        var temp = 0;
        if (temp > 0)
        {
            System.Console.Write("Test1");
        }
    }
    
    void IfTest2()
    {
        var temp = 0;
        if (temp > 0)
            System.Console.Write("True");
        else
            System.Console.Write("False");
    }
    
    void IfTest3()
    {
        var temp = 0;
        if (temp == 0)
        {
            temp++;
        }
        else if (temp > 1)
        {
            temp -= 1;
        }
        else
        {
            temp = 0;
        }
    }
}