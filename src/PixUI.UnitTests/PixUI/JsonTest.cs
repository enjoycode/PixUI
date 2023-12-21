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
}