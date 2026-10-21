using System;
using System.Reflection;
using System.Reflection.Metadata;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PixUI.CodeEditor.Roslyn;
using PixUI.Diagram;
using PixUI.LiveCharts;

namespace PixUI.Demo;

internal sealed class PlaygroundContext
{
    public PlaygroundContext()
    {
        Workspace = new RoslynWorkspace();
        _projectId = ProjectId.CreateNewId();
        DocumentId = DocumentId.CreateNewId(_projectId);

        var prjInfo = ProjectInfo.Create(_projectId, VersionStamp.Create(),
            "DemoProject", "DemoProject", LanguageNames.CSharp, null, null,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable),
            new CSharpParseOptions().WithLanguageVersion(LanguageVersion.Latest));

        var solution = Workspace.CurrentSolution.AddProject(prjInfo)
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(object).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(Console).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(Widget).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(DataGrid<>).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(PieChart).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(MaterialIcons).Assembly))
            .AddMetadataReference(_projectId, CreateMetadataReference(typeof(DiagramView).Assembly))
            .AddDocument(DocumentId, "Demo.cs", string.Empty)
            .AddDocument(DocumentId.CreateNewId(_projectId), "GlobalUsing.cs",
                "global using System;global using System.Threading.Tasks;global using System.Linq;global using System.Collections.Generic;global using PixUI;"
            );
        Workspace.TryApplyChanges(solution);
    }

    private readonly ProjectId _projectId;
    internal readonly DocumentId DocumentId;
    internal readonly RoslynWorkspace Workspace;

    public readonly State<SidePadType> ActiveSidePad = SidePadType.Tree;
    internal readonly TreeController<DemoNode> TreeController = new();

    private static MetadataReference CreateMetadataReference(Assembly assembly)
    {
        unsafe
        {
            if (assembly.TryGetRawMetadata(out var blob, out var len))
            {
                var metadata = ModuleMetadata.CreateFromMetadata((IntPtr)blob, len);
                return AssemblyMetadata.Create(metadata).GetReference();
            }
        }

        throw new NotSupportedException($"Assembly '{assembly}' doesn't have a MetadataReference.");
    }
}