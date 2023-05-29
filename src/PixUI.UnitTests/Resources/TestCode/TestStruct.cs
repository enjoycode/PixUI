struct Point
{
    public static readonly Point Empty = new Point();

    public static Point All(float v) => new Point();
    
    public float X { get; set; }
    public float Y { get; set; }
    

    void Test()
    {
        float? a = 21f;
        float? b = a;

        var array = new Point[3];
        
        var num1 = 1;
        var num2 = num1;
        var p1 = Point.Empty;       // will clone
        var p2 = p1;                // will clone
        PassStruct2(p1);            // not clone
        PassStruct(p1);             // will clone
        PassStruct(new Point());    // not clone

        Point? obj = null;
        var obj2 = obj?.GetPoint(); // not clone
    }

    Point? GetPoint() => null;

    void PassStruct(Point arg) {}
    
    void PassStruct2(in Point arg) {}
    
}