namespace PixUI.Dynamic;

partial class DynamicWidgetManager
{
    private static DynamicWidgetMeta MakeButtonMeta() =>
        new()
        {
            Catelog = "Common", Name = "Button", WidgetType = typeof(Button), ContainerType = ContainerType.None,
            Icon = MaterialIcons.SmartButton,
            CtorArgs = new[]
            {
                new DynamicCtorArgMeta()
                {
                    Name = "Text", AllowNull = true,
                    Value = new DynamicValueMeta
                        { ValueType = typeof(string), IsState = true, DefaultValue = () => new Rx<string>("Button") }
                },
                new DynamicCtorArgMeta()
                {
                    Name = "Icon", AllowNull = true,
                    Value = new DynamicValueMeta { ValueType = typeof(IconData), IsState = true }
                }
            },
            Properties = new[]
            {
                new DynamicPropertyMeta
                {
                    Name = nameof(Button.TextColor),
                    AllowNull = true,
                    Value = new DynamicValueMeta { ValueType = typeof(Color), IsState = true }
                }
            },
        };
}