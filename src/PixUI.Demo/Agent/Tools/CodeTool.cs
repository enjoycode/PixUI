using System.ComponentModel;
using System.Threading.Tasks;

namespace PixUI.Demo.Agent.Tools;

public static class CodeTool
{
    [Description("读取当前示例代码.")]
    public static async Task<string> ReadCurrentDemoCode()
    {
        var doc = PlaygroundContext.Instance.RoslynDocument;
        var text = await doc.GetTextAsync();
        return text.ToString();
    }
}