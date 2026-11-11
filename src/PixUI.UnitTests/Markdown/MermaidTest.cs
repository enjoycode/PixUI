using NUnit.Framework;
using Mermaider.Layout;

namespace PixUI.UnitTests.Markdown;

public class MermaidTest
{
    [Test]
    public void ParseTest()
    {
        var code = """
                      graph TD
                      A[开始] --> B{判断}
                      B -->|条件1| C[处理1]
                      B -. 条件2 .-> D[处理2]
                      C --> E[结束]
                      D --> E
                      """;
       var graph = Mermaider.MermaidRenderer.Parse(code);
       var layouted = LightweightLayoutEngine.Layout(graph);
    }
}