using System;
using PixUI;

namespace CodeEditor;

/// <summary>
/// <p> This class represents an immutable character sequence with 
/// fast {@link #concat concatenation}, {@link #insert insertion} and 
/// {@link #delete deletion} capabilities (O[Log(n)]) instead of 
/// O[n] for StringBuffer/StringBuilder).</p>
/// 
/// <p><i> Implementation Note: To avoid expensive copy operations , 
/// {@link ImmutableText} instances are broken down into smaller immutable 
/// sequences, they form a minimal-depth binary tree.
/// The tree is maintained balanced automatically through <a 
/// href="http://en.wikipedia.org/wiki/Tree_rotation">tree rotations</a>. 
/// Insertion/deletions are performed in <code>O[Log(n)]</code>
/// instead of <code>O[n]</code> for 
/// <code>StringBuffer/StringBuilder</code>.</i></p>
/// </summary>
public sealed class ImmutableText
{
    /// <summary>Holds the default size for primitive blocks of characters.</summary>
    private const int BlockSize = 1 << 6;

    /// <summary>Holds the mask used to ensure a block boundary cesures.</summary>
    private const int BlockMask = ~(BlockSize - 1);

    private static readonly LeafNode EmptyNode =
#if __WEB__
            new LeafNode(new Uint16Array(0));
#else
        new LeafNode(Array.Empty<char>());
#endif

    public static readonly ImmutableText Empty = new ImmutableText(EmptyNode);

    private readonly Node _root;

    private int _hash;

    /// <summary>
    /// Returns the length of this text.
    /// </summary>
    /// <value>the number of characters (16-bits Unicode) composing this text.</value>
    public int Length => _root.Length;

    private volatile InnerLeaf? myLastLeaf;

    /// <summary>
    /// Gets a single character.
    /// Runs in O(lg N) for random access. Sequential read-only access benefits from a special optimization and runs in amortized O(1).
    /// </summary>
    public char GetCharAt(int index)
    {
        if (_root is LeafNode)
            return _root.GetCharAt(index);

        var leaf = myLastLeaf;
        if (leaf == null || index < leaf.Offset ||
            index >= leaf.Offset + leaf.LeafNode.Length)
        {
            myLastLeaf = leaf = FindLeaf(index, 0);
        }

        return leaf.LeafNode.GetCharAt(index - leaf.Offset);
    }

    private ImmutableText(Node node)
    {
        _root = node;
    }

    public static ImmutableText FromString(string str)
    {
#if __WEB__
            return new ImmutableText(new LeafNode(Uint16Array.FromString(str)));
#else
        return new ImmutableText(new LeafNode(str.ToCharArray()));
#endif
    }

    /// <summary>
    /// Concatenates the specified text to the end of this text. 
    /// This method is very fast (faster even than 
    /// <code>StringBuffer.append(String)</code>) and still returns
    /// a text instance with an internal binary tree of minimal depth!
    /// </summary>
    /// <param name="that">that the text that is concatenated.</param>
    /// <returns><code>this + that</code></returns>
    private ImmutableText Concat(ImmutableText that)
    {
        return that.Length == 0 ? this :
            Length == 0 ? that :
            new ImmutableText(ConcatNodes(EnsureChunked()._root, that.EnsureChunked()._root));
    }

    /// <summary>
    /// Returns the text having the specified text inserted at 
    /// the specified location.
    /// </summary>
    /// <param name="index">index the insertion position.</param>
    /// <param name="txt">txt the text being inserted.</param>
    /// <returns>subtext(0, index).concat(txt).concat(subtext(index))</returns>
    /// <exception cref="IndexOutOfRangeException">if <code>(index &lt; 0) || (index &gt; this.Length)</code></exception>
    public ImmutableText InsertText(int index, string txt)
    {
        return GetText(0, index).Concat(FromString(txt)).Concat(SubText(index));
    }

    /// <summary>
    /// Returns the text without the characters between the specified indexes.
    /// </summary>
    /// <returns><code>subtext(0, start).concat(subtext(end))</code></returns>
    public ImmutableText RemoveText(int start, int count)
    {
        if (count == 0)
            return this;
        var end = start + count;
        if (end > Length)
            throw new IndexOutOfRangeException();
        return EnsureChunked().GetText(0, start).Concat(SubText(end));
    }

    /// <summary>
    /// Returns a portion of this text.
    /// </summary>
    /// <returns>the sub-text starting at the specified start position and ending just before the specified end position.</returns>
    public ImmutableText GetText(int start, int count)
    {
        var end = start + count;
        if ((start < 0) || (start > end) || (end > Length))
        {
            throw new IndexOutOfRangeException(" start :" + start + " end :" + end +
                                               " needs to be between 0 <= " + Length);
        }

        if ((start == 0) && (end == Length))
        {
            return this;
        }

        if (start == end)
        {
            return Empty;
        }

        return new ImmutableText(_root.SubNode(start, end));
    }

    /// <summary>
    /// Copies the a part of the immutable text into the specified array.
    /// Runs in O(lg N + M).
    /// </summary>
    /// <remarks>
    /// This method counts as a read access and may be called concurrently to other read accesses.
    /// </remarks>
#if __WEB__
        public void CopyTo(int srcOffset, Uint16Array dest, int count)
#else
    public void CopyTo(int srcOffset, Span<char> dest, int count)
#endif
    {
        VerifyRange(srcOffset, count);
        _root.CopyTo(srcOffset, dest, count);
    }

