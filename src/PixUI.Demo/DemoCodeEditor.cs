using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeEditor;

namespace PixUI.Demo.Mac
{
    public sealed class DemoCodeEditor : View
    {
        private readonly CodeEditorController _controller;

        private const string SrcCode = @"
public sealed class Person
{
    public string Name { get; set; }

    public void SayHello()
    {
        System.Console.WriteLine(Name);
    }
} //中国 */
";

        public DemoCodeEditor()
        {
            _controller = new CodeEditorController("Demo.cs", SrcCode, new MockCompletionProvider());

            Child = new Container()
            {
                Padding = EdgeInsets.All(20),
                Child = new CodeEditorWidget(_controller),
            };
        }
    }

    internal sealed class MockCompletionProvider : ICompletionProvider
    {
        public IEnumerable<char> TriggerCharacters => new char[] { '.' };

        public Task<IList<ICompletionItem>?> ProvideCompletionItems(Document document,
            int offset, string? completionWord)
        {
            var list = new List<ICompletionItem>
            {
                new CompletionItem(CompletionItemKind.Class, "Person"),
                new CompletionItem(CompletionItemKind.Method, "SayHello"),
                new CompletionItem(CompletionItemKind.Method, "SayHi"),
                new CompletionItem(CompletionItemKind.Keyword, "where"),
                new CompletionItem(CompletionItemKind.Interface, "IEquatable")
            };
            if (completionWord != null)
                list = list.Where(t => t.Label.StartsWith(completionWord!)).ToList();
            return Task.FromResult<IList<ICompletionItem>?>(list);
        }

        private sealed class CompletionItem : ICompletionItem
        {
            public CompletionItemKind Kind { get; }
            public string Label { get; }
            public string? InsertText { get; }
            public string? Detail { get; }

            public CompletionItem(CompletionItemKind kind, string label, string? insertText = null,
                string? detail = null)
            {
                Kind = kind;
                Label = label;
                InsertText = insertText;
                Detail = detail;
            }
        }
    }
}