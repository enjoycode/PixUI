using System;
using PixUI;

namespace CodeEditor
{
    /// <summary>
    /// 需要重新绘制的行范围[StartLine, EndLine)
    /// </summary>
    internal sealed class DirtyLines : IDirtyArea
    {
        internal DirtyLines(CodeEditorController controller)
        {
            _controller = controller;
        }

        private readonly CodeEditorController _controller;
        internal int StartLine;
        internal int EndLine;

        public void Merge(IDirtyArea? newArea)
        {
            //TODO:
        }

        public Rect GetRect()
        {
            //TODO:暂返回所有范围,注意需要包含GutterArea
            return Rect.Empty;
        }

        public bool IntersectsWith(Widget child) => throw new NotSupportedException();

        public IDirtyArea? ToChild(Widget child) => throw new NotSupportedException();
    }
}