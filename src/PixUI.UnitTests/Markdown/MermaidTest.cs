using System;
using Mermaider;
using NUnit.Framework;
using Mermaider.Layout;
using Mermaider.Parsing;

namespace PixUI.UnitTests.Markdown;

public class MermaidTest
{
    [Test]
    public void ParseTest()
    {
        // var code = """
        //               graph TD
        //               A((开始)) --> B{判断}
        //               B -->|条件1| C[处理1]
        //               B -. 条件2 .-> D[处理2]
        //               C --> E[结束]
        //               D --> E
        //               """;
        var mermaid = """
                      sequenceDiagram
                      participant A as Alice
                      participant B as Bob
                      A->>B: Hello Bob!
                      B-->>A: Hi Alice!
                      A->>B: How are you?
                      B-->>A: Great, thanks!
                      """;

        var (cleaned, metadata) = DiagramPreprocessor.Process(mermaid);
        var lines = Mermaider.MermaidRenderer.PreprocessLines(cleaned);
        if (lines.Length == 0)
            throw new MermaidParseException("Empty mermaid diagram");

        var diagramType = DiagramDetector.Detect(cleaned.AsSpan());
        var parsed = SequenceParser.Parse(lines);
        var positioned = SequenceLayout.Layout(parsed);
    }
}