using NUnit.Framework;
using CodeEditor;

namespace PixUI.UnitTests.CodeEditor
{
    public class DocumentTest
    {
        private const string SrcCode = @"
public class Person
{
    Action action = () => SayHello();
    public void SayHello { Console.WriteLine(Name); }
}
";
        
        [Test]
        public void Test1()
        {
            var doc = new Document("Test.cs");
            doc.TextContent = SrcCode;
            doc.SyntaxParser.DumpTree();
            
            doc.Insert(20, "\n{}/*");
            doc.SyntaxParser.DumpTree();
        }

        [Test]
        public void TokenizeTest()
        {
            var doc = new Document("Test.cs");
            doc.TextContent = SrcCode;
            // doc.SyntaxParser.Tokenize(2, 3);
            var line = doc.GetLineSegment(3);
            line.DumpTokens(doc);
        }
    }
}