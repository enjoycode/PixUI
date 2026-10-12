using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

internal sealed class SyntaxDiffer
{
    private const int InitialStackSize = 8;
    private const int MaxSearchLength = 8;
    private readonly Stack<SyntaxNodeOrToken> _oldNodes = new(InitialStackSize);
    private readonly Stack<SyntaxNodeOrToken> _newNodes = new(InitialStackSize);
    private List<ChangeRecord> _changes = [];
    private TextSpan _oldSpan;
    private readonly HashSet<object /*GreenNode*/> _nodeSimilaritySet = [];
    private readonly HashSet<string> _tokenTextSimilaritySet = [];

    internal IList<ChangeRecord> GetChanges(SyntaxTree before, SyntaxTree after)
    {
        var oldNode = before.GetRoot();
        var newNode = after.GetRoot();

        _oldNodes.Push((SyntaxNodeOrToken)oldNode);
        _newNodes.Push((SyntaxNodeOrToken)newNode);

        _oldSpan = oldNode.FullSpan;

        ComputeChangeRecords();
        var result = _changes;

        //Reset before return
        _oldNodes.Clear();
        _newNodes.Clear();
        _changes = [];
        _oldSpan = default;
        _nodeSimilaritySet.Clear();
        _tokenTextSimilaritySet.Clear();

        return result;
    }

    private void ComputeChangeRecords()
    {
        while (true)
        {
            // first check end-of-lists termination cases...
            if (_newNodes.Count == 0)
            {
                // remaining old nodes are deleted
                if (_oldNodes.Count > 0)
                {
                    RecordDeleteOld(_oldNodes.Count);
                }

                break;
            }
            else if (_oldNodes.Count == 0)
            {
                // remaining nodes were inserted
                if (_newNodes.Count > 0)
                {
                    RecordInsertNew(_newNodes.Count);
                }

                break;
            }
            else
            {
                var action = GetNextAction();
                switch (action.Operation)
                {
                    case DiffOp.SkipBoth:
                        RemoveFirst(_oldNodes, action.Count);
                        RemoveFirst(_newNodes, action.Count);
                        break;
                    case DiffOp.ReduceOld:
                        ReplaceFirstWithChildren(_oldNodes);
                        break;
                    case DiffOp.ReduceNew:
                        ReplaceFirstWithChildren(_newNodes);
                        break;
                    case DiffOp.ReduceBoth:
                        ReplaceFirstWithChildren(_oldNodes);
                        ReplaceFirstWithChildren(_newNodes);
                        break;
                    case DiffOp.InsertNew:
                        RecordInsertNew(action.Count);
                        break;
                    case DiffOp.DeleteOld:
                        RecordDeleteOld(action.Count);
                        break;
                    case DiffOp.ReplaceOldWithNew:
                        RecordReplaceOldWithNew(action.Count, action.Count);
                        break;
                }
            }
        }
    }

    private enum DiffOp
    {
        // ReSharper disable once UnusedMember.Local
        None = 0,
        SkipBoth,
        ReduceOld,
        ReduceNew,
        ReduceBoth,
        InsertNew,
        DeleteOld,
        ReplaceOldWithNew
    }

    private readonly struct DiffAction
    {
        public readonly DiffOp Operation;
        public readonly int Count;

        public DiffAction(DiffOp operation, int count)
        {
            Debug.Assert(count >= 0);
            this.Operation = operation;
            this.Count = count;
        }
    }

