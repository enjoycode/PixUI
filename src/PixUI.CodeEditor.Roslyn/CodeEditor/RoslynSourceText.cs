using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

/// <summary>
/// 包装Roslyn的SourceText为ITextBuffer
/// </summary>
public sealed class RoslynSourceText : ITextBuffer
{
    public RoslynSourceText(RoslynWorkspace workspace, DocumentId documentId)
    {
        _workspace = workspace;
        _documentId = documentId;
    }

    private readonly RoslynWorkspace _workspace;
    private readonly DocumentId _documentId;
    private SourceText _sourceText = null!;
    public bool HasOpen => _sourceText != null!;

    internal Microsoft.CodeAnalysis.Document GetRoslynDocument() =>
        _workspace.CurrentSolution.GetDocument(_documentId)!;

    public async Task Open()
    {
        //先判断是否已经打开，是则先关闭，主要用于签出后重新加载
        if (_workspace.IsDocumentOpen(_documentId))
            _workspace.CloseDocument(_documentId);

        await _workspace.OpenDocumentAsync(_documentId);

        //从已加载的设计树对应的RoslynDocument中获取源码
        _sourceText = await GetRoslynDocument().GetTextAsync();
    }

    public int Length => _sourceText.Length;

    public SourceText CurrentVersion => _sourceText;

    public void Insert(int offset, string text)
    {
        _sourceText = _sourceText.Replace(offset, 0, text);
        _workspace.OnDocumentChanged(_documentId, _sourceText);
    }

    public void Remove(int offset, int length)
    {
        _sourceText = _sourceText.Replace(offset, length, string.Empty);
        _workspace.OnDocumentChanged(_documentId, _sourceText);
    }

    public void Replace(int offset, int length, string text)
    {
        _sourceText = _sourceText.Replace(offset, length, text);
        _workspace.OnDocumentChanged(_documentId, _sourceText);
    }

    public string GetText(int offset, int length) => _sourceText.ToString(new TextSpan(offset, length));

    public char GetCharAt(int offset) => _sourceText[offset];

    public void SetContent(string text) => Replace(0, _sourceText.Length, text);

    public void CopyTo(Span<char> dest, int offset, int count)
    {
        throw new NotImplementedException();
    }
}