    private void VerifyRange(int startIndex, int length)
    {
        if (startIndex < 0 || startIndex > Length)
        {
            throw new ArgumentOutOfRangeException(nameof(startIndex),
                $"0 <= startIndex <= {Length}");
        }

        if (length < 0 || startIndex + length > Length)
        {
            throw new ArgumentOutOfRangeException(nameof(length),
                $"0 <= length, startIndex({startIndex})+length({length}) <= {length} ");
        }
    }

    public override string ToString() => _root.ToString();

    [TSRawScript(@"
        public GetString(offset: number, length: number): string {
            let data = new Uint16Array(length);
            this.CopyTo(offset, data, length);
            // @ts-ignore
            return String.fromCharCode.apply(null, data);
        }
")]
    public string GetString(int offset, int length)
    {
#if __WEB__
            throw new Exception();
#else
        var data = new char[length];
        CopyTo(offset, data, length);
        return new string(data);
#endif
    }

    // public void WriteTo(TextWriter output, int index, int count)
    // {
    //     while (index < index + count)
    //     {
    //         output.Write(GetCharAt(index));
    //         index++;
    //     }
    // }

    public override bool Equals(object? obj)
    {
        if (this == obj)
            return true;

        if (obj is ImmutableText that)
        {
            var len = Length;
            if (len != that.Length)
                return false;

            for (var i = 0; i < len; i++)
            {
                if (GetCharAt(i) != that.GetCharAt(i))
                    return false;
            }

            return true;
        }

        return false;
    }


    public override int GetHashCode()
    {
        int h = _hash;
        if (h == 0)
        {
            for (int off = 0; off < Length; off++)
            {
                h = 31 * h + GetCharAt(off);
            }

            _hash = h;
        }

        return h;
    }

    #region ====Helper methods====

    private ImmutableText SubText(int start)
    {
        return GetText(start, Length - start);
    }

    /// <summary>
    /// When first loaded, ImmutableText contents are stored as a single large array. This saves memory but isn't
    /// modification-friendly as it disallows slightly changed texts to retain most of the internal structure of the
    /// original document. Whoever retains old non-chunked version will use more memory than really needed.
    /// </summary>
    /// <returns>A copy of this text better prepared for small modifications to fully enable structure-sharing capabilities</returns>
    private ImmutableText EnsureChunked()
    {
        if (Length > BlockSize && _root is LeafNode)
        {
            return new ImmutableText(NodeOf((LeafNode)_root, 0, Length));
        }

        return this;
    }

    static Node NodeOf(LeafNode node, int offset, int length)
    {
        if (length <= BlockSize)
        {
            return node.SubNode(offset, offset + length);
        }

        // Splits on a block boundary.
        int half = ((length + BlockSize) >> 1) & BlockMask;
        return new CompositeNode(NodeOf(node, offset, half),
            NodeOf(node, offset + half, length - half));
    }

    internal static Node ConcatNodes(Node node1, Node node2)
    {
        // All Text instances are maintained balanced:
        //   (head < tail * 2) & (tail < head * 2)
        var length = node1.Length + node2.Length;
        if (length <= BlockSize)
        {
            // Merges to primitive.
#if __WEB__
                var mergedArray = new Uint16Array(node1.Length + node2.Length);
                node1.CopyTo(0, mergedArray, node1.Length);
                node2.CopyTo(0, mergedArray.subarray(node1.Length), node2.Length);
                return new LeafNode(mergedArray);
#else
            var mergedArray = new char[node1.Length + node2.Length];
            node1.CopyTo(0, mergedArray, node1.Length);
            node2.CopyTo(0, mergedArray.AsSpan().Slice(node1.Length), node2.Length);
            return new LeafNode(mergedArray);
#endif
        }

        // Returns a composite.
        var head = node1;
        var tail = node2;
        if ((head.Length << 1) < tail.Length && tail is CompositeNode)
        {
            var compositeTail = (CompositeNode)tail;
            // head too small, returns (head + tail/2) + (tail/2)
            if (compositeTail.head.Length > compositeTail.tail.Length)
            {
                // Rotates to concatenate with smaller part.
                tail = compositeTail.RotateRight();
            }

            head = ConcatNodes(head, compositeTail.head);
            tail = compositeTail.tail;
        }
        else
        {
            if ((tail.Length << 1) < head.Length && head is CompositeNode)
            {
                var compositeHead = (CompositeNode)head;
                // tail too small, returns (head/2) + (head/2 concat tail)
                if (compositeHead.tail.Length > compositeHead.head.Length)
                {
                    // Rotates to concatenate with smaller part.
                    head = compositeHead.RotateLeft();
                }

                tail = ConcatNodes(compositeHead.tail, tail);
                head = compositeHead.head;
            }
        }

        return new CompositeNode(head, tail);
    }

    private InnerLeaf FindLeaf(int index, int offset)
    {
        Node node = _root;
        while (true)
        {
            if (index >= node.Length)
                throw new IndexOutOfRangeException();

            if (node is LeafNode leafNode)
            {
                return new InnerLeaf(leafNode, offset);
            }

            var composite = (CompositeNode)node;
            if (index < composite.head.Length)
            {
                node = composite.head;
            }
            else
            {
                offset += composite.head.Length;
                index -= composite.head.Length;
                node = composite.tail;
            }
        }
    }

    #endregion
}

internal sealed class InnerLeaf
{
    internal readonly LeafNode LeafNode;
    internal readonly int Offset;

    public InnerLeaf(LeafNode leafNode, int offset)
    {
        LeafNode = leafNode;
        Offset = offset;
    }
}