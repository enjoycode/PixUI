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
public sealed class DynamicPropertyAttribute : Attribute
{
    public DynamicPropertyAttribute(bool allowNull = false, bool initSetter = false,
        object? initValue = null, string? editor = null)
    {
        AllowNull = allowNull;
        Editor = editor;
        InitValue = initValue;
        InitSetter = initSetter;
    }

    /// <summary>
    /// 仅对属性类型为引用类型有效
    /// </summary>
    public readonly bool AllowNull;

    /// <summary>
    /// 指定属性编辑器名称
    /// </summary>
    public readonly string? Editor;

    /// <summary>
    /// 属性的初始化值
    /// </summary>
    public readonly object? InitValue;

    public readonly bool InitSetter;
}