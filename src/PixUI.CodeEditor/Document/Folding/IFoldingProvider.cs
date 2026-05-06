using System.Collections.Generic;

namespace PixUI.CodeEditor;

public interface IFoldingProvider
{
    IEnumerable<NewFolding> GenerateFoldMarkers(Document document);
}