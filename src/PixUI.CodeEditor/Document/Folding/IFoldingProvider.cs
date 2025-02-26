using System.Collections.Generic;

namespace CodeEditor;

public interface IFoldingProvider
{
    IEnumerable<NewFolding> GenerateFoldMarkers(Document document);
}