using System;
using PixUI;

namespace CodeEditor
{
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

        [TSRawScript(@"
        public toString(): string {
            // @ts-ignore
            return String.fromCharCode.apply(null, this._data);
        }")]
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
            this.head = head;
            this.tail = tail;
        }

        private readonly int _count;
        internal readonly Node head;
        internal readonly Node tail;

        public override int Length => _count;

        public override char GetCharAt(int index)
        {
            var headLength = head.Length;
            return index < headLength
                ? head.GetCharAt(index)
                : tail.GetCharAt(index - headLength);
        }

        internal Node RotateRight()
        {
            // See: http://en.wikipedia.org/wiki/Tree_rotation
            if (head is CompositeNode p)
            {
                return new CompositeNode(p.head, new CompositeNode(p.tail, tail));
            }

            return this; // Head not a composite, cannot rotate.
        }

        internal Node RotateLeft()
        {
            // See: http://en.wikipedia.org/wiki/Tree_rotation
            if (tail is CompositeNode q)
            {
                return new CompositeNode(new CompositeNode(head, q.head), q.tail);
            }

            return this; // Tail not a composite, cannot rotate.
        }

#if __WEB__
        public override void CopyTo(int srcOffset, Uint16Array dest, int count)
#else
        public override void CopyTo(int srcOffset, Span<char> dest, int count)
#endif
        {
            var cesure = head.Length;
            if (srcOffset + count <= cesure)
            {
                head.CopyTo(srcOffset, dest, count);
            }
            else if (srcOffset >= cesure)
            {
                tail.CopyTo(srcOffset - cesure, dest, count);
            }
            else
            {
                // Overlaps head and tail.
                var headChunkSize = cesure - srcOffset;
                head.CopyTo(srcOffset, dest, headChunkSize);
#if __WEB__
                tail.CopyTo(0, dest.subarray(headChunkSize), count - headChunkSize);
#else
                tail.CopyTo(0, dest.Slice(headChunkSize), count - headChunkSize);
#endif
            }
        }

        public override Node SubNode(int start, int end)
        {
            var cesure = head.Length;
            if (end <= cesure)
                return head.SubNode(start, end);

            if (start >= cesure)
                return tail.SubNode(start - cesure, end - cesure);

            if ((start == 0) && (end == _count))
                return this;

            // Overlaps head and tail.
            return ImmutableText.ConcatNodes(head.SubNode(start, cesure),
                tail.SubNode(0, end - cesure));
        }
    }
}