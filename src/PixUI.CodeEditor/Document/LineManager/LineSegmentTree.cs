using System;
using System.Diagnostics;
using PixUI;

namespace CodeEditor
{
    internal struct RBNode
    {
        internal readonly LineSegment LineSegment;
        internal int Count;
        internal int TotalLength;

        public RBNode(LineSegment lineSegment)
        {
            LineSegment = lineSegment;
            Count = 1;
            TotalLength = lineSegment.TotalLength;
        }

        public override string ToString()
        {
            return "[RBNode count=" + Count + " totalLength=" + TotalLength
                   + " lineSegment.LineNumber=" + LineSegment.LineNumber
                   + " lineSegment.Offset=" + LineSegment.Offset
                   + " lineSegment.TotalLength=" + LineSegment.TotalLength
                   + " lineSegment.DelimiterLength=" + LineSegment.DelimiterLength + "]";
        }
    }

    internal struct RBHost : IRedBlackTreeHost<RBNode>
    {
        public int Compare(RBNode x, RBNode y)
        {
            throw new NotImplementedException();
        }

        public bool Equals(RBNode a, RBNode b)
        {
            throw new NotImplementedException();
        }

        public void UpdateAfterChildrenChange(RedBlackTreeNode<RBNode> node)
        {
            int count = 1;
            int totalLength = node.Value.LineSegment.TotalLength;
            if (node.Left != null)
            {
                count += node.Left.Value.Count;
                totalLength += node.Left.Value.TotalLength;
            }

            if (node.Right != null)
            {
                count += node.Right.Value.Count;
                totalLength += node.Right.Value.TotalLength;
            }

            if (count != node.Value.Count || totalLength != node.Value.TotalLength)
            {
                node.Value.Count = count;
                node.Value.TotalLength = totalLength;
                if (node.Parent != null) UpdateAfterChildrenChange(node.Parent);
            }
        }

        public void UpdateAfterRotateLeft(RedBlackTreeNode<RBNode> node)
        {
            UpdateAfterChildrenChange(node);
            UpdateAfterChildrenChange(node.Parent!);
        }

        public void UpdateAfterRotateRight(RedBlackTreeNode<RBNode> node)
        {
            UpdateAfterChildrenChange(node);
            UpdateAfterChildrenChange(node.Parent!);
        }
    }

    public struct LinesEnumerator
    {
        /// <summary>
        /// An invalid enumerator value. Calling MoveNext on the invalid enumerator
        /// will always return false, accessing Current will throw an exception.
        /// </summary>
        public static readonly LinesEnumerator Invalid =
            new LinesEnumerator(new RedBlackTreeIterator<RBNode>(null));

        internal RedBlackTreeIterator<RBNode> Iterator;

        internal LinesEnumerator(RedBlackTreeIterator<RBNode> it)
        {
            Iterator = it;
        }

        public LinesEnumerator Clone() => new LinesEnumerator(Iterator);

        /// <summary>
        /// Gets the current value. Runs in O(1).
        /// </summary>
        public LineSegment Current => Iterator.Current.LineSegment;

        public bool IsValid => Iterator.IsValid;

        /// <summary>
        /// Gets the index of the current value. Runs in O(lg n).
        /// </summary>
        public int CurrentIndex
        {
            get
            {
                if (Iterator.Node == null)
                    throw new InvalidOperationException();
                return GetIndexFromNode(Iterator.Node);
            }
        }

        /// <summary>
        /// Gets the offset of the current value. Runs in O(lg n).
        /// </summary>
        public int CurrentOffset
        {
            get
            {
                if (Iterator.Node == null)
                    throw new InvalidOperationException();
                return GetOffsetFromNode(Iterator.Node);
            }
        }

        /// <summary>
        /// Moves to the next index. Runs in O(lg n), but for k calls, the combined time is only O(k+lg n).
        /// </summary>
        public bool MoveNext() => Iterator.MoveNext();

        /// <summary>
        /// Moves to the previous index. Runs in O(lg n), but for k calls, the combined time is only O(k+lg n).
        /// </summary>
        public bool MoveBack() => Iterator.MoveBack();

