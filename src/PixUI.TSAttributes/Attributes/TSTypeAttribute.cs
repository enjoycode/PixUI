using System;

namespace PixUI;

/// <summary>
/// 将C#类转换为对应的TS类型 eg: Color 转为 Float32Array
/// 1. 标记的类型定义不转换;
/// 2. 标记的参数定义转换为runtimeType;
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum |
                AttributeTargets.Delegate | AttributeTargets.Parameter)]
public sealed class TSTypeAttribute : Attribute
{
    public TSTypeAttribute(string runtimeType) { }
}