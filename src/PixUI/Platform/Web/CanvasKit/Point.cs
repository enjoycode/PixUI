using System;

#if __WEB__
namespace PixUI
{
    [TSType("PixUI.Point")]
    public struct Point : IEquatable<Point>
    {
        public static readonly Point Empty = new Point(0, 0);

        public Point(float x, float y)
        {
            X = Y = 0;
        }

        public float X { get; set; }

        public float Y { get; set; }
        
        public void Offset(float dx, float dy) {}

        public bool Equals(Point other) => false;

        public static bool operator ==(Point left, Point right) => left.Equals(right);

        public static bool operator !=(Point left, Point right) => !left.Equals(right);
    }
}
#endif