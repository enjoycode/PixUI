using System;
using NUnit.Framework;
using CodeEditor;

namespace PixUI.UnitTests.CodeEditor;

public class DocumentTest
{
    private const string SrcCode = @"
class Person
{
    Person(string name)
    {
        IsMan = null;
        WriteLine(IsMan);
    }
}
";

    // [Test]
    // public void Test1()
    // {
    //     var doc = new Document("Test.cs", new ImmutableTextBuffer());
    //     doc.TextContent = SrcCode;
    //     doc.SyntaxParser.DumpTree();
    //
    //     // doc.Insert(20, "\n{}/*");
    //     // doc.SyntaxParser.DumpTree();
    //
    //     var rootNode = doc.SyntaxParser.RootNode!.Value;
    //     var node = rootNode.DescendantForPosition(new(3, 20));
    //     if (node.HasValue)
    //     {
    //         var thisType = node.Value.Type;
    //         var parentType = node.Value.Parent.HasValue ? node.Value.Parent.Value.Type : "null";
    //         Console.WriteLine($"Type: {thisType} Parent: {parentType}");
    //     }
    // }

    [Test]
    public void TokenizeTest()
    {
        var doc = new Document("Test.cs", new ImmutableTextBuffer(), new TreeSitterSyntaxParser());
        doc.TextContent = SrcCode;
        // doc.SyntaxParser.Tokenize(2, 3);
        var line = doc.GetLineSegment(5);
        line.DumpTokens(doc);
    }
}