using System;

namespace PixUI
{
    /// <summary>
    /// 转换Indexer的set为方法
    /// </summary>
    /// <summary>
    /// <code>
    /// obj[idx] = 32
    /// </code>
    /// 转换为
    /// <code>
    /// obj.SetAt(idx, 32)
    /// </code>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TSIndexerSetToMethodAttribute : Attribute { }
}