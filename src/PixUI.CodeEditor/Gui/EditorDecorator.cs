using PixUI;

namespace CodeEditor
{
    /// <summary>
    /// 专用于在Overlay绘制光标、高亮及选择区等
    /// </summary>
    internal sealed class EditorDecorator : Widget
    {
        internal EditorDecorator(CodeEditorWidget codeEditor)
        {
            _codeEditor = codeEditor;
        }

        private readonly CodeEditorWidget _codeEditor;

        public override void Layout(float availableWidth, float availableHeight)
        {
            SetSize(0, 0);
        }

        public override void Paint(Canvas canvas, IDirtyArea? area = null)
        {
            var textEditor = _codeEditor.Controller.TextEditor;
            
            //TODO:解决转场动画过程中超出绘制范围
            //方案1: 待实现Widget.VisibleArea属性(向上判断是否Clip直至根)后替换,但无法解决不规则的clip
            //方案2: 绘制CodeEditor时想办法获取canvas的clip并缓存，在这里重新clip相同的区域
            //方案3: 简单判断是否动画过程中，是则干脆不绘制，或者不用Overlay绘制

            canvas.Save();

            var pt2Win = _codeEditor.LocalToWindow(0, 0);
            canvas.Translate(pt2Win.X, pt2Win.Y);
            //clip to visible area
            canvas.ClipRect(textEditor.TextView.Bounds, ClipOp.Intersect, false);

            // paint caret and highlight line
            textEditor.Caret.Paint(canvas);

            // paint selection
            var textView = textEditor.TextView;
            var paint = PaintUtils.Shared(textEditor.Theme.SelectionColor);
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
                               textEditor.Document.GetVisibleLine(i) * textView.FontHeight -
                               textEditor.VirtualTop.Y;
                    canvas.DrawRect(Rect.FromLTWH(startXPos + textView.Bounds.Left, yPos,
                        endXPos - startXPos, textView.FontHeight), paint);
                }
            }
            
            canvas.Restore();
        }
    }
}