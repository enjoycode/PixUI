using NUnit.Framework;
using PixUI.Dynamic;

namespace PixUI.UnitTests.Dynamic;

public class ContainerTest
{
    [Test]
    public void AddChildTest()
    {
        var parent = new Stack();
        var child = new Positioned();
        var slot = new ContainerSlot(nameof(Stack.Children), ContainerType.MultiChild);
        slot.AddChild(parent, child);
        Assert.True(ReferenceEquals(parent.Children[0], child));
    }

    [Test]
    public void SetChildTest()
    {
        var parent = new Center();
        var child = new Text("Hello");

        var slot = new ContainerSlot(nameof(Center.Child), ContainerType.SingleChild);
        slot.SetChild(parent, child);
        Assert.True(ReferenceEquals(parent.Child, child));
        
        slot.SetChild(parent, null);
        Assert.True(parent.Child == null);
    }
}