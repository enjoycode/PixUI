using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PixUI.CS2TS
{
    /// <summary>
    /// 管理待翻译的工程
    /// </summary>
    public sealed class Translator
    {
        private readonly Workspace _workspace;
        private readonly ProjectId _projectId;
        private readonly HashSet<string> _allTypes = new(); //所有类型的名称，用于判断重复

        internal readonly bool IsPixUIProject;

        private static readonly MetadataReference[] Refs;

        static Translator()
        {
            var path = Path.GetDirectoryName(typeof(object).Assembly.Location)!;
            Refs = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Path.Combine(path, "netstandard.dll")),
                MetadataReference.CreateFromFile(Path.Combine(path, "System.Collections.dll")),
                MetadataReference.CreateFromFile(Path.Combine(path, "System.Runtime.dll")),
                MetadataReference.CreateFromFile(Path.Combine(path, "System.Linq.dll")),
                MetadataReference.CreateFromFile(Path.Combine(path, "System.Console.dll")),
                // MetadataReference.CreateFromFile(Path.Combine(path, "System.Private.CoreLib.dll")),
            };

            //注册自定义拦截器
            TSInterceptorFactory.RegisterCustomInterceptor("CanvasKitCtor", new CanvasKitCtorInterceptor());
        }

        public Translator(string projectName, string[]? refDllPaths = null, params string[]? preprocessorSymbols)
        {
            _workspace = new AdhocWorkspace();
            IsPixUIProject = projectName == "PixUI";
            _projectId = ProjectId.CreateNewId();
            var preprocessors = new List<string> { "__WEB__" };
            if (preprocessorSymbols != null)
                preprocessors.AddRange(preprocessorSymbols);
            CreateProject(_projectId, projectName, preprocessors, refDllPaths);
        }

        /// <summary>
        /// 专用于设计时转换单个视图模型
        /// </summary>
        public Translator(Workspace workspace, ProjectId projectId)
        {
            _workspace = workspace;
            _projectId = projectId;
        }

        private void CreateProject(ProjectId id, string name, IEnumerable<string> preprocessorSymbols,
            string[]? refDllPaths = null)
        {
            var complieOpts = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable);
            var parseOpts = new CSharpParseOptions()
                .WithLanguageVersion(LanguageVersion.CSharp10)
                .WithPreprocessorSymbols(preprocessorSymbols);

            var metaRefs = Refs;
            if (refDllPaths != null)
            {
                foreach (var dllPath in refDllPaths)
                {
                    var metaRef = MetadataReference.CreateFromFile(dllPath);
                    metaRefs = metaRefs.Concat(new[] { metaRef }).ToArray();
                }
            }

            var projectInfo = ProjectInfo.Create(id, VersionStamp.Default,
                    name, name, LanguageNames.CSharp)
                .WithCompilationOptions(complieOpts)
                .WithParseOptions(parseOpts)
                .WithMetadataReferences(metaRefs);
            ((AdhocWorkspace)_workspace).AddProject(projectInfo);
        }

        /// <summary>
        /// 添加需要转换的工程的所有源文件
        /// </summary>
        internal Workspace AddSourceFiles(string prjPath)
        {
            foreach (var fullPath in Directory.EnumerateFiles(prjPath, "*.cs",
                         SearchOption.AllDirectories))
            {
                var filePath = Path.GetRelativePath(prjPath, fullPath);
                if (filePath.StartsWith("obj/") || filePath.StartsWith("bin/")) continue;
                if (filePath.EndsWith("AssemblyInfo.cs")) continue;

                var fileName = Path.GetFileName(filePath);
                var folders = Path.GetDirectoryName(filePath)?.Split(Path.PathSeparator);

                var docInfo = DocumentInfo.Create(DocumentId.CreateNewId(_projectId), fileName,
                    folders, SourceCodeKind.Regular,
                    new FileTextLoader(Path.GetFullPath(fullPath), Encoding.Default));
                ((AdhocWorkspace)_workspace).AddDocument(docInfo);
            }

            return _workspace;
        }

        /// <summary>
        /// Only for test
        /// </summary>
        internal Document AddTestFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var fullPath = Path.GetFullPath(filePath);
            var docInfo = DocumentInfo.Create(DocumentId.CreateNewId(_projectId), fileName,
                new[] { "Src" }, SourceCodeKind.Regular,
                new FileTextLoader(fullPath, Encoding.Default));
            return ((AdhocWorkspace)_workspace).AddDocument(docInfo);
        }

        internal int DumpErrors()
        {
            var project = _workspace.CurrentSolution.Projects.First();
            var cu = project.GetCompilationAsync().Result;
            var errors = cu!.GetDiagnostics();
            foreach (var error in errors)
            {
                if (error.Severity == DiagnosticSeverity.Error)
                    Console.WriteLine(error);
            }

            return errors.Count(err => err.Severity == DiagnosticSeverity.Error);
        }

        /// <summary>
        /// 加入处理过的类型，如果包名不一致或名称已存在则抛异常
        /// </summary>
        internal void AddType(string rootNamespace, string typeName)
        {
            //TODO:判断包名是否一致，判断类型名称是否存在
        }
    }
}