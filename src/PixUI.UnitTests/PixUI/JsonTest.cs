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
}