using PixUI;

namespace CodeEditor;

/// <summary>
/// 专用于在Overlay绘制光标、高亮及选择区等
/// </summary>
internal sealed class EditorDecorator : FlowDecorator<CodeEditorWidget>
{
    internal EditorDecorator(CodeEditorWidget codeEditor) : base(codeEditor, false)
    {
        codeEditor.Controller.TextEditor.Caret.PositionChanged += OnCaretPosChanged;
    }

    private void OnCaretPosChanged()
    {
        var textEditor = Target.Controller.TextEditor;
        // var rootNode = Target.Controller.Document.SyntaxParser.RootNode;
        // if (!rootNode.HasValue) return;
        //
        // var caretPos = textEditor.Caret.Position;
        // var startPoint = new TSPoint(caretPos.Line, caretPos.Column * SyntaxParser.ParserEncoding);
        // // var endPoint = new TSPoint(caretPos.Line, (caretPos.Column + 1) * SyntaxParser.ParserEncoding);
        // var nodeAtCaret = rootNode.Value.DescendantForPosition(startPoint /*, endPoint*/);
        // if (!nodeAtCaret.HasValue) return;

        //Log.Debug($"Type: {nodeAtCaret.Value.Type} IsNamed: {nodeAtCaret.Value.IsNamed()}");
    }

    protected override void PaintCore(Canvas canvas)
    {
        var textEditor = Target.Controller.TextEditor;
        //clip to visible area
        canvas.ClipRect(textEditor.TextView.Bounds, ClipOp.Intersect, false);

        // paint caret and highlight line
        textEditor.Caret.Paint(canvas);

        // paint selection
        var textView = textEditor.TextView;
        var paint = PixUI.Paint.Shared(textEditor.Theme.SelectionColor);
        foreach (var selection in textEditor.SelectionManager.SelectionCollection)
        {
            var startLine = selection.StartPosition.Line;
            var endLine = selection.EndPosition.Line;

            for (var i = startLine; i <= endLine; i++)
            {
                if (!textEditor.Document.FoldingManager.IsLineVisible(i))
                    continue;

                var startXPos = 0f;
                var endXPos = 0f;
                if (i == startLine)
                {
                    startXPos = textView.GetDrawingXPos(i, selection.StartPosition.Column);
                    if (i == endLine)
                        endXPos = textView.GetDrawingXPos(i, selection.EndPosition.Column);
                    else
                        endXPos = textView.Bounds.Width;
                }
                else if (i == endLine)
                {
                    endXPos = textView.GetDrawingXPos(i, selection.EndPosition.Column);
                }
                else
                {
                    endXPos = textView.Bounds.Width;
                }

                var yPos = textView.Bounds.Top +
                    textEditor.Document.GetVisibleLine(i) * textView.FontHeight - textEditor.VirtualTop.Y;
                canvas.DrawRect(Rect.FromLTWH(startXPos + textView.Bounds.Left, yPos,
                    endXPos - startXPos, textView.FontHeight), paint);
            }
        }
    }
}