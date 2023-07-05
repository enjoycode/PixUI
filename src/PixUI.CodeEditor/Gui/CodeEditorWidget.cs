using PixUI;

namespace CodeEditor
{
    public sealed class CodeEditorWidget : Widget, IMouseRegion, IFocusable, IScrollable
    {
        public CodeEditorWidget(CodeEditorController controller)
        {
            MouseRegion = new MouseRegion();
            FocusNode = new FocusNode();

            Controller = controller;
            Controller.AttachWidget(this);
            _decoration = new EditorDecorator(this);

            MouseRegion.PointerDown += Controller.OnPointerDown;
            MouseRegion.PointerUp += Controller.OnPointerUp;
            MouseRegion.PointerMove += Controller.OnPointerMove;
            FocusNode.FocusChanged += _OnFocusChanged;
            FocusNode.KeyDown += Controller.OnKeyDown;
            FocusNode.TextInput += Controller.OnTextInput;
        }

        internal readonly CodeEditorController Controller;
        private readonly EditorDecorator _decoration;

        public MouseRegion MouseRegion { get; }
        public FocusNode FocusNode { get; }

        #region ====IScrollable====

        public float ScrollOffsetX => Controller.TextEditor.VirtualTop.X;

        public float ScrollOffsetY => Controller.TextEditor.VirtualTop.Y;

        public bool IgnoreScrollOffsetForHitTest => false;

        public Offset OnScroll(float dx, float dy) => Controller.OnScroll(dx, dy);

        #endregion

        #region ====EventHandles====

        internal void RequestInvalidate(bool all, IDirtyArea? dirtyArea)
        {
            if (all)
                Invalidate(InvalidAction.Repaint, dirtyArea);
            else
                _decoration.Invalidate(InvalidAction.Repaint);
        }

        private void _OnFocusChanged(FocusChangedEvent e)
        {
            // Focused.Value = focused;
            if (e.IsFocused)
                Root!.Window.StartTextInput();
            else
                Root!.Window.StopTextInput();
        }

        protected override void OnMounted()
        {
            Overlay!.Show(_decoration);
            base.OnMounted();
        }

        protected override void OnUnmounted()
        {
            if (_decoration.Parent != null)
                ((Overlay)_decoration.Parent).Remove(_decoration);
            base.OnUnmounted();
        }

        #endregion

        #region ====Overrides====

        public override bool IsOpaque => true;

        public override void Layout(float availableWidth, float availableHeight)
        {
            var width = CacheAndCheckAssignWidth(availableWidth);
            var height = CacheAndCheckAssignHeight(availableHeight);
            SetSize(width, height);

            //布局各个EditorArea
            Controller.TextEditor.Layout(width, height);
        }

        public override void Paint(Canvas canvas, IDirtyArea? area = null)
        {
            var clipRect = Rect.FromLTWH(0, 0, W, H);
            canvas.Save();
            canvas.ClipRect(clipRect, ClipOp.Intersect, false);
            Controller.TextEditor.Paint(canvas, new Size(W, H), area);
            canvas.Restore();
        }

        #endregion
    }
}