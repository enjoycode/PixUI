using System;
using System.Dynamic;
using System.Linq.Expressions;
using NUnit.Framework;

namespace PixUI.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        float a = 1;
        float b = float.PositiveInfinity;
        Assert.True(a < b);
        Assert.True(float.IsPositiveInfinity(b - 1));

        float n = float.NaN;
        n += 1.0f;
        Assert.True(float.IsNaN(n));
            
        Assert.Pass();
    }

    [Test]
    public void DynamicTest()
    {
        dynamic obj = new Person();
        obj.Name = "Rick";
        //obj["Name"] = "Rick";
        obj.SayHello();
    }
}

class Person //: IDynamicMetaObjectProvider
{
    public string Name { get; set; }

    public void SayHello() => Console.WriteLine($"Hello {Name}");
    public DynamicMetaObject GetMetaObject(Expression parameter)
    {
        throw new NotImplementedException();
    }
}