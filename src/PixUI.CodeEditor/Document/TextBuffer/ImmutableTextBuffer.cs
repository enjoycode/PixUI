using System;

namespace CodeEditor
{
    public sealed class ImmutableTextBuffer : ITextBuffer
    {
        internal ImmutableTextBuffer(ImmutableText? buffer = null)
        {
            _buffer = buffer ?? ImmutableText.Empty;
        }

        private ImmutableText _buffer;
        internal ImmutableText ImmutableText => _buffer;

        public int Length => _buffer.Length;

        public char GetCharAt(int offset)
            => _buffer.GetText(offset, 1).GetCharAt(0);

        public string GetText(int offset, int length)
            => _buffer.GetString(offset, length);

        public void Insert(int offset, string text)
            => _buffer = _buffer.InsertText(offset, text);

        public void Remove(int offset, int length)
            => _buffer = _buffer.RemoveText(offset, length);

        public void Replace(int offset, int length, string text)
        {
            _buffer = _buffer.RemoveText(offset, length);
            if (!string.IsNullOrEmpty(text))
                _buffer = _buffer.InsertText(offset, text);
        }

        public void SetContent(string text) => _buffer = ImmutableText.FromString(text);

#if __WEB__
        public void CopyTo(Uint16Array dest, int offset, int count)
            => _buffer.CopyTo(offset, dest, count);
#else
        public void CopyTo(Span<char> dest, int offset, int count)
            => _buffer.CopyTo(offset, dest, count);
#endif
    }
}