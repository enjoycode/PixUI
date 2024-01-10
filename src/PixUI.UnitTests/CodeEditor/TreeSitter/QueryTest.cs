using System;
using System.Text;
using NUnit.Framework;
using CodeEditor;

namespace PixUI.UnitTests.TreeSitter
{
    public class QueryTest
    {
        [Test]
        public void Test1()
        {
            var language = TSCSharpLanguage.Get();
            using var parser = new TSParser();
            parser.Language = language;
            using var tree = parser.Parse(@"
public class Person
{
    private int _age;
    public string Name { get; set; }
    int SayHello(int v) { return _age + v; }
}
");
            // Concatenate the query strings, keeping track of the start offset of each section.
            var sb = new StringBuilder();
            // sb.Append(PixUI.CodeEditor.ResourceLoad.LoadString("Resources.CSharpLocals.scm"));
            var locals_query_offset = sb.Length;
            sb.Append(PixUI.CodeEditor.ResourceLoad.LoadString("Resources.CSharpHighlights.scm"));
            var highlights_query_offset = sb.Length;
            
            // Construct a single query by concatenating the three query strings, but record the
            // range of pattern indices that belong to each individual string.
            using var query = language.Query(sb.ToString())!;
            var locals_pattern_index = 0;
            var highlights_pattern_index = 0;
            for (uint i = 0; i < query.PatternCount; i++)
            {
                var pattern_offset = query.StartByteForPattern(i);
                if (pattern_offset < highlights_query_offset)
                {
                    if (pattern_offset < highlights_query_offset)
                        highlights_pattern_index += 1;
                    if (pattern_offset < locals_query_offset)
                        locals_pattern_index += 1;
                }
            }
            
            var captures = query!.Captures(tree.Root,
                new TSPoint(4, 0), new TSPoint(5, 0));
            foreach (var capture in captures)
            {
                var captureName = query.CaptureNameForId(capture.index);
                var node = TSSyntaxNode.Create(capture.node)!;
                Console.WriteLine($"{captureName} {node.Type} {node.StartPosition}-{node.EndPosition}");
            }
        }
    }
}