    private DiffAction GetNextAction()
    {
        bool oldIsToken = _oldNodes.Peek().IsToken;
        bool newIsToken = _newNodes.Peek().IsToken;

        // look for exact match
        int indexOfOldInNew;
        int similarityOfOldInNew;
        int indexOfNewInOld;
        int similarityOfNewInOld;

        FindBestMatch(_newNodes, _oldNodes.Peek(), out indexOfOldInNew, out similarityOfOldInNew);
        FindBestMatch(_oldNodes, _newNodes.Peek(), out indexOfNewInOld, out similarityOfNewInOld);

        if (indexOfOldInNew == 0 && indexOfNewInOld == 0)
        {
            // both first nodes are somewhat similar to each other

            if (AreIdentical(_oldNodes.Peek(), _newNodes.Peek()))
            {
                // they are identical, so just skip over both first new and old nodes.
                return new DiffAction(DiffOp.SkipBoth, 1);
            }
            else if (!oldIsToken && !newIsToken)
            {
                // neither are tokens, so replace each first node with its child nodes
                return new DiffAction(DiffOp.ReduceBoth, 1);
            }
            else
            {
                // otherwise just claim one's text replaces the other.. 
                // NOTE: possibly we can improve this by reducing the side that may not be token?
                return new DiffAction(DiffOp.ReplaceOldWithNew, 1);
            }
        }
        else if (indexOfOldInNew >= 0 || indexOfNewInOld >= 0)
        {
            // either the first old-node is similar to some node in the new-list or
            // the first new-node is similar to some node in the old-list

            if (indexOfNewInOld < 0 || similarityOfOldInNew >= similarityOfNewInOld)
            {
                // either there is no match for the first new-node in the old-list or the 
                // the similarity of the first old-node in the new-list is much greater

                // if we find a match for the old node in the new list, that probably means nodes were inserted before it.
                if (indexOfOldInNew > 0)
                {
                    // look ahead to see if the old node also appears again later in its own list
                    int indexOfOldInOld;
                    int similarityOfOldInOld;
                    FindBestMatch(_oldNodes, _oldNodes.Peek(), out indexOfOldInOld, out similarityOfOldInOld, 1);

                    // don't declare an insert if the node also appeared later in the original list
                    var oldHasSimilarSibling = (indexOfOldInOld >= 1 && similarityOfOldInOld >= similarityOfOldInNew);
                    if (!oldHasSimilarSibling)
                    {
                        return new DiffAction(DiffOp.InsertNew, indexOfOldInNew);
                    }
                }

                if (!newIsToken)
                {
                    if (AreSimilar(_oldNodes.Peek(), _newNodes.Peek()))
                    {
                        return new DiffAction(DiffOp.ReduceBoth, 1);
                    }
                    else
                    {
                        return new DiffAction(DiffOp.ReduceNew, 1);
                    }
                }
                else
                {
                    return new DiffAction(DiffOp.ReplaceOldWithNew, 1);
                }
            }
            else
            {
                if (indexOfNewInOld > 0)
                {
                    return new DiffAction(DiffOp.DeleteOld, indexOfNewInOld);
                }
                else if (!oldIsToken)
                {
                    if (AreSimilar(_oldNodes.Peek(), _newNodes.Peek()))
                    {
                        return new DiffAction(DiffOp.ReduceBoth, 1);
                    }
                    else
                    {
                        return new DiffAction(DiffOp.ReduceOld, 1);
                    }
                }
                else
                {
                    return new DiffAction(DiffOp.ReplaceOldWithNew, 1);
                }
            }
        }
        else
        {
            // no similarities between first node of old-list in new-list or between first new-node in old-list

            if (!oldIsToken && !newIsToken)
            {
                // check similarity anyway
                var sim = GetSimilarity(_oldNodes.Peek(), _newNodes.Peek());
                if (sim >= Math.Max(_oldNodes.Peek().FullSpan.Length, _newNodes.Peek().FullSpan.Length))
                {
                    return new DiffAction(DiffOp.ReduceBoth, 1);
                }
            }

            return new DiffAction(DiffOp.ReplaceOldWithNew, 1);
        }
    }

    private static void ReplaceFirstWithChildren(Stack<SyntaxNodeOrToken> stack)
    {
        var node = stack.Pop();

        int c = 0;
        var children = new SyntaxNodeOrToken[node.ChildNodesAndTokens().Count];
        foreach (var child in node.ChildNodesAndTokens())
        {
            if (child.FullSpan.Length > 0)
            {
                children[c] = child;
                c++;
            }
        }

        for (int i = c - 1; i >= 0; i--)
        {
            stack.Push(children[i]);
        }
    }

    private void FindBestMatch(Stack<SyntaxNodeOrToken> stack, in SyntaxNodeOrToken node, out int index,
        out int similarity, int startIndex = 0)
    {
        index = -1;
        similarity = -1;

        int i = 0;
        foreach (var stackNode in stack)
        {
            if (i >= MaxSearchLength)
            {
                break;
            }

            if (i >= startIndex)
            {
                if (AreIdentical(stackNode, node))
                {
                    var sim = node.FullSpan.Length;
                    if (sim > similarity)
                    {
                        index = i;
                        similarity = sim;
                        return;
                    }
                }
                else if (AreSimilar(stackNode, node))
                {
                    var sim = GetSimilarity(stackNode, node);

                    // Are these really the same? This may be expensive so only check this if 
                    // similarity is rated equal to them being identical.
                    if (sim == node.FullSpan.Length && node.IsToken)
                    {
                        if (stackNode.ToFullString() == node.ToFullString())
                        {
                            index = i;
                            similarity = sim;
                            return;
                        }
                    }

                    if (sim > similarity)
                    {
                        index = i;
                        similarity = sim;
                    }
                }
                else
                {
                    // check one level deep inside list node's children
                    int j = 0;
                    foreach (var child in stackNode.ChildNodesAndTokens())
                    {
                        if (j >= MaxSearchLength)
                        {
                            break;
                        }

                        j++;

                        if (AreIdentical(child, node))
                        {
                            index = i;
                            similarity = node.FullSpan.Length;
                            return;
                        }
                        else if (AreSimilar(child, node))
                        {
                            var sim = GetSimilarity(child, node);
                            if (sim > similarity)
                            {
                                index = i;
                                similarity = sim;
                            }
                        }
                    }
                }
            }

            i++;
        }
    }

