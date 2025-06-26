using System.IO;
using NUnit.Framework;

namespace PixUI.UnitTests.Drawing;

public class CanvasTest
{
    [Test]
    public void RasterTest()
    {
        var imgInfo = new ImageInfo()
        {
            Width = 400,
            Height = 300,
            ColorType = ColorType.Rgba8888,
            AlphaType = AlphaType.Premul
        };
        using var surface = SKSurface.Create(imgInfo);
        var canvas = surface.Canvas;
        canvas.Clear(new Color(255, 0, 0));
        canvas.Flush();

        // using var img = surface.Snapshot();
        // using var imgData = img.Encode(EncodedImageFormat.Png, 100);
    }

    [Test]
    public void PdfTest()
    {
        using var fs = File.OpenWrite("test.pdf");
        using var pdfDoc = SKDocument.CreatePdf(fs);
        using var canvas = pdfDoc.BeginPage(400, 300);
        canvas.Clear(Colors.Gray);
        var paint = Paint.Shared(Colors.Red);
        canvas.DrawRect(20, 20, 100, 50, paint);
        var typeface = FontCollection.Instance.FindTypeface("PingFang SC", false, false);
        var font = new Font(typeface, 16);
        canvas.DrawString("Hello World", 30, 40, font, Colors.Black);
        pdfDoc.EndPage();
        pdfDoc.Close();
    }
}