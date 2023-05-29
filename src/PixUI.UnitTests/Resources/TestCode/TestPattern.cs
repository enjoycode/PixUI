using System;

class Dog
{
    public void Wang() {}
}

class Cat
{
    public void Meow() {}
}

class Test<T>
{
    void IfPattern()
    {
        object obj = new Cat();
        if (obj is Dog dog)
            dog.Wang();
        else if (obj is Cat cat)
        {
            cat.Meow();
        }
        else
        {
            System.Console.Write("What");
        }
    }

    void SwitchPattern()
    {
        object obj = new Cat();
        switch (obj)
        {
            case Dog dog:
            {
                dog.Wang();
                break;
            }
            case Cat cat:
                cat.Meow();
                break;
            case Test<T> test:
                break;
            // case 1:
            // case 2:
            // {
            //     Console.Write("hello");
            // }
            //     break;
            default:
                break;
        }
    }
}