[PixUI.TSType("CanvasKit.FontWeight")]
public enum FontWeight
{
    Invisible = 0,
    Normal,
    Bold,
}

enum EntityType
{
    Type1 = 1,
    Typd2 = 2,
}

class TestClass
{
    public FontWeight? FontWeight;

    string Test()
    {
        var type = EntityType.Type1;
        return type.ToString();
    }
}

