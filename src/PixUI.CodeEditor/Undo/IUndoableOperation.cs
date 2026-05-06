namespace PixUI.CodeEditor;

public interface IUndoableOperation
{
    void Undo();

    void Redo();
}