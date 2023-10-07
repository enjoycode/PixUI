using System;

namespace PixUI;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DynamicWidgetAttribute : Attribute
{
    public DynamicWidgetAttribute(string catalog = "", string name = "", string icon = "")
    {
        Catalog = catalog;
        Name = name;
        Icon = icon;
    }

    public readonly string Catalog;
    public readonly string Name;
    public readonly string Icon;
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class DynamicPropertyAttribute : Attribute { }