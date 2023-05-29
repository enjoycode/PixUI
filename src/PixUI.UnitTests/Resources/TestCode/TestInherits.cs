using System;

public abstract class BaseClass
{
    public abstract string GetName();
}

public class Person : BaseClass, IDisposable
{
    public override string GetName() => "Rick";
    
    public void Dispose() {}
}

public class Alien : IDisposable
{
    public void Dispose() {}
}