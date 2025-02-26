using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace CodeEditor;

/// <summary>
/// <para>
/// A collection of text segments that supports efficient lookup of segments
/// intersecting with another segment.
/// </para>
/// </summary>
/// <remarks><inheritdoc cref="TextSegment"/></remarks>
/// <see cref="TextSegment"/>
internal sealed class TextSegmentTree<T> : RedBlackTree<TextSegment>, ISegmentTree, ICollection<T>
    where T : TextSegment
{
    #region ====FirstSegment/LastSegment====

    /// <summary>
    /// Returns the first segment in the collection or null, if the collection is empty.
    /// </summary>
    public T? FirstSegment => (T?)Root?.LeftMost();

    /// <summary>
    /// Returns the last segment in the collection or null, if the collection is empty.
    /// </summary>
    public T? LastSegment => (T?)Root?.RightMost();

    #endregion

    #region ====GetNextSegment / GetPreviousSegment====

    /// <summary>
    /// Gets the next segment after the specified segment.
    /// Segments are sorted by their start offset.
    /// Returns null if segment is the last segment.
    /// </summary>
    public T? GetNextSegment(T segment)
    {
        if (!Contains(segment))
            throw new ArgumentException("segment is not inside the segment tree");
        return (T?)((TextSegment)segment).Successor();
    }

    /// <summary>
    /// Gets the previous segment before the specified segment.
    /// Segments are sorted by their start offset.
    /// Returns null if segment is the first segment.
    /// </summary>
    public T? GetPreviousSegment(T segment)
    {
        if (!Contains(segment))
            throw new ArgumentException("segment is not inside the segment tree");
        return (T?)((TextSegment)segment).Predecessor();
    }

    #endregion

    #region ====FindFirstSegmentWithStartAfter====

    /// <summary>
    /// Gets the first segment with a start offset greater or equal to <paramref name="startOffset"/>.
    /// Returns null if no such segment is found.
    /// </summary>
    public T? FindFirstSegmentWithStartAfter(int startOffset)
    {
        if (Root == null)
            return null;
        if (startOffset <= 0)
            return (T)Root.LeftMost();
        var s = FindNode(ref startOffset);
        // startOffset means that the previous segment is starting at the offset we were looking for
        while (startOffset == 0)
        {
            var p = (s == null) ? Root.RightMost() : s.Predecessor();
            // There must always be a predecessor: if we were looking for the first node, we would have already
            // returned it as root.LeftMost above.
            Debug.Assert(p != null);
            startOffset += p.NodeLength;
            s = p;
        }

        return (T?)s;
    }

    /// <summary>
    /// Finds the node at the specified offset.
    /// After the method has run, offset is relative to the beginning of the returned node.
    /// </summary>
    private TextSegment? FindNode(ref int offset)
    {
        var n = Root!;
        while (true)
        {
            if (n.Left != null)
            {
                if (offset < n.Left.TotalNodeLength)
                {
                    n = n.Left; // descend into left subtree
                    continue;
                }
                else
                {
                    offset -= n.Left.TotalNodeLength; // skip left subtree
                }
            }

            if (offset < n.NodeLength)
            {
                return n; // found correct node
            }
            else
            {
                offset -= n.NodeLength; // skip this node
            }

            if (n.Right != null)
            {
                n = n.Right; // descend into right subtree
            }
            else
            {
                // didn't find any node containing the offset
                return null;
            }
        }
    }

    #endregion

    #region ====FindOverlappingSegments====

    /// <summary>
    /// Finds all segments that contain the given offset.
    /// (StartOffset &lt;= offset &lt;= EndOffset)
    /// Segments are returned in the order given by GetNextSegment/GetPreviousSegment.
    /// </summary>
    /// <returns>Returns a new collection containing the results of the query.
    /// This means it is safe to modify the TextSegmentCollection while iterating through the result collection.</returns>
    public ReadOnlyCollection<T> FindSegmentsContaining(int offset)
    {
        return FindOverlappingSegments(offset, 0);
    }

    /// <summary>
    /// Finds all segments that overlap with the given segment (including touching segments).
    /// </summary>
    /// <returns>Returns a new collection containing the results of the query.
    /// This means it is safe to modify the TextSegmentCollection while iterating through the result collection.</returns>
    public ReadOnlyCollection<T> FindOverlappingSegments(ISegment segment)
    {
        ArgumentNullException.ThrowIfNull(segment);
        return FindOverlappingSegments(segment.Offset, segment.Length);
    }

    /// <summary>
    /// Finds all segments that overlap with the given segment (including touching segments).
    /// Segments are returned in the order given by GetNextSegment/GetPreviousSegment.
    /// </summary>
    /// <returns>Returns a new collection containing the results of the query.
    /// This means it is safe to modify the TextSegmentCollection while iterating through the result collection.</returns>
    public ReadOnlyCollection<T> FindOverlappingSegments(int offset, int length)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(length);

        var results = new List<T>();
        if (Root != null)
        {
            FindOverlappingSegments(results, Root, offset, offset + length);
        }

        return new ReadOnlyCollection<T>(results);
    }

    private static void FindOverlappingSegments(List<T> results, TextSegment node, int low, int high)
    {
        // low and high are relative to node.LeftMost startpos (not node.LeftMost.Offset)
        if (high < 0)
        {
            // node is irrelevant for search because all intervals in node are after high
            return;
        }

        // find values relative to node.Offset
        var nodeLow = low - node.NodeLength;
        var nodeHigh = high - node.NodeLength;
        if (node.Left != null)
        {
            nodeLow -= node.Left.TotalNodeLength;
            nodeHigh -= node.Left.TotalNodeLength;
        }

        if (node.DistanceToMaxEnd < nodeLow)
        {
            // node is irrelevant for search because all intervals in node are before low
            return;
        }

        if (node.Left != null)
            FindOverlappingSegments(results, node.Left, low, high);

        if (nodeHigh < 0)
        {
            // node and everything in node.right is before low
            return;
        }

        if (nodeLow <= node.SegmentLength)
        {
            results.Add((T)node);
        }

        if (node.Right != null)
            FindOverlappingSegments(results, node.Right, nodeLow, nodeHigh);
    }

    #endregion

    #region ====UpdateOffsets====
    
    public void UpdateOffsets(in DocumentEventArgs e)
    {
        // if (_isConnectedToDocument)
        //     throw new InvalidOperationException("This TextSegmentCollection will automatically update offsets; do not call UpdateOffsets manually!");
        // OnDocumentChanged(this, e);
        
        UpdateOffsetsInternal(new OffsetChangeEntry(e.Offset, e.Length, e.Text.Length));
        
        CheckProperties();
    }

    private void UpdateOffsetsInternal(in OffsetChangeEntry change)
    {
        // Special case pure insertions, because they don't always cause a text segment to increase in size when the replaced region
        // is inside a segment (when offset is at start or end of a text semgent).
        if (change.RemovalLength == 0)
        {
            InsertText(change.Offset, change.InsertionLength);
        }
        else
        {
            ReplaceText(change);
        }
    }

    private void InsertText(int offset, int length)
    {
        if (length == 0)
            return;

        // enlarge segments that contain offset (excluding those that have offset as endpoint)
        foreach (var segment in FindSegmentsContaining(offset))
        {
            if (segment.StartOffset < offset && offset < segment.EndOffset)
            {
                segment.Length += length;
            }
        }

        // move start offsets of all segments >= offset
        var node = FindFirstSegmentWithStartAfter(offset);
        if (node != null)
        {
            node.NodeLength += length;
            UpdateAfterChildrenChange(node);
        }
    }

    private void ReplaceText(in OffsetChangeEntry change)
    {
        Debug.Assert(change.RemovalLength > 0);
        var offset = change.Offset;
        foreach (var segment in FindOverlappingSegments(offset, change.RemovalLength))
        {
            if (segment.StartOffset <= offset)
            {
                if (segment.EndOffset >= offset + change.RemovalLength)
                {
                    // Replacement inside segment: adjust segment length
                    segment.Length += change.InsertionLength - change.RemovalLength;
                }
                else
                {
                    // Replacement starting inside segment and ending after segment end: set segment end to removal position
                    //segment.EndOffset = offset;
                    segment.Length = offset - segment.StartOffset;
                }
            }
            else
            {
                // Replacement starting in front of text segment and running into segment.
                // Keep segment.EndOffset constant and move segment.StartOffset to the end of the replacement
                var remainingLength = segment.EndOffset - (offset + change.RemovalLength);
                RemoveSegment(segment);
                segment.StartOffset = offset + change.RemovalLength;
                segment.Length = Math.Max(0, remainingLength);
                AddSegment(segment);
            }
        }

        // move start offsets of all segments > offset
        var node = FindFirstSegmentWithStartAfter(offset + 1);
        if (node != null)
        {
            Debug.Assert(node.NodeLength >= change.RemovalLength);
            node.NodeLength += change.InsertionLength - change.RemovalLength;
            UpdateAfterChildrenChange(node);
        }
    }

    #endregion

    #region ====Add====

    /// <summary>
    /// Adds the specified segment to the tree. This will cause the segment to update when the
    /// document changes.
    /// </summary>
    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        if (item.OwnerTree != null)
            throw new ArgumentException("The segment is already added to a SegmentTree.");
        AddSegment(item);
    }

    void ISegmentTree.Add(TextSegment s) => AddSegment((T)s);

    private void AddSegment(T node)
    {
        var insertionOffset = node.StartOffset;
        node.DistanceToMaxEnd = node.SegmentLength;
        if (Root == null)
        {
            Root = node;
            node.TotalNodeLength = node.NodeLength;
        }
        else if (insertionOffset >= Root.TotalNodeLength)
        {
            // append segment at end of tree
            node.NodeLength = node.TotalNodeLength = insertionOffset - Root.TotalNodeLength;
            InsertAsRight(Root.RightMost(), node);
        }
        else
        {
            // insert in middle of tree
            var n = FindNode(ref insertionOffset);
            Debug.Assert(insertionOffset < n!.NodeLength);
            // split node segment 'n' at offset
            node.TotalNodeLength = node.NodeLength = insertionOffset;
            n.NodeLength -= insertionOffset;
            InsertBefore((T)n, node);
        }

        node.OwnerTree = this;
        Count++;
        CheckProperties();
    }


    private void InsertBefore(TextSegment node, TextSegment newNode)
    {
        if (node.Left == null)
        {
            InsertAsLeft(node, newNode);
        }
        else
        {
            InsertAsRight(node.Left.RightMost(), newNode);
        }
    }

    #endregion

    #region ====Remove====

    /// <summary>
    /// Removes the specified segment from the tree. This will cause the segment to not update
    /// anymore when the document changes.
    /// </summary>
    public bool Remove(T item)
    {
        if (!Contains(item))
            return false;
        RemoveSegment(item);
        return true;
    }

    void ISegmentTree.Remove(TextSegment s) => RemoveSegment(s);

    private void RemoveSegment(TextSegment s)
    {
        var oldOffset = s.StartOffset;
        var successor = s.Successor();
        if (successor != null)
            successor.NodeLength += s.NodeLength;
        RemoveNode(s);
        if (successor != null)
            UpdateAfterChildrenChange(successor);
        Disconnect(s, oldOffset);
        CheckProperties();
    }

    private void Disconnect(TextSegment s, int offset)
    {
        s.Left = s.Right = s.Parent = null;
        s.OwnerTree = null;
        s.NodeLength = offset;
        Count--;
    }

    /// <summary>
    /// Removes all segments from the tree.
    /// </summary>
    public void Clear()
    {
        var segments = this.ToArray();
        Root = null;
        var offset = 0;
        foreach (var s in segments)
        {
            offset += s.NodeLength;
            Disconnect(s, offset);
        }

        CheckProperties();
    }

    #endregion

    #region ====UpdateAugmentedData====

    protected override void UpdateAfterChildrenChange(TextSegment node)
    {
        var totalLength = node.NodeLength;
        var distanceToMaxEnd = node.SegmentLength;
        if (node.Left != null)
        {
            totalLength += node.Left.TotalNodeLength;

            var leftDtme = node.Left.DistanceToMaxEnd;
            // dtme is relative, so convert it to the coordinates of node:
            if (node.Left.Right != null)
                leftDtme -= node.Left.Right.TotalNodeLength;
            leftDtme -= node.NodeLength;
            if (leftDtme > distanceToMaxEnd)
                distanceToMaxEnd = leftDtme;
        }

        if (node.Right != null)
        {
            totalLength += node.Right.TotalNodeLength;

            var rightDtme = node.Right.DistanceToMaxEnd;
            // dtme is relative, so convert it to the coordinates of node:
            rightDtme += node.Right.NodeLength;
            if (node.Right.Left != null)
                rightDtme += node.Right.Left.TotalNodeLength;
            if (rightDtme > distanceToMaxEnd)
                distanceToMaxEnd = rightDtme;
        }

        if (node.TotalNodeLength != totalLength
            || node.DistanceToMaxEnd != distanceToMaxEnd)
        {
            node.TotalNodeLength = totalLength;
            node.DistanceToMaxEnd = distanceToMaxEnd;
            if (node.Parent != null)
                UpdateAfterChildrenChange(node.Parent);
        }
    }

    void ISegmentTree.UpdateAugmentedData(TextSegment node) => UpdateAfterChildrenChange(node);

    #endregion

    #region ====CheckProperties====

    [Conditional("DATACONSISTENCYTEST")]
    internal void CheckProperties()
    {
#if DEBUG
        if (Root != null)
        {
            CheckProperties(Root);

            // check red-black property:
            var blackCount = -1;
            CheckNodeProperties(Root, null, Red, 0, ref blackCount);
        }

        var expectedCount = 0;
        // we cannot trust LINQ not to call ICollection.Count, so we need this loop
        // to count the elements in the tree
        using (var en = GetEnumerator())
        {
            while (en.MoveNext()) expectedCount++;
        }

        Debug.Assert(Count == expectedCount);
#endif
    }

