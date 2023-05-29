using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeEditor
{
    public interface ICompletionItem
    {
        CompletionItemKind Kind { get; }
        string Label { get; }
        string? InsertText { get; }
        string? Detail { get; }
    }

    public interface ICompletionProvider
    {
        IEnumerable<char> TriggerCharacters { get; }

        Task<IList<ICompletionItem>?> ProvideCompletionItems(Document document,
            int offset, string? completionWord);
    }

    public readonly struct CompletionWord
    {
        public readonly int Offset;
        public readonly string Word;

        public CompletionWord(int offset, string word)
        {
            Offset = offset;
            Word = word;
        }
    }
}