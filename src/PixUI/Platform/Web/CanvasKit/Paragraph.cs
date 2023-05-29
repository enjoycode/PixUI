#if __WEB__
using System;

namespace PixUI
{
    [TSType("CanvasKit.Paragraph")]
    public sealed class Paragraph : IDisposable
    {
        private Paragraph() { }

        [TSPropertyToGetSet] public float LongestLine { get; } = 0;

        [TSPropertyToGetSet] public float Height { get; } = 0;

        [TSPropertyToGetSet] public float MaxIntrinsicWidth { get; } = 0;

        [TSRename("layout")]
        public void Layout(float width) { }

        [TSRename("getGlyphPositionAtCoordinate")]
        public PositionWithAffinity GetGlyphPositionAtCoordinate(float x, float y)
            => new PositionWithAffinity(0, 0);

        [TSTemplate("PixUI.GetRectForPosition({0}, {1}, {2}, {3})")]
        public TextBox GetRectForPosition(int pos,
            BoxHeightStyle heightStyle, BoxWidthStyle widthStyle) => new TextBox();

        [TSRename("delete")]
        public void Dispose() { }
    }
}
#endif