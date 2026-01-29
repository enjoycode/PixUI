namespace CodeEditor;

internal abstract class Node
{
    public abstract int Length { get; }

    public abstract char GetCharAt(int index);

#if __WEB__
        public abstract void CopyTo(int srcOffset, Uint16Array dest, int count);
#else
    public abstract void CopyTo(int srcOffset, Span<char> dest, int count);
#endif

    public abstract Node SubNode(int start, int end);

    public override string ToString()
    {
#if __WEB__
            throw new NotImplementedException();
#else
        var len = Length;
        var data = new char[len];
        CopyTo(0, data.AsSpan(), len);
        return new string(data);
#endif
    }

    public Node SubSequence(int start, int end)
    {
        return SubNode(start, end);
    }
}

internal sealed class LeafNode : Node
{
#if __WEB__
        public LeafNode(Uint16Array data)
        {
            _data = data;
        }

        private readonly Uint16Array _data;

        public override int Length => _data.length;
#else
    public LeafNode(char[] data)
    {
        _data = data;
    }

    private readonly char[] _data;

    public override int Length => _data.Length;
#endif

    public override char GetCharAt(int index) => _data[index];

#if __WEB__
        public override void CopyTo(int srcOffset, Uint16Array dest, int count)
        {
            var src = _data.subarray(srcOffset, srcOffset + count);
            dest.set(src);
        }
#else
    public override void CopyTo(int srcOffset, Span<char> dest, int count)
    {
        var src = _data.AsSpan(srcOffset, count);
        src.CopyTo(dest);
    }
#endif

    public override Node SubNode(int start, int end)
    {
        if (start == 0 && end == Length)
            return this;

#if __WEB__
            var subArray = new Uint16Array(end - start);
            subArray.set(_data.subarray(start, end));
            return new LeafNode(subArray);
#else
        var subArray = new char[end - start];
        Array.Copy(_data, start, subArray, 0, subArray.Length);
        return new LeafNode(subArray);
#endif
    }

    public override string ToString()
    {
#if __WEB__
            throw new Exception();
#else
        return new string(_data);
#endif
    }
}

internal sealed class CompositeNode : Node
{
    public CompositeNode(Node head, Node tail)
    {
        _count = head.Length + tail.Length;
        this.Head = head;
        this.Tail = tail;
    }

    private readonly int _count;
    internal readonly Node Head;
    internal readonly Node Tail;

    public override int Length => _count;

    public override char GetCharAt(int index)
    {
        var headLength = Head.Length;
        return index < headLength
            ? Head.GetCharAt(index)
            : Tail.GetCharAt(index - headLength);
    }

    internal Node RotateRight()
    {
        // See: http://en.wikipedia.org/wiki/Tree_rotation
        if (Head is CompositeNode p)
        {
            return new CompositeNode(p.Head, new CompositeNode(p.Tail, Tail));
        }

        return this; // Head not a composite, cannot rotate.
    }

    internal Node RotateLeft()
    {
        // See: http://en.wikipedia.org/wiki/Tree_rotation
        if (Tail is CompositeNode q)
        {
            return new CompositeNode(new CompositeNode(Head, q.Head), q.Tail);
        }

        return this; // Tail not a composite, cannot rotate.
    }

#if __WEB__
    public override void CopyTo(int srcOffset, Uint16Array dest, int count)
#else
    public override void CopyTo(int srcOffset, Span<char> dest, int count)
#endif
    {
        var cesure = Head.Length;
        if (srcOffset + count <= cesure)
        {
            Head.CopyTo(srcOffset, dest, count);
        }
        else if (srcOffset >= cesure)
        {
            Tail.CopyTo(srcOffset - cesure, dest, count);
        }
        else
        {
            // Overlaps head and tail.
            var headChunkSize = cesure - srcOffset;
            Head.CopyTo(srcOffset, dest, headChunkSize);
#if __WEB__
            Tail.CopyTo(0, dest.subarray(headChunkSize), count - headChunkSize);
#else
            Tail.CopyTo(0, dest.Slice(headChunkSize), count - headChunkSize);
#endif
        }
    }

    public override Node SubNode(int start, int end)
    {
        var cesure = Head.Length;
        if (end <= cesure)
            return Head.SubNode(start, end);

        if (start >= cesure)
            return Tail.SubNode(start - cesure, end - cesure);

        if ((start == 0) && (end == _count))
            return this;

        // Overlaps head and tail.
        return ImmutableText.ConcatNodes(Head.SubNode(start, cesure),
            Tail.SubNode(0, end - cesure));
    }
}