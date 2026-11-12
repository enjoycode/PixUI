using Mermaider;
using Mermaider.Layout;
using Mermaider.Models;
using Mermaider.Parsing;

namespace PixUI.Mermaid;

public static class PixMermaidRenderer
{
    public static IPicture RenderPicture(string text)
    {
        var (cleaned, metadata) = DiagramPreprocessor.Process(text);
        var lines = Mermaider.MermaidRenderer.PreprocessLines(cleaned);
        if (lines.Length == 0)
            throw new MermaidParseException("Empty mermaid diagram");

        var diagramType = DiagramDetector.Detect(cleaned.AsSpan());
        switch (diagramType)
        {
            case DiagramType.Flowchart:
            {
                var parsed = FlowchartParser.Parse(lines);
                var positioned = LightweightLayoutEngine.Layout(parsed);
                return PixRenderer.Render(positioned, diagramType);
            }
            case DiagramType.Sequence:
            {
                var parsed = SequenceParser.Parse(lines);
                var positioned = SequenceLayout.Layout(parsed);
                return PixSequenceRenderer.Render(positioned);
            }
            default:
                throw new NotImplementedException($"DiagramType: {diagramType}暂未实现");
        }
    }
}