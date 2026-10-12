using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using PixUI.CodeEditor.Roslyn;

namespace PixUI.UnitTests.CodeEditor;

public class RoslynTest
{
    [Test]
    public async Task Test1()
    {
        var workspace = new RoslynWorkspace();
        var prjId = ProjectId.CreateNewId();
        var docId = DocumentId.CreateNewId(prjId);
        
        var prjInfo = ProjectInfo.Create(prjId, VersionStamp.Create(),
            "TestProject", "TestProject", LanguageNames.CSharp, null, null,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable),
            new CSharpParseOptions().WithLanguageVersion(LanguageVersion.CSharp14));

        var solution = workspace.CurrentSolution.AddProject(prjInfo)
            .AddMetadataReference(prjId, MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddDocument(docId, "test.cs", string.Empty);
        workspace.TryApplyChanges(solution);
        
        
        var source = new RoslynSourceText(workspace, docId);
        await source.Open();
        var doc = new PixUI.CodeEditor.Document("test.cs", source, new RoslynSyntaxParser(source));
        doc.Open("class TestClass {}");
        doc.Insert(15, "2");
        //workspace.CurrentSolution.AddDocument()
    }
}