    private int GetSimilarity(in SyntaxNodeOrToken node1, in SyntaxNodeOrToken node2)
    {
        // count the characters in the common/identical nodes
        int w = 0;
        _nodeSimilaritySet.Clear();
        _tokenTextSimilaritySet.Clear();

        if (node1.IsToken && node2.IsToken)
        {
            var text1 = node1.ToString();
            var text2 = node2.ToString();

            if (text1 == text2)
            {
                // main text of token is the same
                w += text1.Length;
            }

            foreach (var tr in node1.GetLeadingTrivia())
            {
                Debug.Assert(tr.GetUnderlyingNode() != null);
                _nodeSimilaritySet.Add(tr.GetUnderlyingNode());
            }

            foreach (var tr in node1.GetTrailingTrivia())
            {
                Debug.Assert(tr.GetUnderlyingNode() != null);
                _nodeSimilaritySet.Add(tr.GetUnderlyingNode());
            }

            foreach (var tr in node2.GetLeadingTrivia())
            {
                Debug.Assert(tr.GetUnderlyingNode() != null);
                if (_nodeSimilaritySet.Contains(tr.GetUnderlyingNode()))
                {
                    w += tr.FullSpan.Length;
                }
            }

            foreach (var tr in node2.GetTrailingTrivia())
            {
                Debug.Assert(tr.GetUnderlyingNode() != null);
                if (_nodeSimilaritySet.Contains(tr.GetUnderlyingNode()))
                {
                    w += tr.FullSpan.Length;
                }
            }
        }
        else
        {
            foreach (var n1 in node1.ChildNodesAndTokens())
            {
                Debug.Assert(n1.GetUnderlyingNode() != null);
                _nodeSimilaritySet.Add(n1.GetUnderlyingNode());

                if (n1.IsToken)
                {
                    _tokenTextSimilaritySet.Add(n1.ToString());
                }
            }

            foreach (var n2 in node2.ChildNodesAndTokens())
            {
                Debug.Assert(n2.GetUnderlyingNode() != null);
                if (_nodeSimilaritySet.Contains(n2.GetUnderlyingNode()))
                {
                    w += n2.FullSpan.Length;
                }
                else if (n2.IsToken)
                {
                    var tokenText = n2.ToString();
                    if (_tokenTextSimilaritySet.Contains(tokenText))
                    {
                        w += tokenText.Length;
                    }
                }
            }
        }

        return w;
    }

    private static bool AreIdentical(in SyntaxNodeOrToken node1, in SyntaxNodeOrToken node2)
    {
        return node1.GetUnderlyingNode() == node2.GetUnderlyingNode();
    }

    private static bool AreSimilar(in SyntaxNodeOrToken node1, in SyntaxNodeOrToken node2)
    {
        return node1.RawKind == node2.RawKind;
    }

    internal readonly struct ChangeRecord
    {
        public readonly TextChangeRange Range;
        public readonly Queue<SyntaxNodeOrToken>? OldNodes;
        public readonly Queue<SyntaxNodeOrToken>? NewNodes;

        internal ChangeRecord(TextChangeRange range, Queue<SyntaxNodeOrToken>? oldNodes,
            Queue<SyntaxNodeOrToken>? newNodes)
        {
            this.Range = range;
            this.OldNodes = oldNodes;
            this.NewNodes = newNodes;
        }
    }

    private void RecordDeleteOld(int oldNodeCount)
    {
        var oldSpan = GetSpan(_oldNodes, 0, oldNodeCount);
        var removedNodes = CopyFirst(_oldNodes, oldNodeCount);
        RemoveFirst(_oldNodes, oldNodeCount);
        RecordChange(new ChangeRecord(new TextChangeRange(oldSpan, 0), removedNodes, null));
    }

    private void RecordReplaceOldWithNew(int oldNodeCount, int newNodeCount)
    {
        if (oldNodeCount == 1 && newNodeCount == 1)
        {
            // Avoid creating a Queue<T> which we immediately discard in the most common case for old/new counts
            var removedNode = _oldNodes.Pop();
            var oldSpan = removedNode.FullSpan;

            var insertedNode = _newNodes.Pop();
            var newSpan = insertedNode.FullSpan;

            RecordChange(new TextChangeRange(oldSpan, newSpan.Length), removedNode, insertedNode);
        }
        else
        {
            var oldSpan = GetSpan(_oldNodes, 0, oldNodeCount);
            var removedNodes = CopyFirst(_oldNodes, oldNodeCount);
            RemoveFirst(_oldNodes, oldNodeCount);
            var newSpan = GetSpan(_newNodes, 0, newNodeCount);
            var insertedNodes = CopyFirst(_newNodes, newNodeCount);
            RemoveFirst(_newNodes, newNodeCount);
            RecordChange(new ChangeRecord(new TextChangeRange(oldSpan, newSpan.Length), removedNodes, insertedNodes));
        }
    }

