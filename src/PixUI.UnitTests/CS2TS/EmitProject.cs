using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PixUI.CS2TS;

namespace PixUI.UnitTests.CS2TS
{
    public class EmitProject
    {
        private const string TSAttibutesDllPath =
            "../../../../PixUI.TSAttributes/bin/Debug/netstandard2.1/PixUI.TSAttributes.dll";

        private const string PixUIDllPath = "../../../../PixUI/bin/DebugWeb/netstandard2.1/PixUI.dll";

        [Test]
        public async Task EmitPixUI()
        {
            const string prjPath = "../../../../PixUI/";
            const string outPath = "../../../../../Dev.Web/src/PixUI/Generated/";

            var translator = new Translator("PixUI", new[] { TSAttibutesDllPath });
            var workspace = translator.AddSourceFiles(prjPath);

            // translator.DumpErrors();

            var internalExports = new StringBuilder(1024);

            var sw = Stopwatch.StartNew();
            foreach (var document in workspace.CurrentSolution.Projects.Single().Documents)
            {
                var path = string.Join('/', document.Folders);
                if (path.StartsWith("Platform/Native") ||
                    path.StartsWith("Platform/Web")) continue;

                // Console.WriteLine($"{path}/{document.Name}");
                var emitter = await Emitter.MakeAsync(translator, document);
                emitter.Emit();

                var fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outPath, path));
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var filePath = System.IO.Path.Combine(fullPath,
                    System.IO.Path.GetFileNameWithoutExtension(document.Name) + ".ts");
                var typeScriptCode = emitter.GetTypeScriptCode();
                if (string.IsNullOrEmpty(typeScriptCode))
                    continue;
                await File.WriteAllTextAsync(filePath, typeScriptCode);

                // add to exports
                internalExports.Append("export * from './");
                if (!string.IsNullOrEmpty(path))
                {
                    internalExports.Append(path);
                    internalExports.Append('/');
                }

                internalExports.Append(System.IO.Path.GetFileNameWithoutExtension(document.Name));
                internalExports.Append('\'');
                internalExports.AppendLine();
            }

            sw.Stop();
            Console.WriteLine($"耗时: {sw.ElapsedMilliseconds} ms");

