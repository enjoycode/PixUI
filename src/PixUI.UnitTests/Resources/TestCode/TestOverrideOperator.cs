struct Point
{
    public int X;
    public int Y;

    public static Point operator *(Point pt, int operand) =>
        new Point() { X = pt.X * operand, Y = pt.Y * operand };

    public static bool operator ==(Point d1, Point d2) => false;
    
    public static bool operator !=(Point d1, Point d2) => false;
    
    public static bool operator >=(Point d1, Point d2) => false;
    
    public static bool operator <=(Point d1, Point d2) => false;

    public void Test()
    {
        var a = new Point();
        var b = a * 3;
        var e1 = a <= b;
        var e2 = a == b;
        Point? c = null;
        if (c == null /*不处理*/) {}
    }
}