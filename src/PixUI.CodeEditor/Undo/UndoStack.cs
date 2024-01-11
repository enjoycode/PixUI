using System;
using System.Collections.Generic;

namespace CodeEditor;

internal sealed class UndoStack
{
    private readonly Stack<IUndoableOperation> _undostack = new Stack<IUndoableOperation>();
    private readonly Stack<IUndoableOperation> _redostack = new Stack<IUndoableOperation>();

    internal TextEditor? TextEditor;

    private int _undoGroupDepth;
    private int _actionCountInUndoGroup;

    /// <summary>
    /// Gets/Sets if changes to the document are protocolled by the undo stack.
    /// Used internally to disable the undo stack temporarily while undoing an action.
    /// </summary>
    internal bool AcceptChanges = true;

    /// <summary>
    /// Gets if there are actions on the undo stack.
    /// </summary>
    public bool CanUndo => _undostack.Count > 0;

    /// <summary>
    /// Gets if there are actions on the redo stack.
    /// </summary>
    public bool CanRedo => _redostack.Count > 0;

    /// <summary>
    /// Gets the number of actions on the undo stack.
    /// </summary>
    public int UndoItemCount => _undostack.Count;

    /// <summary>
    /// Gets the number of actions on the redo stack.
    /// </summary>
    public int RedoItemCount => _redostack.Count;

    public void StartUndoGroup()
    {
        if (_undoGroupDepth == 0)
        {
            _actionCountInUndoGroup = 0;
        }

        _undoGroupDepth++;
        //Util.LoggingService.Debug("Open undo group (new depth=" + undoGroupDepth + ")");
    }

    public void EndUndoGroup()
    {
        if (_undoGroupDepth == 0)
            throw new InvalidOperationException("There are no open undo groups");
        _undoGroupDepth--;
        //Util.LoggingService.Debug("Close undo group (new depth=" + undoGroupDepth + ")");
        if (_undoGroupDepth == 0 && _actionCountInUndoGroup > 1)
        {
            var op = new UndoQueue(_undostack, _actionCountInUndoGroup);
            _undostack.Push(op);
            //OperationPushed?.Invoke(this, new OperationEventArgs(op));
        }
    }

    public void AssertNoUndoGroupOpen()
    {
        if (_undoGroupDepth != 0)
        {
            _undoGroupDepth = 0;
            throw new InvalidOperationException("No undo group should be open at this point");
        }
    }

    /// <summary>
    /// Call this method to undo the last operation on the stack
    /// </summary>
    public void Undo()
    {
        AssertNoUndoGroupOpen();
        if (_undostack.Count > 0)
        {
            var uedit = _undostack.Pop();
            _redostack.Push(uedit);
            uedit.Undo();
            // OnActionUndone();
        }
    }

    /// <summary>
    /// Call this method to redo the last undone operation
    /// </summary>
    public void Redo()
    {
        AssertNoUndoGroupOpen();
        if (_redostack.Count > 0)
        {
            var uedit = _redostack.Pop();
            _undostack.Push(uedit);
            uedit.Redo();
            // OnActionRedone();
        }
    }

    /// <summary>
    /// Call this method to push an UndoableOperation on the undostack, the redostack
    /// will be cleared, if you use this method.
    /// </summary>
    public void Push(IUndoableOperation operation)
    {
        if (operation == null)
            throw new ArgumentNullException(nameof(operation));

        if (AcceptChanges)
        {
            StartUndoGroup();
            _undostack.Push(operation);
            _actionCountInUndoGroup++;
            if (TextEditor != null)
            {
                _undostack.Push(new UndoableSetCaretPosition(this, TextEditor.Caret.Position));
                _actionCountInUndoGroup++;
            }

            EndUndoGroup();
            ClearRedoStack();
        }
    }

    /// <summary>
    /// Call this method, if you want to clear the redo stack
    /// </summary>
    public void ClearRedoStack()
    {
        _redostack.Clear();
    }

    /// <summary>
    /// Clears both the undo and redo stack.
    /// </summary>
    public void ClearAll()
    {
        AssertNoUndoGroupOpen();
        _undostack.Clear();
        _redostack.Clear();
        _actionCountInUndoGroup = 0;
    }
}