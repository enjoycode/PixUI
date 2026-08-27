using System.Text;
using NUnit.Framework;
using PixUI.Dynamic.Design;
using PixUI.Dynamic.Json;

namespace PixUI.UnitTests.Dynamic;

public class SerializationTest
{
    [Test]
    public void LoadTest()
    {
        var json = """
                   {
                     "Root": {
                       "Type": "Center",
                       "Child": {
                         "Type": "Button",
                         "Text": {"Const": "Button1"},
                         "TextColor": {"Const": "FFFF0000"}
                       }
                     }
                   }
                   """;
        var controller = new DesignController();
        _ = new DesignCanvas(controller);
        var jsonSerializer = new DynamicJsonSerializer(controller);
        jsonSerializer.Load(Encoding.UTF8.GetBytes(json));
        Assert.IsTrue(controller.RootElement != null);
    }
}