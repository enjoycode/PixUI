using System;

#if __WEB__
namespace PixUI
{
    [TSType("PixUI.Rect")]
    public struct Rect : IEquatable<Rect>
    {
        public static readonly Rect Empty = new Rect(0, 0, 0, 0);

        public Rect(float left, float top, float right, float bottom)
        {
            Left = Top = Right = Bottom = 0;
        }

        public static Rect FromLTWH(float left, float top, float width, float height)
            => new Rect(left, top, width, height);

        public bool IsEmpty => false;

        public float Left { get; }

        public float Top { get; }

        public float Right { get; }

        public float Bottom { get; }

        public float Width => 0;

        public float Height => 0;
        
        public float MidX => 0;

        public float MidY => 0;

        public void Offset(float x, float y) { }

        [TSRename("IntersectTo")]
        public void Intersect(in Rect other) {}

        public bool ContainsPoint(float x, float y) => false;

        public bool IntersectsWith(float x, float y, float w, float h) => false;

        public bool Equals(Rect other) => false;

        public static bool operator ==(PixUI.Rect left, PixUI.Rect right) =>
            left.Equals(right);

        public static bool operator !=(PixUI.Rect left, PixUI.Rect right) =>
            !left.Equals(right);
    }
}
#endif