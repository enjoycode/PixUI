using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using PixUI.CS2TS;

namespace PixUI.UnitTests.CS2TS
{
    public class EmitterTest
    {
        private static async Task<Translator> Run(string srcFileName, bool withAttributeFiles = false)
        {
            const string attrFilesPath = "../../../../PixUI.TSAttributes/Attributes/";
            const string testFilesPath = "../../../../PixUI.UnitTests/Resources/TestCode/";
            //var content = Resources.LoadString($"Resources.TestCode.{srcFileName}");

            var translator = new Translator("TestProject");

            if (withAttributeFiles)
            {
                foreach (var fullPath in Directory.EnumerateFiles(attrFilesPath, "*.cs"))
                {
                    translator.AddTestFile(fullPath);
                }
            }

            //加入必须的通用定义
            translator.AddTestFile(System.IO.Path.Combine(testFilesPath, "Common.cs"));
            //加入目标测试文件
            var document = translator.AddTestFile(System.IO.Path.Combine(testFilesPath, srcFileName));

            Assert.True(translator.DumpErrors() == 0);

            var sw = Stopwatch.StartNew();
            var emitter = await Emitter.MakeAsync(translator, document);
            emitter.Emit();
            sw.Stop();
            Console.WriteLine($"Emit耗时: {sw.ElapsedMilliseconds} ms");
            Console.Write(emitter.GetTypeScriptCode());

            return translator;
        }

        [Test]
        public async Task BugTest() => await Run("TestBug.cs", true);

        [Test]
        public async Task FirstTest() => await Run("TestClass.cs");

        [Test]
        public async Task LiteralTest() => await Run("TestLiteral.cs", true);

        [Test]
        public async Task StructTest() => await Run("TestStruct.cs");

        [Test]
        public async Task OverloadsTest() => await Run("TestOverloads.cs", true);

        [Test]
        public async Task MemberAccessTest() => await Run("TestMemberAccess.cs");

        [Test]
        public async Task ConstructorTest() => await Run("TestConstructor.cs", true);

        [Test]
        public async Task InitializerTest() => await Run("TestInitializer.cs", true);

        [Test]
        public async Task NullableTest() => await Run("TestNullable.cs");

        [Test]
        public async Task PropertyTest() => await Run("TestProperty.cs", true);

        [Test]
        public async Task InheritsTest() => await Run("TestInherits.cs");

        [Test]
        public async Task StaticTest() => await Run("TestStatic.cs");

        [Test]
        public async Task GenericTest() => await Run("TestGeneric.cs", true);

        [Test]
        public async Task IfTest() => await Run("TestIfStatement.cs");

        [Test]
        public async Task ForTest() => await Run("TestForStatement.cs");

        [Test]
        public async Task WhileTest() => await Run("TestWhileStatement.cs");

        [Test]
        public async Task LambdaTest() => await Run("TestLambda.cs");

        [Test]
        public async Task SystemTest() => await Run("TestSystem.cs", true);

        [Test]
        public async Task InterceptorTest() => await Run("TestInterceptor.cs", true);

        [Test]
        public async Task PatternTest() => await Run("TestPattern.cs", true);

        [Test]
        public async Task SwitchExpressionTest() => await Run("TestSwitchExpression.cs");

        [Test]
        public async Task ConversionOperatorTest() => await Run("TestConversionOperator.cs", true);

        [Test]
        public async Task OverrideOperatorTest() => await Run("TestOverrideOperator.cs");

        [Test]
        public async Task InterfaceOfTest() => await Run("TestInterfaceOf.cs", true);

        [Test]
        public async Task DisposeTest() => await Run("TestDispose.cs");

        [Test]
        public async Task MethodArgsTest() => await Run("TestMethodArgs.cs", true);

        [Test]
        public async Task EventTest() => await Run("TestEvent.cs", true);

        [Test]
        public async Task DelegateTest() => await Run("TestDelegate.cs", true);

        [Test]
        public async Task EnumTypeTest() => await Run("TestEnumType.cs", true);

        [Test]
        public async Task CollectionTest() => await Run("TestCollection.cs");

        [Test]
        public async Task IndexerTest() => await Run("TestIndexer.cs", true);

        [Test]
        public async Task ExtensitionTest() => await Run("TestExtension.cs", true);

        [Test]
        public async Task InnerClassTest() => await Run("TestInnerClass.cs", true);
        
        [Test]
        public async Task IsGenericTypeTest() => await Run("TestIsGenericType.cs", true);

        [Test]
        public async Task TaskTest() => await Run("TestTask.cs", true);
        
        [Test]
        public async Task CastTest() => await Run("TestCast.cs", true);
        
        [Test]
        public async Task YieldTest() => await Run("TestYield.cs", true);
        
        [Test]
        public async Task InnerFunctionTest() => await Run("TestInnerFunction.cs", true);
        
        [Test]
        public async Task LinqTest() => await Run("TestLinq.cs", true);
        
        [Test]
        public async Task RemoveNewLineTest() => await Run("TestRemoveNewLine.cs", true);
        
        [Test]
        public async Task DictionaryTest() => await Run("TestDictionary.cs", true);
    }
}