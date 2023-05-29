namespace CodeEditor
{
    internal sealed class UndoableReplace : IUndoableOperation
    {
        internal UndoableReplace(Document document, int offset, string origText, string text)
        {
            _document = document;
            _offset = offset;
            _text = text;
            _origText = origText;
        }

        private readonly Document _document;
        private readonly int _offset;
        private readonly string _text;
        private readonly string _origText;

        public void Undo()
        {
            _document.UndoStack.TextEditor?.SelectionManager.ClearSelection();

            _document.UndoStack.AcceptChanges = false;
            _document.Replace(_offset, _text.Length, _origText);
            _document.UndoStack.AcceptChanges = true;
        }

        public void Redo()
        {
            _document.UndoStack.TextEditor?.SelectionManager.ClearSelection();

            _document.UndoStack.AcceptChanges = false;
            _document.Replace(_offset, _origText.Length, _text);
            _document.UndoStack.AcceptChanges = true;
        }
    }
}