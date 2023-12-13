using System.Text.Json;

namespace PixUI.Dynamic;

public interface IEventAction
{
    string ActionName { get; }
    
    /// <summary>
    /// 写入属性值，不需要写入ActionName及EndObject
    /// </summary>
    void WriteProperties(Utf8JsonWriter writer);

    /// <summary>
    /// 读取属性值，需要读取EndObject判断结束
    /// </summary>
    void ReadProperties(ref Utf8JsonReader reader);

    void Run(IDynamicContext dynamicContext, object? eventArg = null);
}