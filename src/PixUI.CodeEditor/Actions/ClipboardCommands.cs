using System.Threading.Tasks;
using PixUI;

namespace CodeEditor;

internal sealed class CutCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var selectedText = editor.SelectionManager.SelectedText;
        if (selectedText.Length > 0)
        {
            Clipboard.WriteText(selectedText);
            editor.Caret.Position =
                editor.SelectionManager.SelectionCollection[0].StartPosition;
            editor.SelectionManager.RemoveSelectedText();
        }
    }
}

internal sealed class CopyCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        var selectedText = editor.SelectionManager.SelectedText;
        if (selectedText.Length > 0)
            Clipboard.WriteText(selectedText);
    }
}

internal sealed class PasteCommand : IEditCommand
{
    public void Execute(TextEditor editor)
    {
        //TODO: return when readonly
        ExecInternal(editor);
    }

    private static async void ExecInternal(TextEditor editor)
    {
        var text = await Clipboard.ReadText();
        if (string.IsNullOrEmpty(text)) return;

        editor.Document.UndoStack.StartUndoGroup();
        if (editor.SelectionManager.HasSomethingSelected)
        {
            editor.Caret.Position = editor.SelectionManager.SelectionCollection[0].StartPosition;
            editor.SelectionManager.RemoveSelectedText();
        }

        editor.InsertOrReplaceString(text);
        editor.Document.UndoStack.EndUndoGroup();
    }
}