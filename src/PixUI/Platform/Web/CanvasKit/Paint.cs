#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Paint")]
    public sealed class Paint : IDisposable
    {
        // public static Paint Shared(Color? color = null,PaintStyle style = PaintStyle.Fill,
        //     float strokeWidth = 1) => new Paint();

        [TSTemplate("new CanvasKit.Paint()")]
        public Paint() { }

        [TSPropertyToGetSet] public Color Color { get; set; }

        [TSPropertyToGetSet] public PaintStyle Style { get; set; }

        [TSPropertyToGetSet] public float StrokeWidth { get; set; }

        [TSPropertyToGetSet] public float StrokeMiter { get; set; }

        [TSPropertyToGetSet] public StrokeCap StrokeCap { get; set; }

        [TSPropertyToGetSet] public StrokeJoin StrokeJoin { get; set; }

        [TSPropertyToGetSet] public bool AntiAlias { get; set; }

        [TSPropertyToGetSet] public MaskFilter? MaskFilter { get; set; }

        [TSPropertyToGetSet] public Shader? Shader { get; set; }

        [TSPropertyToGetSet] public ColorFilter? ColorFilter { get; set; }

        [TSPropertyToGetSet] public ImageFilter? ImageFilter { get; set; }

        [TSPropertyToGetSet] public PathEffect? PathEffect { get; set; }

        [TSRename("delete")]
        public void Dispose() { }
    }
}
#endif