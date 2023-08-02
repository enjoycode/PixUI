using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUnit.Framework;
using PixUI.Dynamic;
using PixUI.Dynamic.Design;

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

    [Test]
    public void LoadTest()
    {
        var json = """
{
  "View": {
    "Type": "Center",
    "Child": {
      "Type": "Button",
      "CtorArgs": [ { "From": 0, "Value": "Button1" }, { "From": 0, "Value": null } ],
      "Properties": { "TextColor": { "From": 0, "Value": "00FF0000" } }
    }
  }
}
""";
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var controller = new DesignController();
        controller.Load(jsonBytes);
    }
}