using System.Collections.Generic;

namespace CodeEditor;

public interface IFoldingProvider
{
    List<FoldMarker>? GenerateFoldMarkers(Document document);
}