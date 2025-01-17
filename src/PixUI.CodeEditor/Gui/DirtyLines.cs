using System;
using PixUI;

namespace CodeEditor;

/// <summary>
/// 需要重新绘制的行范围[StartLine, EndLine)
/// </summary>
public sealed class DirtyLines : IDirtyArea
{
    public DirtyLines(CodeEditorController controller)
    {
        _controller = controller;
    }

    private readonly CodeEditorController _controller;
    public int StartLine;
    public int EndLine;

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

    public void ApplyClip(Canvas canvas)
    {
        // do nothing now
    }
}