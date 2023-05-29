using System;
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
            using var parser = new TSParser { Language = language };
            using var tree = parser.Parse(@"
public class Person
{
    public string Name {get; set;}
}
");
            // using var query = language.Query(Language.GetCSharpHighlights());
            // var captures = query.Captures(tree.Root,
            //     new global::TreeSitter.Point() { Row = 0, Column = 2 },
            //     new global::TreeSitter.Point() { Row = 1, Column = 38 });
            // foreach (var capture in captures)
            // {
            //     var captureName = query.CaptureNameForId(capture.index);
            //     Console.WriteLine(captureName);
            //     // var node = SyntaxNode.Create(capture.node);
            //     // Console.WriteLine($"{node.KindId} {node.Kind} {node.StartPosition} {node.EndPosition}");
            // }
        }
    }
}