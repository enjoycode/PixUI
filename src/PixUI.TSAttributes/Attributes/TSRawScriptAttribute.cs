using System;

namespace PixUI;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Event)]
public sealed class TSRawScriptAttribute : Attribute
{
    public TSRawScriptAttribute(string script) { }
}