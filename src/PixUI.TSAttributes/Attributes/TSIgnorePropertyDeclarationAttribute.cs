using System;

namespace PixUI;

/// <summary>
/// 忽略属性定义，目前主要用于显式实现接口导致的重复定义
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class TSIgnorePropertyDeclarationAttribute : Attribute
{
    public TSIgnorePropertyDeclarationAttribute() {}
}