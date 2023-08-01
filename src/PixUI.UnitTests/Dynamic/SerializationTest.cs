using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUnit.Framework;
using PixUI.Dynamic;

namespace PixUI.UnitTests.Dynamic;

public class SerializationTest
{
    [Test]
    public void ValueSerializationTest()
    {
        var v1 = new ValueSource() { From = ValueFrom.Const, Value = "Hello World" };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(v1);
        var json = Encoding.UTF8.GetString(bytes);
        Console.WriteLine(json);
    }
}