using System.Collections.Generic;

namespace CodeEditor;

public interface IFoldingProvider
{
    List<FoldingSegment>? GenerateFoldMarkers(Document document);
}