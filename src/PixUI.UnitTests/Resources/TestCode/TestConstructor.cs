using PixUI;

class BaseClass
{
    public readonly string Type;

    public BaseClass(string type)
    {
        Type = type;
    }
}

class Widget : BaseClass
{
    public string Text { get; set; }
    
    public int? Width { get; set; }
    
    public int? Height { get; set; }

    public Widget(string text): base("Widget")
    {
        Text = text;
    }

    public static Widget Make()
    {
        // var b = new BaseClass("aa") { Type = "bb" };
        return new Widget("Hello") { Height = 10, Width = 5 };
    }
}