    private void RecordInsertNew(int newNodeCount)
    {
        var newSpan = GetSpan(_newNodes, 0, newNodeCount);
        var insertedNodes = CopyFirst(_newNodes, newNodeCount);
        RemoveFirst(_newNodes, newNodeCount);
        int start = _oldNodes.Count > 0 ? _oldNodes.Peek().GetPosition() : _oldSpan.End;
        RecordChange(new ChangeRecord(new TextChangeRange(new TextSpan(start, 0), newSpan.Length), null,
            insertedNodes));
    }

    private void RecordChange(ChangeRecord change)
    {
        if (_changes.Count > 0)
        {
            var last = _changes[_changes.Count - 1];
            if (last.Range.Span.End == change.Range.Span.Start)
            {
                // merge changes...
                _changes[_changes.Count - 1] = new ChangeRecord(
                    new TextChangeRange(
                        new TextSpan(last.Range.Span.Start, last.Range.Span.Length + change.Range.Span.Length),
                        last.Range.NewLength + change.Range.NewLength),
                    Combine(last.OldNodes, change.OldNodes),
                    Combine(last.NewNodes, change.NewNodes));
                return;
            }

            Debug.Assert(change.Range.Span.Start >= last.Range.Span.End);
        }

        _changes.Add(change);
    }

    private void RecordChange(TextChangeRange textChangeRange, in SyntaxNodeOrToken removedNode,
        SyntaxNodeOrToken insertedNode)
    {
        if (_changes.Count > 0)
        {
            var last = _changes[_changes.Count - 1];
            if (last.Range.Span.End == textChangeRange.Span.Start)
            {
                // merge changes...
                last.OldNodes?.Enqueue(removedNode);
                last.NewNodes?.Enqueue(insertedNode);
                _changes[_changes.Count - 1] = new ChangeRecord(
                    new TextChangeRange(
                        new TextSpan(last.Range.Span.Start, last.Range.Span.Length + textChangeRange.Span.Length),
                        last.Range.NewLength + textChangeRange.NewLength),
                    last.OldNodes ?? CreateQueue(removedNode),
                    last.NewNodes ?? CreateQueue(insertedNode));
                return;
            }

            Debug.Assert(textChangeRange.Span.Start >= last.Range.Span.End);
        }

        _changes.Add(new ChangeRecord(textChangeRange, CreateQueue(removedNode), CreateQueue(insertedNode)));

        // Local Functions
        Queue<SyntaxNodeOrToken> CreateQueue(SyntaxNodeOrToken nodeOrToken)
        {
            var queue = new Queue<SyntaxNodeOrToken>();
            queue.Enqueue(nodeOrToken);
            return queue;
        }
    }

    private static TextSpan GetSpan(Stack<SyntaxNodeOrToken> stack, int first, int length)
    {
        int start = -1, end = -1, i = 0;
        foreach (var n in stack)
        {
            if (i == first)
            {
                start = n.GetPosition();
            }

            if (i == first + length - 1)
            {
                end = n.GetEndPosition();
                break;
            }

            i++;
        }

        Debug.Assert(start >= 0);
        Debug.Assert(end >= 0);

        return TextSpan.FromBounds(start, end);
    }

    private static Queue<SyntaxNodeOrToken>? Combine(Queue<SyntaxNodeOrToken>? first, Queue<SyntaxNodeOrToken>? next)
    {
        if (first == null || first.Count == 0)
        {
            return next;
        }

        if (next == null || next.Count == 0)
        {
            return first;
        }

        foreach (var nodeOrToken in next)
        {
            first.Enqueue(nodeOrToken);
        }

        return first;
    }

    private static Queue<SyntaxNodeOrToken>? CopyFirst(Stack<SyntaxNodeOrToken> stack, int n)
    {
        if (n == 0)
        {
            return null;
        }

        var queue = new Queue<SyntaxNodeOrToken>(n);

        int remaining = n;
        foreach (var node in stack)
        {
            if (remaining == 0)
            {
                break;
            }

            queue.Enqueue(node);
            remaining--;
        }

        return queue;
    }

    private static void RemoveFirst(Stack<SyntaxNodeOrToken> stack, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            stack.Pop();
        }
    }
}