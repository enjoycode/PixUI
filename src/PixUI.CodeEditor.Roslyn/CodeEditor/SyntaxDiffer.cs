using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PixUI.CodeEditor.Roslyn;

internal sealed class SyntaxDiffer : Microsoft.CodeAnalysis.SyntaxDiffer
{
    private static readonly SyntaxNode MockNode = SyntaxFactory.EmptyStatement();

    internal SyntaxDiffer() : base(MockNode, MockNode, false)
    {
        SyntaxNodeOrToken nodeOrToken;
    }

    internal IList<ChangeRecord> GetChanges(SyntaxTree oldTree, SyntaxTree newTree)
    {
        _changes.Clear();
        _newNodes.Clear();
        _oldNodes.Clear();

        var oldNode = oldTree.GetRoot();
        _oldNodes.Push(oldNode);
        _newNodes.Push(newTree.GetRoot());
        ref var oldSpan = ref UnsafeAccessors.SyntaxDifferGetOldSpan(this);
        oldSpan = oldNode.FullSpan;

        ComputeChangeRecords();
        return _changes;
    }
}