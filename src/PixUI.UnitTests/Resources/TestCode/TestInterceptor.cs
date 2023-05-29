using PixUI;

[TSType("Float32Array")]
public struct Point
{
    [TSTemplate("new Float32Array([{1}, {2}])")]
    public Point(float x, float y)
    {
        X = Y = 0;
    }

    [TSTemplate("{0}[0]")] public float X { get; }

    [TSTemplate("{0}[1]")] public float Y { get; }
}

class Canvas
{
    [TSPropertyToGetSet] public float Width { get; set; }
    
    [TSRename("drawRect")]
    public void DrawRect(float width) {}
    
    [TSMethodIngnore]
    public void Dispose() {}
}

class TestClass
{
    void Test()
    {
        var x = 10;
        var p = new Point(x + 10, 20);
        System.Console.Write(p.X);
        
        var c = new Canvas();
        // c.Width++;
        // c.Width += 1;
        c.DrawRect(c.Width);
        
        c.Dispose(); //will be ingnore
    }
}

