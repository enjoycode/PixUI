using System;
using NUnit.Framework;

namespace PixUI.UnitTests;

public class FontTest
{
    private const string fontFamilyName = "PingFang SC";

    private static ITypeface GetTypeface(string familyName, bool bold, bool italic) =>
        FontCollection.FindTypeface(familyName, bold, italic)!;

    private static IFont GetFont(string familyName, bool bold, bool italic, int sizeInPoints) =>
        GetTypeface(familyName, bold, italic).MakeFont(sizeInPoints);

    [Test]
    public void FindTypefaceTest()
    {
        var typeface = GetTypeface(fontFamilyName, true, false);
        Assert.NotNull(typeface);
    }

    [Test]
    public void FindTypefaceNotExistsTest()
    {
        var typeface = GetTypeface("NotExistsTypeface", false, false);
        Assert.NotNull(typeface);
    }

    [Test]
    public void FontHeightTest()
    {
        var font = GetFont(fontFamilyName, false, false, 12);
        Assert.NotNull(font);
        Assert.True(font.Height > font.Size);
    }

    [Test]
    public void FontMetricsTest()
    {
        var font = GetFont(fontFamilyName, false, false, 12);
        var metrics = font.GetMetrics();
        Assert.NotNull(font);
    }

    [Test]
    public void ParagraphTest()
    {
        using var ts = TextStyle.Create(Colors.Red, 20);
        //ts.SetFontFamilies(new[] { FontCollection.DefaultFamilyName });
        using var ps = ParagraphStyle.Create();
        ps.MaxLines = 1;
        using var pb = ParagraphBuilder.Create(ps);
        pb.PushStyle(ts);
        pb.AddText("ABC");
        pb.Pop();
        using var ph = pb.Build();
        ph.Layout(500);
        Console.WriteLine($"{ph.LongestLine} {ph.MaxIntrinsicWidth} {ph.Height}");
    }
}