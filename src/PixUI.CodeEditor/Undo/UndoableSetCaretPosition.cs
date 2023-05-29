namespace CodeEditor
{
    internal sealed class UndoableSetCaretPosition : IUndoableOperation
    {
        internal UndoableSetCaretPosition(UndoStack stack, TextLocation pos)
        {
            _stack = stack;
            _pos = pos;
        }

        private readonly UndoStack _stack;
        private readonly TextLocation _pos;
        private TextLocation _redoPos;

        public void Undo()
        {
            _redoPos = _stack.TextEditor!.Caret.Position;
            _stack.TextEditor!.Caret.Position = _pos;
            _stack.TextEditor!.SelectionManager.ClearSelection();
        }

        public void Redo()
        {
            _stack.TextEditor!.Caret.Position = _redoPos;
            _stack.TextEditor!.SelectionManager.ClearSelection();
        }
    }
}