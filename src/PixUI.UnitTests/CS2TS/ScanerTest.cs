using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using PixUI.CS2TS;

namespace PixUI.UnitTests.CS2TS
{
    
    public class ScanerTest
    {
        [Test]
        public async Task Test()
        {
            const string attrFilesPath = "../../../../PixUI.TSAttributes/";
            const string testFilesPath = "../../../../PixUI.UnitTests/Resources/TestCode/";
            const string srcFileName = "TestOverloads.cs";

            var translator = new Translator("TestProject");
            foreach (var fullPath in Directory.EnumerateFiles(attrFilesPath, "*.cs"))
            {
                translator.AddTestFile(fullPath);
            }

            //加入必须的通用定义
            translator.AddTestFile(System.IO.Path.Combine(testFilesPath, "Common.cs"));
            //加入目标测试文件
            var document = translator.AddTestFile(System.IO.Path.Combine(testFilesPath, srcFileName));

            Assert.True(translator.DumpErrors() == 0);

            var sw = Stopwatch.StartNew();
            var scaner = new Scaner();
            var rootNode = await document.GetSyntaxRootAsync();
            scaner.Visit(rootNode);
            scaner.BuildTypeOverloads();
            sw.Stop();
            Console.WriteLine($"Scan耗时: {sw.ElapsedMilliseconds} ms");
        }
    }
}