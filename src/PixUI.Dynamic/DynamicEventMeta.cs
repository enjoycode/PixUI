namespace PixUI.Dynamic;

/// <summary>
/// 动态组件的事件定义
/// </summary>
public sealed class DynamicEventMeta
{
    public DynamicEventMeta(string name)
    {
        Name = name;
    }

    public readonly string Name;
}