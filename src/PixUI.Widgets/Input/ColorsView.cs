namespace PixUI;

/// <summary>
/// 颜色选择视图
/// </summary>
public sealed class ColorsView : Widget
{
    //TODO: 暂简单实现
    
    public ColorsView(State<Color?> state)
    {
        _state = state;
    }

    private readonly State<Color?> _state;
    
}