#if DEBUG

    private void CheckProperties(TextSegment node)
    {
        var totalLength = node.NodeLength;
        var distanceToMaxEnd = node.SegmentLength;
        if (node.Left != null)
        {
            CheckProperties(node.Left);
            totalLength += node.Left.TotalNodeLength;
            distanceToMaxEnd = Math.Max(distanceToMaxEnd,
                node.Left.DistanceToMaxEnd + node.Left.StartOffset - node.StartOffset);
        }

        if (node.Right != null)
        {
            CheckProperties(node.Right);
            totalLength += node.Right.TotalNodeLength;
            distanceToMaxEnd = Math.Max(distanceToMaxEnd,
                node.Right.DistanceToMaxEnd + node.Right.StartOffset - node.StartOffset);
        }

        Debug.Assert(node.TotalNodeLength == totalLength);
        Debug.Assert(node.DistanceToMaxEnd == distanceToMaxEnd);
    }

    /*
    1. A node is either red or black.
    2. The root is black.
    3. All leaves are black. (The leaves are the NIL children.)
    4. Both children of every red node are black. (So every red node must have a black parent.)
    5. Every simple path from a node to a descendant leaf contains the same number of black nodes. (Not counting the leaf node.)
     */
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static void CheckNodeProperties(TextSegment? node, TextSegment? parentNode, bool parentColor,
        int blackCount,
        ref int expectedBlackCount)
    {
        if (node == null) return;

        Debug.Assert(node.Parent == parentNode);

        if (parentColor == Red)
        {
            Debug.Assert(node.Color == Black);
        }

        if (node.Color == Black)
        {
            blackCount++;
        }

        if (node.Left == null && node.Right == null)
        {
            // node is a leaf node:
            if (expectedBlackCount == -1)
                expectedBlackCount = blackCount;
            else
                Debug.Assert(expectedBlackCount == blackCount);
        }

        CheckNodeProperties(node.Left, node, node.Color, blackCount, ref expectedBlackCount);
        CheckNodeProperties(node.Right, node, node.Color, blackCount, ref expectedBlackCount);
    }

    private static void AppendTreeToString(TextSegment node, StringBuilder b, int indent)
    {
        b.Append(node.Color == Red ? "RED   " : "BLACK ");
        b.AppendLine(node + node.ToDebugString());
        indent += 2;
        if (node.Left != null)
        {
            b.Append(' ', indent);
            b.Append("L: ");
            AppendTreeToString(node.Left, b, indent);
        }

        if (node.Right != null)
        {
            b.Append(' ', indent);
            b.Append("R: ");
            AppendTreeToString(node.Right, b, indent);
        }
    }
