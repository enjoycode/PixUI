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
    public void LoadTest()
    {
        var json = """
                   {
                     "View": {
                       "Type": "Center",
                       "Child": {
                         "Type": "Button",
                         "CtorArgs": [ { "Const": "Button1" }, { "Const": null } ],
                         "Properties": { "TextColor": { "Const": "FFFF0000" } }
                       }
                     }
                   }
                   """;
        var controller = new DesignController();
        controller.Load(Encoding.UTF8.GetBytes(json));
    }
}