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
        using var pathBiggerBuilder = PathBuilder.Create();
        pathBiggerBuilder.AddRect(bigger);
        using var pathBigger = pathBiggerBuilder.Detach();
        using var pathSmallerBuilder = PathBuilder.Create();
        pathSmallerBuilder.AddRect(smaller);
        using var pathSmaller = pathSmallerBuilder.Detach();

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
        using var path1Builder = PathBuilder.Create();
        path1Builder.AddRect(rect1);
        using var path1 = path1Builder.Detach();
        using var path2Builder = PathBuilder.Create();
        path2Builder.AddRect(rect2);
        var path2 = path2Builder.Detach();

        var res = path1.Op(path2, PathOp.Intersect);
        Assert.True(res);
        Assert.True(path1.IsEmpty());
    }

    [Test]
    public void IsClosedTest()
    {
        using var path1Builder = PathBuilder.Create();
        path1Builder.MoveTo(1, 1);
        path1Builder.LineTo(2, 2);
        using var path1 = path1Builder.Detach();
        Assert.False(path1.IsClosed());

        using var path2Builder = PathBuilder.Create();
        path2Builder.AddRect(Rect.FromLTWH(0, 0, 10, 10));
        using var path2 = path2Builder.Detach();
        Assert.True(path2.IsClosed());
    }

    [Test]
    public void OutlineContainsTest()
    {
        using var path1Builder = PathBuilder.Create();
        path1Builder.MoveTo(10, 10);
        path1Builder.LineTo(20, 10);
        var path1 = path1Builder.Detach();

        var path2 = path1.GetOutlinePath(4);
        Assert.IsTrue(path2 != null);
        Assert.IsTrue(path2!.Contains(11, 11));
        Assert.IsFalse(path2.Contains(21, 10));
        Assert.IsFalse(path2.Contains(11, 13));
    }
}