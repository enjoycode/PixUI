namespace PixUI.Dynamic;

public interface IEventAction
{
    /// <summary>
    /// 名称 eg: FetchData or Expression
    /// </summary>
    string ActionName { get; }

    /// <summary>
    /// 运行时执行
    /// </summary>
    /// <param name="dynamicContext"></param>
    /// <param name="eventArg"></param>
    void Run(IDynamicContext dynamicContext, object? eventArg = null);
}