#endif

    internal string GetTreeAsString()
    {
#if DEBUG
        var b = new StringBuilder();
        if (Root != null)
            AppendTreeToString(Root, b, 0);
        return b.ToString();
#else
			return "Not available in release build.";
#endif
    }

    #endregion

    #region ====ICollection====

    /// <summary>
    /// Gets the number of segments in the tree.
    /// </summary>
    public int Count { get; private set; }

    bool ICollection<T>.IsReadOnly => false;

    /// <summary>
    /// Gets whether this tree contains the specified item.
    /// </summary>
    public bool Contains(T? item) => item != null && item.OwnerTree == this;

    /// <summary>
    /// Copies all segments in this SegmentTree to the specified array.
    /// </summary>
    public void CopyTo(T[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (array.Length < Count)
            throw new ArgumentException("The array is too small", nameof(array));
        if (arrayIndex < 0 || arrayIndex + Count > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex), arrayIndex,
                "Value must be between 0 and " + (array.Length - Count));
        foreach (var s in this)
        {
            array[arrayIndex++] = s;
        }
    }

    /// <summary>
    /// Gets an enumerator to enumerate the segments.
    /// </summary>
    public new IEnumerator<T> GetEnumerator()
    {
        if (Root == null) yield break;

        var current = Root.LeftMost();
        while (current != null)
        {
            yield return (T)current;
            // TODO: check if collection was modified during enumeration
            current = current.Successor();
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion
}