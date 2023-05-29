namespace CodeEditor
{
    internal sealed class UndoableInsert : IUndoableOperation
    {
        internal UndoableInsert(Document document, int offset, string text)
        {
            _document = document;
            _offset = offset;
            _text = text;
        }

        private readonly Document _document;
        private readonly int _offset;
        private readonly string _text;

        public void Undo()
        {
            _document.UndoStack.TextEditor?.SelectionManager.ClearSelection();

            _document.UndoStack.AcceptChanges = false;
            _document.Remove(_offset, _text.Length);
            _document.UndoStack.AcceptChanges = true;
        }

        public void Redo()
        {
            _document.UndoStack.TextEditor?.SelectionManager.ClearSelection();

            _document.UndoStack.AcceptChanges = false;
            _document.Insert(_offset, _text);
            _document.UndoStack.AcceptChanges = true;
        }
    }
}