using System;

namespace PixUI;

public abstract class TSInterceptorAttribute : Attribute { }

/// <summary>
/// 自定义拦截器
/// </summary>
public sealed class TSCustomInterceptorAttribute : TSInterceptorAttribute
{
    /// <param name="customName">自定义拦截器名称</param>
    public TSCustomInterceptorAttribute(string customName) { }
}

/// <summary>
/// 基于模版的拦截器
/// </summary>
public sealed class TSTemplateAttribute : TSInterceptorAttribute
{
    public TSTemplateAttribute(string template) { }
}

/// <summary>
/// 忽略方法调用, 目前仅用于CanvasKit忽略Dispose
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TSIgnoreMethodInvokeAttribute : TSInterceptorAttribute
{
    public TSIgnoreMethodInvokeAttribute() { }
}

[AttributeUsage(AttributeTargets.Property)]
public sealed class TSPropertyToGetSetAttribute : TSInterceptorAttribute { }

/// <summary>
/// 构造转换为工厂方法调用 eg: new AA() 转换为 Factory.MakeAA()
/// </summary>
public sealed class TSCtorToFactoryAttribute : TSInterceptorAttribute
{
    /// <param name="factoryName">工厂静态方法的全名称</param>
    public TSCtorToFactoryAttribute(string factoryName) { }
}