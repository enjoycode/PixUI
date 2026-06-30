using NUnit.Framework;
using PixUI.Drawing.Skia;

namespace PixUI.UnitTests;

public class PathTest
{
    [SetUp]
    public void Setup()
    {
        Render.Init(new SkiaRender());
    }

    [Test]
    public void OpIntersectTest1()
    {
        var bigger = Rect.FromLTWH(10, 10, 200, 200);
        var smaller = Rect.FromLTWH(20, 20, 50, 50);
        var pathBigger = Path.Create();
        pathBigger.AddRect(bigger);
        var pathSmaller = Path.Create();
        pathSmaller.AddRect(smaller);

        var res = pathBigger.Op(pathSmaller, PathOp.Intersect);
        Assert.True(res);
        Assert.True(pathBigger.IsRect);
        var rect = pathBigger.GetRect();
        Assert.True(rect == smaller);
    }

    [Test]
    public void OpIntersectTest2()
    {
        var rect1 = Rect.FromLTWH(0, 0, 10, 10);
        var rect2 = Rect.FromLTWH(30, 30, 10, 10);
        var path1 = Path.Create();
        path1.AddRect(rect1);
        var path2 = Path.Create();
        path2.AddRect(rect2);

        var res = path1.Op(path2, PathOp.Intersect);
        Assert.True(res);
        Assert.True(path1.IsEmpty());
    }

    [Test]
    public void IsClosedTest()
    {
        var path1 = Path.Create();
        path1.MoveTo(1, 1);
        path1.LineTo(2, 2);
        Assert.False(path1.IsClosed());

        var path2 = Path.Create();
        path2.AddRect(Rect.FromLTWH(0, 0, 10, 10));
        Assert.True(path2.IsClosed());
    }

    [Test]
    public void OutlineContainsTest()
    {
        var path1 = Path.Create();
        path1.MoveTo(10, 10);
        path1.LineTo(20, 10);

        var path2 = path1.GetOutlinePath(4);
        Assert.IsTrue(path2 != null);
        Assert.IsTrue(path2!.Contains(11, 11));
        Assert.IsFalse(path2.Contains(21, 10));
        Assert.IsFalse(path2.Contains(11, 13));
    }
}