        private static int GetIndexFromNode(RedBlackTreeNode<RBNode> node)
        {
            var index = (node.Left != null) ? node.Left.Value.Count : 0;
            while (node.Parent != null)
            {
                if (node == node.Parent.Right)
                {
                    if (node.Parent.Left != null)
                        index += node.Parent.Left.Value.Count;
                    index++;
                }

                node = node.Parent;
            }

            return index;
        }

        private static int GetOffsetFromNode(RedBlackTreeNode<RBNode> node)
        {
            var offset = (node.Left != null) ? node.Left.Value.TotalLength : 0;
            while (node.Parent != null)
            {
                if (node == node.Parent.Right)
                {
                    if (node.Parent.Left != null)
                        offset += node.Parent.Left.Value.TotalLength;
                    offset += node.Parent.Value.LineSegment.TotalLength;
                }

                node = node.Parent;
            }

            return offset;
        }
    }

    /// <summary>
    /// Data structure for efficient management of the line segments (most operations are O(lg n)).
    /// This implements an augmented red-black tree where each node has fields for the number of
    /// nodes in its subtree (like an order statistics tree) for access by index(=line number).
    /// Additionally, each node knows the total length of all segments in its subtree.
    /// This means we can find nodes by offset in O(lg n) time. Since the offset itself is not stored in
    /// the line segment but computed from the lengths stored in the tree, we adjusting the offsets when
    /// text is inserted in one line means we just have to increment the totalLength of the affected line and
    /// its parent nodes - an O(lg n) operation.
    /// However this means getting the line number or offset from a LineSegment is not a constant time
    /// operation, but takes O(lg n).
    /// 
    /// NOTE: The tree is never empty, Clear() causes it to contain an empty segment.
    /// </summary>
    internal sealed class LineSegmentTree
    {
        private readonly RedBlackTree<RBNode, RBHost> _tree =
            new RedBlackTree<RBNode, RBHost>(new RBHost());

        internal RedBlackTreeNode<RBNode> GetNode(int index)
        {
            if (index < 0 || index >= _tree.Count)
                throw new ArgumentOutOfRangeException(nameof(index),
                    "index should be between 0 and " + (_tree.Count - 1));
            RedBlackTreeNode<RBNode> node = _tree.Root;
            while (true)
            {
                if (node.Left != null && index < node.Left.Value.Count)
                {
                    node = node.Left;
                }
                else
                {
                    if (node.Left != null)
                    {
                        index -= node.Left.Value.Count;
                    }

                    if (index == 0)
                        return node;
                    index--;
                    node = node.Right;
                }
            }
        }

        private RedBlackTreeNode<RBNode> GetNodeByOffset(int offset)
        {
            if (offset < 0 || offset > this.TotalLength)
                throw new ArgumentOutOfRangeException(nameof(offset),
                    "offset should be between 0 and " + TotalLength);
            if (offset == this.TotalLength)
            {
                if (_tree.Root == null)
                    throw new InvalidOperationException(
                        "Cannot call GetNodeByOffset while tree is empty.");
                return _tree.Root.RightMost;
            }

            var node = _tree.Root;
            while (true)
            {
                if (node.Left != null && offset < node.Left.Value.TotalLength)
                {
                    node = node.Left;
                }
                else
                {
                    if (node.Left != null)
                    {
                        offset -= node.Left.Value.TotalLength;
                    }

                    offset -= node.Value.LineSegment.TotalLength;
                    if (offset < 0)
                        return node;
                    node = node.Right;
                }
            }
        }

        public LineSegment GetByOffset(int offset)
        {
            return GetNodeByOffset(offset).Value.LineSegment;
        }

        /// <summary>
        /// Gets the total length of all line segments. Runs in O(1).
        /// </summary>
        public int TotalLength => _tree.Root == null ? 0 : _tree.Root.Value.TotalLength;

        /// <summary>
        /// Updates the length of a line segment. Runs in O(lg n).
        /// </summary>
        public void SetSegmentLength(LineSegment segment, int newTotalLength)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));
            RedBlackTreeNode<RBNode> node = segment.TreeEntry.Iterator.Node!;
            segment.TotalLength = newTotalLength;
            new RBHost().UpdateAfterChildrenChange(node);
#if DEBUG && !__WEB__
            CheckProperties();
