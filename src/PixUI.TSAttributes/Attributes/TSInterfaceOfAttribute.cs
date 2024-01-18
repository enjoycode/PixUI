using System;

namespace PixUI;

/// <summary>
/// 用于生成检查对象是否实现了指定接口的代码
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public sealed class TSInterfaceOfAttribute : Attribute { }