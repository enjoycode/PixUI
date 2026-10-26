using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Structure;

namespace PixUI.CodeEditor.Roslyn;

public static class FoldingService
{
    public static async Task<ImmutableArray<NewFolding>> GetFoldings(Microsoft.CodeAnalysis.Document document,
        CancellationToken cancellationToken = default)
    {
        var blockStructureService = BlockStructureService.GetService(document);
        if (blockStructureService == null) return [];

        var options = new BlockStructureOptions()
        {
            ShowBlockStructureGuidesForCommentsAndPreprocessorRegions = false,
            ShowOutliningForCommentsAndPreprocessorRegions = true,
        };
        var result = await blockStructureService.GetBlockStructureAsync(document, options, cancellationToken);

        var text = await document.GetTextAsync(cancellationToken);
        return result.Spans
            .Where(s => s.IsCollapsible && text.Lines.GetLinePosition(s.TextSpan.Start).Line !=
                text.Lines.GetLinePosition(s.TextSpan.End).Line) //暂简单处理同一行的block
            .Select(s => new NewFolding(s.TextSpan.Start, s.TextSpan.Length, $"{{{s.BannerText}}}"))
            .OrderBy(s => s.Offset)
            .ToImmutableArray();
    }
}