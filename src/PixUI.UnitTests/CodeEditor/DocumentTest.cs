using NUnit.Framework;
using CodeEditor;

namespace PixUI.UnitTests.CodeEditor
{
    public class DocumentTest
    {
        private const string SrcCode = @"
public sealed class Person
{
    public string Name {get; set;}
    public void SayHello { Console.WriteLine(Name); }
}// */
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
            //doc.SyntaxParser.Tokenize(3, 4);
            var line = doc.GetLineSegment(4);
            line.DumpTokens(doc);
        }
    }
}