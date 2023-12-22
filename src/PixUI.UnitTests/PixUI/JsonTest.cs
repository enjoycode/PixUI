using System.Text.Json;
using NUnit.Framework;

namespace PixUI.UnitTests;

public class JsonTest
{
    [Test]
    public void IconDataTest()
    {
        var icon = MaterialIcons.Search;
        var bytes = JsonSerializer.SerializeToUtf8Bytes(icon);
        var res = JsonSerializer.Deserialize<IconData>(bytes);
        Assert.True(icon.CodePoint == res.CodePoint);
        Assert.True(icon.Asset.AssemblyName == res.Asset.AssemblyName);
    }

    [Test]
    public void EdgeInsetsTest()
    {
        var edge1 = EdgeInsets.All(10);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(edge1);
        var edge2 = JsonSerializer.Deserialize<EdgeInsets>(bytes);
        Assert.True(edge1 == edge2);

        var edge3 = EdgeInsets.Only(1, 2, 3, 4);
        bytes = JsonSerializer.SerializeToUtf8Bytes(edge3);
        var edge4 = JsonSerializer.Deserialize<EdgeInsets>(bytes);
        Assert.True(edge3 == edge4);
    }

    [Test]
    public void InputBorderTest()
    {
        var b1 = new OutlineInputBorder(new BorderSide(Colors.Red, 2f), BorderRadius.Circular(4f));
        var json = JsonSerializer.Serialize(b1, typeof(InputBorder));
        var b2 = (OutlineInputBorder)JsonSerializer.Deserialize<InputBorder>(json)!;
        Assert.True(b2.BorderRadius == b1.BorderRadius);
        Assert.True(b2.BorderSide == b1.BorderSide);

        var b3 = new UnderlineInputBorder(new BorderSide(Colors.Red, 2f));
        json = JsonSerializer.Serialize(b3, typeof(InputBorder));
        var b4 = (UnderlineInputBorder)JsonSerializer.Deserialize<InputBorder>(json)!;
        Assert.True(b4.BorderSide == b3.BorderSide);
    }

    [Test]
    public void OtherTest()
    {
        object? obj = null;
        var json = JsonSerializer.Serialize(obj, typeof(int?));
    }
}