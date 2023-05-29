using System;
using System.Collections.Generic;

namespace CodeEditor
{
    internal sealed class UndoQueue : IUndoableOperation
    {
        internal UndoQueue(Stack<IUndoableOperation> stack, int numops)
        {
            numops = Math.Min(numops, stack.Count);
            _undoList = new IUndoableOperation[numops];
            for (var i = 0; i < numops; ++i)
            {
                _undoList[i] = stack.Pop();
            }
        }

        private readonly IUndoableOperation[] _undoList;

        public void Undo()
        {
            for (var i = 0; i < _undoList.Length; ++i)
            {
                _undoList[i].Undo();
            }
        }

        public void Redo()
        {
            for (var i = _undoList.Length - 1; i >= 0; --i)
            {
                _undoList[i].Redo();
            }
        }
    }
}