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

            doc.SyntaxParser.TokenizeLine(3);
        }
    }
}