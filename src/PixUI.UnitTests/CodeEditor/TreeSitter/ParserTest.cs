using System;
using NUnit.Framework;
using CodeEditor;

namespace PixUI.UnitTests.TreeSitter
{
    public class ParserTest
    {
        [Test]
        public void Test1()
        {
            var language = TSCSharpLanguage.Instance;
            using var parser = new TSParser { Language = language };
            using var tree = parser.Parse(@"
class Person
{
    private char name = 'a';

    public void SayHello()
    {
        System.Console.WriteLine(Name);
    }
} // */
");
            Console.WriteLine(tree.Root);
        }

        [Test]
        public void CSharpSymbolNames()
        {
            var language = TSCSharpLanguage.Instance;
            var symbolCount = language.SymbolCount;
            for (var i = 0; i < symbolCount; i++)
            {
                Console.WriteLine($"{i} {language.SymbolName((ushort)i)}");
            }
        }
    }
}