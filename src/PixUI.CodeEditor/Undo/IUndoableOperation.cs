namespace CodeEditor;

public interface IUndoableOperation
{
    void Undo();

    void Redo();
}