using System;
using System.IO;
using NUnit.Framework;
using PixUI.Drawing.Skia;

namespace PixUI.UnitTests;

public class FontTest
{
    [SetUp]
    public static void Setup()
    {
        Render.Init(new SkiaRender());
    }

    private const string fontFamilyName = "PingFang SC";

    private static ITypeface GetTypeface(string familyName, bool bold, bool italic) =>
        FontCollection.FindTypeface(familyName, bold, italic)!;

    private static IFont GetFont(string familyName, bool bold, bool italic, int sizeInPoints) =>
        GetTypeface(familyName, bold, italic).MakeFont(sizeInPoints);


    [Test]
    public void CreateTypefaceTest()
    {
        //var path = "/Users/rick/Projects/AppBox/ext/PixUI/src/PixUI.Platform.Blazor/wwwroot/fonts/MiSans-Regular.woff2";
        var path = "/Users/rick/Projects/AppBox/ext/PixUI/src/PixUI.Platform.Blazor/wwwroot/fonts/NotoMono-Regular.ttf";
        var fs = File.OpenRead(path);
        var ms = new MemoryStream((int)fs.Length);
        fs.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        // var data = SKData.Create(ms);

        var fontManager = SKFontManager.Default;
        // var typeface = fontManager.CreateTypeface(data);
        var typeface = fontManager.CreateTypeface(ms);
        Assert.NotNull(typeface);
    }

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

        var textBlobPtr = SkiaApi.sk_paragraph_get_line_first_textblob(((SKParagraph)ph).Handle, 0);
        Console.WriteLine(textBlobPtr);
    }
}