#endif
        }

        public void RemoveSegment(LineSegment segment)
        {
            _tree.RemoveAt(segment.TreeEntry.Iterator);
#if DEBUG && !__WEB__
            CheckProperties();
#endif
        }

        public LineSegment InsertSegmentAfter(LineSegment segment, int length)
        {
            var newSegment = new LineSegment();
            newSegment.TotalLength = length;
            newSegment.DelimiterLength = segment.DelimiterLength;

            newSegment.TreeEntry = InsertAfter(segment.TreeEntry.Iterator.Node!, newSegment);
            return newSegment;
        }

        LinesEnumerator InsertAfter(RedBlackTreeNode<RBNode> node, LineSegment newSegment)
        {
            RedBlackTreeNode<RBNode> newNode = new RedBlackTreeNode<RBNode>(new RBNode(newSegment));
            if (node.Right == null)
            {
                _tree.InsertAsRight(node, newNode);
            }
            else
            {
                _tree.InsertAsLeft(node.Right.LeftMost, newNode);
            }
#if DEBUG && !__WEB__
            CheckProperties();
#endif
            return new LinesEnumerator(new RedBlackTreeIterator<RBNode>(newNode));
        }

        /// <summary>
        /// Gets the number of items in the collections. Runs in O(1).
        /// </summary>
        public int Count => _tree.Count;

        /// <summary>
        /// Gets  an item by index. Runs in O(lg n).
        /// </summary>
        public LineSegment GetAt(int index) => GetNode(index).Value.LineSegment;

        /// <summary>
        /// Gets the index of an item. Runs in O(lg n).
        /// </summary>
        public int IndexOf(LineSegment item)
        {
            var index = item.LineNumber;
            if (index < 0 || index >= this.Count)
                return -1;
            if (item != GetAt(index))
                return -1;
            return index;
        }

#if DEBUG && !__WEB__
        [Conditional("DATACONSISTENCYTEST")]
        void CheckProperties()
        {
            if (_tree.Root == null)
            {
                Debug.Assert(this.Count == 0);
            }
            else
            {
                Debug.Assert(_tree.Root.Value.Count == this.Count);
                CheckProperties(_tree.Root);
            }
        }

        void CheckProperties(RedBlackTreeNode<RBNode> node)
        {
            int count = 1;
            int totalLength = node.Value.LineSegment.TotalLength;
            if (node.Left != null)
            {
                CheckProperties(node.Left);
                count += node.Left.Value.Count;
                totalLength += node.Left.Value.TotalLength;
            }

            if (node.Right != null)
            {
                CheckProperties(node.Right);
                count += node.Right.Value.Count;
                totalLength += node.Right.Value.TotalLength;
            }

            Debug.Assert(node.Value.Count == count);
            Debug.Assert(node.Value.TotalLength == totalLength);
        }

        public string GetTreeAsString()
        {
            return _tree.GetTreeAsString();
        }
#endif

        public LineSegmentTree()
        {
            Clear();
        }

        /// <summary>
        /// Clears the list. Runs in O(1).
        /// </summary>
        public void Clear()
        {
            _tree.Clear();
            LineSegment emptySegment = new LineSegment();
            emptySegment.TotalLength = 0;
            emptySegment.DelimiterLength = 0;
            _tree.Add(new RBNode(emptySegment));
            emptySegment.TreeEntry = GetEnumeratorForIndex(0);
#if DEBUG && !__WEB__
            CheckProperties();
#endif
        }

        /// <summary>
        /// Tests whether an item is in the list. Runs in O(n).
        /// </summary>
        public bool Contains(LineSegment item)
        {
            return IndexOf(item) >= 0;
        }

        // public Enumerator GetEnumerator()
        // {
        //     return new LinesEnumerator(tree.GetEnumerator());
        // }

        public LinesEnumerator GetEnumeratorForIndex(int index)
        {
            return new LinesEnumerator(new RedBlackTreeIterator<RBNode>(GetNode(index)));
        }

        public LinesEnumerator GetEnumeratorForOffset(int offset)
        {
            return new LinesEnumerator(new RedBlackTreeIterator<RBNode>(GetNodeByOffset(offset)));
        }
    }
}