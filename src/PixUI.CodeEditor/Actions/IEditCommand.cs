using System;

namespace CodeEditor
{
    public interface IEditCommand
    {
        void Execute(TextEditor editor);
    }

    public sealed class CustomEditCommand : IEditCommand
    {
        public CustomEditCommand(Action<TextEditor> command)
        {
            _command = command;
        }

        private readonly Action<TextEditor> _command;

        public void Execute(TextEditor editor) => _command(editor);
    }
}