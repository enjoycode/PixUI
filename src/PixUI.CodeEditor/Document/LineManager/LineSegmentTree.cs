using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CodeEditor;

/// <summary>
/// Data structure for efficient management of the line segments (most operations are O(lg n)).
/// This implements an augmented red-black tree where each node has fields for the number of
/// nodes in its subtree (like an order statistics tree) for access by index(=line number).
/// Additionally, each node knows the total length of all segments in its subtree.
/// This means we can find nodes by offset in O(lg n) time. Since the offset itself is not stored in
/// the line segment but computed from the lengths stored in the tree, we're adjusting the offsets when
/// text is inserted in one line means we just have to increment the totalLength of the affected line and
/// its parent nodes - an O(lg n) operation.
/// However, this means getting the line number or offset from a LineSegment is not a constant time
/// operation, but takes O(lg n).
/// 
/// NOTE: The tree is never empty, Clear() causes it to contain an empty segment.
/// </summary>
internal sealed class LineSegmentTree : RedBlackTree<LineSegment>
{
    public LineSegmentTree()
    {
        var empty = new LineSegment();
        Root = empty.InitLineNode();
    }

    #region ====GetNodeByXXX / GetXXXFromNode====

    internal LineSegment GetNodeByIndex(int index)
    {
        Debug.Assert(index >= 0 && index < Root!.NodeTotalCount);
        var node = Root;
        while (true)
        {
            if (node!.Left != null && index < node.Left.NodeTotalCount)
            {
                node = node.Left;
            }
            else
            {
                if (node.Left != null)
                {
                    index -= node.Left.NodeTotalCount;
                }

                if (index == 0)
                    return node;
                index--;
                node = node.Right;
            }
        }
    }

    internal static int GetIndexFromNode(LineSegment node)
    {
        var index = node.Left?.NodeTotalCount ?? 0;
        while (node.Parent != null)
        {
            if (node == node.Parent.Right)
            {
                if (node.Parent.Left != null)
                    index += node.Parent.Left.NodeTotalCount;
                index++;
            }

            node = node.Parent;
        }

        return index;
    }

    internal LineSegment GetNodeByOffset(int offset)
    {
        Debug.Assert(offset >= 0 && offset <= Root!.NodeTotalLength);
        if (offset == Root.NodeTotalLength)
        {
            return Root.RightMost();
        }

        var node = Root;
        while (true)
        {
            if (node!.Left != null && offset < node.Left.NodeTotalLength)
            {
                node = node.Left;
            }
            else
            {
                if (node.Left != null)
                {
                    offset -= node.Left.NodeTotalLength;
                }

                offset -= node.TotalLength;
                if (offset < 0)
                    return node;
                node = node.Right;
            }
        }
    }

    internal static int GetOffsetFromNode(LineSegment node)
    {
        var offset = node.Left?.NodeTotalLength ?? 0;
        while (node.Parent != null)
        {
            if (node == node.Parent.Right)
            {
                if (node.Parent.Left != null)
                    offset += node.Parent.Left.NodeTotalLength;
                offset += node.Parent.TotalLength;
            }

            node = node.Parent;
        }

        return offset;
    }

    #endregion

    #region ====RedBlackTree Update====

    protected override void UpdateAfterChildrenChange(LineSegment node)
    {
        var totalCount = 1;
        var totalLength = node.TotalLength;
        if (node.Left != null)
        {
            totalCount += node.Left.NodeTotalCount;
            totalLength += node.Left.NodeTotalLength;
        }

        if (node.Right != null)
        {
            totalCount += node.Right.NodeTotalCount;
            totalLength += node.Right.NodeTotalLength;
        }

        if (totalCount != node.NodeTotalCount
            || totalLength != node.NodeTotalLength)
        {
            node.NodeTotalCount = totalCount;
            node.NodeTotalLength = totalLength;
            if (node.Parent != null)
                UpdateAfterChildrenChange(node.Parent);
        }
    }

    protected override void UpdateAfterRotate(LineSegment node)
    {
        UpdateAfterChildrenChange(node);

        // not required: rotations only happen on insertions/deletions
        // -> totalCount changes -> the parent is always updated
        //UpdateAfterChildrenChange(node.parent);
    }

    #endregion