            //save exports file TODO:需要按依赖关系排序输出
            // await File.WriteAllTextAsync(Path.Combine(outPath, "Internal.ts"),
            //     internalExports.ToString());
        }

        [Test]
        public async Task EmitCodeEditor()
        {
            const string prjPath = "../../../../PixUI.CodeEditor/";
            const string outPath = "../../../../../Dev.Web/src/CodeEditor/Generated/";

            var translator = new Translator("CodeEditor", new[] { TSAttibutesDllPath, PixUIDllPath });
            var workspace = translator.AddSourceFiles(prjPath);

            Assert.True(translator.DumpErrors() == 0);

            var sw = Stopwatch.StartNew();
            foreach (var document in workspace.CurrentSolution.Projects.Single().Documents)
            {
                var path = string.Join('/', document.Folders);
                if (path.StartsWith("TreeSitter/Native") ||
                    path.StartsWith("TreeSitter/Web") ||
                    path.StartsWith("libs/") ||
                    path.StartsWith("Properties")) continue;

                // Console.WriteLine($"{path}/{document.Name}");
                var emitter = await Emitter.MakeAsync(translator, document);
                emitter.Emit();

                var fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outPath, path));
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var filePath = System.IO.Path.Combine(fullPath,
                    System.IO.Path.GetFileNameWithoutExtension(document.Name) + ".ts");
                var typeScriptCode = emitter.GetTypeScriptCode();
                if (!string.IsNullOrEmpty(typeScriptCode))
                    await File.WriteAllTextAsync(filePath, typeScriptCode);
            }

            sw.Stop();
            Console.WriteLine($"耗时: {sw.ElapsedMilliseconds} ms");
        }

        [Test]
        public async Task EmitLiveChartsCore()
        {
            var sdkPath = System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location)!;
            const string tsDllPath = "../../../../PixUI.TSAttributes/bin/Debug/netstandard2.1/PixUI.TSAttributes.dll";
            const string pixUIDllPath = "../../../../PixUI/bin/DebugWeb/netstandard2.1/PixUI.dll"; //暂需要
            const string prjPath = "../../../../../../ext/LiveCharts2/src/LiveChartsCore";
            const string outPath = "../../../../../Dev.Web/src/LiveChartsCore/Generated/";

            var translator = new Translator("LiveChartsCore", new[]
            {
                tsDllPath, pixUIDllPath,
                System.IO.Path.Combine(sdkPath, "System.Text.Json.dll"),
                System.IO.Path.Combine(sdkPath, "System.ComponentModel.dll"),
                System.IO.Path.Combine(sdkPath, "System.ObjectModel.dll"),
            }, "NET5_0_OR_GREATER");
            var workspace = translator.AddSourceFiles(prjPath);

            Assert.True(translator.DumpErrors() == 0);

            var internalExports = new StringBuilder(1024);

            var sw = Stopwatch.StartNew();
            foreach (var document in workspace.CurrentSolution.Projects.Single().Documents)
            {
                var path = string.Join('/', document.Folders);
                if (path.StartsWith("Properties")) continue;

                // Console.WriteLine($"{path}/{document.Name}");
                var emitter = await Emitter.MakeAsync(translator, document);
                emitter.Emit();

                var fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outPath, path));
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var filePath = System.IO.Path.Combine(fullPath,
                    System.IO.Path.GetFileNameWithoutExtension(document.Name) + ".ts");
                var typeScriptCode = emitter.GetTypeScriptCode();
                if (string.IsNullOrEmpty(typeScriptCode))
                    continue;
                await File.WriteAllTextAsync(filePath, typeScriptCode);

                // add to exports
                internalExports.Append("export * from './");
                if (!string.IsNullOrEmpty(path))
                {
                    internalExports.Append(path);
                    internalExports.Append('/');
                }

                internalExports.Append(System.IO.Path.GetFileNameWithoutExtension(document.Name));
                internalExports.Append('\'');
                internalExports.AppendLine();
            }

            sw.Stop();
            Console.WriteLine($"耗时: {sw.ElapsedMilliseconds} ms");

            //save exports file
            // await File.WriteAllTextAsync(System.IO.Path.Combine(outPath, "Internal.ts"),
            //     internalExports.ToString());
        }

        [Test]
        public async Task EmitLiveCharts()
        {
            var sdkPath = System.IO.Path.GetDirectoryName(typeof(object).Assembly.Location)!;
            const string tsDllPath = "../../../../PixUI.TSAttributes/bin/Debug/netstandard2.1/PixUI.TSAttributes.dll";
            const string pixUIDllPath = "../../../../PixUI/bin/DebugWeb/netstandard2.1/PixUI.dll";
            const string chartsCoreDllPath =
                "../../../../../../ext/LiveCharts2/src/LiveChartsCore/bin/DebugWeb/netstandard2.1/LiveChartsCore.dll";
            const string prjPath = "../../../../PixUI.LiveCharts/";
            const string outPath = "../../../../../Dev.Web/src/LiveCharts/Generated/";

            var translator = new Translator("LiveCharts", new[]
            {
                tsDllPath, pixUIDllPath, chartsCoreDllPath,
                System.IO.Path.Combine(sdkPath, "System.Text.Json.dll"),
                System.IO.Path.Combine(sdkPath, "System.ComponentModel.dll"),
                System.IO.Path.Combine(sdkPath, "System.ObjectModel.dll"),
            }, "NET5_0_OR_GREATER");
            var workspace = translator.AddSourceFiles(prjPath);

            Assert.True(translator.DumpErrors() == 0);

            var internalExports = new StringBuilder(1024);

            var sw = Stopwatch.StartNew();
            foreach (var document in workspace.CurrentSolution.Projects.Single().Documents)
            {
                var path = string.Join('/', document.Folders);
                if (path.StartsWith("Properties")) continue;

                // Console.WriteLine($"{path}/{document.Name}");
                var emitter = await Emitter.MakeAsync(translator, document);
                emitter.Emit();

                var fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outPath, path));
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var filePath = System.IO.Path.Combine(fullPath,
                    System.IO.Path.GetFileNameWithoutExtension(document.Name) + ".ts");
                var typeScriptCode = emitter.GetTypeScriptCode();
                if (string.IsNullOrEmpty(typeScriptCode))
                    continue;
                await File.WriteAllTextAsync(filePath, typeScriptCode);

                // add to exports
                internalExports.Append("export * from './");
                if (!string.IsNullOrEmpty(path))
                {
                    internalExports.Append(path);
                    internalExports.Append('/');
                }

                internalExports.Append(System.IO.Path.GetFileNameWithoutExtension(document.Name));
                internalExports.Append('\'');
                internalExports.AppendLine();
            }

            sw.Stop();
            Console.WriteLine($"耗时: {sw.ElapsedMilliseconds} ms");

            //save exports file
            // await File.WriteAllTextAsync(System.IO.Path.Combine(outPath, "Internal.ts"),
            //     internalExports.ToString());
        }

        [Test]
        public async Task EmitAppStudio()
        {
            const string TSAttibutesDllPath =
                "../../../../PixUI.TSAttributes/bin/Debug/netstandard2.1/PixUI.TSAttributes.dll";
            const string pixUIDllPath = "../../../../PixUI/bin/DebugWeb/netstandard2.1/PixUI.dll";
            const string codeEditorDllPath =
                "../../../../PixUI.CodeEditor/bin/DebugWeb/netstandard2.1/PixUI.CodeEditor.dll";

            const string coreDllPath =
                "../../../../../Core/bin/Debug/netstandard2.1/AppBoxCore.dll";
            const string clientDllPath =
                "../../../../../Client/bin/Debug/netstandard2.1/AppBoxClient.dll";
            const string prjPath = "../../../../../AppStudio/";
            const string outPath = "../../../../../Dev.Web/src/AppBoxDesign/Generated/";

            var translator = new Translator("AppBoxDesign",
                new[]
                {
                    TSAttibutesDllPath, pixUIDllPath, codeEditorDllPath, coreDllPath, clientDllPath
                });
            var workspace = translator.AddSourceFiles(prjPath);

            Assert.True(translator.DumpErrors() == 0);

            var sw = Stopwatch.StartNew();
            foreach (var document in workspace.CurrentSolution.Projects.Single().Documents)
            {
                var path = string.Join('/', document.Folders);
                if (path.StartsWith("Properties")) continue;

                // Console.WriteLine($"{path}/{document.Name}");
                var emitter = await Emitter.MakeAsync(translator, document);
                emitter.Emit();

                var fullPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(outPath, path));
                if (!Directory.Exists(fullPath))
                    Directory.CreateDirectory(fullPath);
                var filePath = System.IO.Path.Combine(fullPath,
                    System.IO.Path.GetFileNameWithoutExtension(document.Name) + ".ts");
                var typeScriptCode = emitter.GetTypeScriptCode();
                if (!string.IsNullOrEmpty(typeScriptCode))
                    await File.WriteAllTextAsync(filePath, typeScriptCode);
            }

            sw.Stop();
            Console.WriteLine($"耗时: {sw.ElapsedMilliseconds} ms");
        }
    }
}