    #region ====CheckProperties====

#if DEBUG
    [Conditional("RED_BLACK_CHECK")]
    internal void CheckProperties()
    {
        CheckProperties(Root!);

        // check red-black property:
        var blackCount = -1;
        CheckNodeProperties(Root, null, Red, 0, ref blackCount);
    }

    private static void CheckProperties(LineSegment node)
    {
        var totalCount = 1;
        var totalLength = node.TotalLength;
        if (node.Left != null)
        {
            CheckProperties(node.Left);
            totalCount += node.Left.NodeTotalCount;
            totalLength += node.Left.NodeTotalLength;
        }

        if (node.Right != null)
        {
            CheckProperties(node.Right);
            totalCount += node.Right.NodeTotalCount;
            totalLength += node.Right.NodeTotalLength;
        }

        Debug.Assert(node.NodeTotalCount == totalCount);
        Debug.Assert(node.NodeTotalLength == totalLength);
    }

    /*
    1. A node is either red or black.
    2. The root is black.
    3. All leaves are black. (The leaves are the NIL children.)
    4. Both children of every red node are black. (So every red node must have a black parent.)
    5. Every simple path from a node to a descendant leaf contains the same number of black nodes. (Not counting the leaf node.)
     */
    [SuppressMessage("ReSharper", "UnusedParameter.Local")]
    private static void CheckNodeProperties(LineSegment? node, LineSegment? parentNode, bool parentColor,
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

    [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    public string GetTreeAsString()
    {
        var b = new StringBuilder();
        AppendTreeToString(Root!, b, 0);
        return b.ToString();
    }

    private static void AppendTreeToString(LineSegment node, StringBuilder b, int indent)
    {
        b.Append(node.Color == Red ? "RED   " : "BLACK ");
        b.AppendLine(node.ToString());
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

    #endregion

    /// <summary>
    /// Updates the length of a line segment. Runs in O(lg n).
    /// </summary>
    public void SetSegmentLength(LineSegment segment, int newTotalLength)
    {
        if (segment == null)
            throw new ArgumentNullException(nameof(segment));
        segment.TotalLength = newTotalLength;
        UpdateAfterChildrenChange(segment);
#if DEBUG
        CheckProperties();
#endif
    }

    public void RemoveSegment(LineSegment segment)
    {
        RemoveNode(segment);
#if DEBUG
        CheckProperties();
#endif
    }

    public LineSegment InsertSegmentAfter(LineSegment segment, int length)
    {
        var newSegment = new LineSegment();
        newSegment.TotalLength = length;
        newSegment.DelimiterLength = segment.DelimiterLength;
        InsertAfter(segment, newSegment);
        return newSegment;
    }

    private void InsertAfter(LineSegment node, LineSegment newSegment)
    {
        newSegment.InitLineNode();
        if (node.Right == null)
        {
            InsertAsRight(node, newSegment);
        }
        else
        {
            InsertAsLeft(node.Right.LeftMost(), newSegment);
        }
#if DEBUG
        CheckProperties();
#endif
    }

    /// <summary>
    /// Gets the number of items in the collections. Runs in O(1).
    /// </summary>
    public int Count
    {
        get
        {
            if (Root!.NodeTotalCount == 1 && Root.NodeTotalLength == 0)
                return 0;
            return Root.NodeTotalCount;
        }
    }

    /// <summary>
    /// Clears the list. Runs in O(1).
    /// </summary>
    public void Clear()
    {
        var empty = new LineSegment();
        Root = empty.InitLineNode();
#if DEBUG
        CheckProperties();
#endif
    }

    // /// <summary>
    // /// Gets the total length of all line segments. Runs in O(1).
    // /// </summary>
    // public int TotalLength => Root?.TotalLength ?? 0;

    // /// <summary>
    // /// Gets an item by index. Runs in O(lg n).
    // /// </summary>
    // public LineSegment GetAt(int index) => GetNodeByIndex(index);

    // /// <summary>
    // /// Gets the index of an item. Runs in O(lg n).
    // /// </summary>
    // public int IndexOf(LineSegment item)
    // {
    //     var index = item.LineNumber;
    //     if (index < 0 || index >= Count)
    //         return -1;
    //     if (item != GetAt(index))
    //         return -1;
    //     return index;
    // }

    // /// <summary>
    // /// Tests whether an item is in the list. Runs in O(n).
    // /// </summary>
    // public bool Contains(LineSegment item)
    // {
    //     return IndexOf(item) >= 0;
    // }
}