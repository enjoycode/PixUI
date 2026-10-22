using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using LiveChartsCore;
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

        References =
        [
            CreateMetadataReference(typeof(object).Assembly),
            CreateMetadataReference(Assembly.Load("netstandard")),
            CreateMetadataReference(Assembly.Load("System.Runtime")),
            CreateMetadataReference(Assembly.Load("System.Linq")),
            CreateMetadataReference(Assembly.Load("System.ObjectModel")),
            CreateMetadataReference(typeof(Console).Assembly),
            CreateMetadataReference(typeof(Color).Assembly),
            CreateMetadataReference(typeof(Widget).Assembly),
            CreateMetadataReference(typeof(DataGrid<>).Assembly),
            CreateMetadataReference(typeof(ISeries).Assembly),
            CreateMetadataReference(typeof(LineSeries<>).Assembly),
            CreateMetadataReference(typeof(PieChart).Assembly),
            CreateMetadataReference(typeof(MaterialIcons).Assembly),
            CreateMetadataReference(typeof(DiagramView).Assembly)
        ];

        var prjInfo = ProjectInfo.Create(_projectId, VersionStamp.Create(),
            "DemoProject", "DemoProject", LanguageNames.CSharp, null, null,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable), ParseOptions
        );

        var solution = Workspace.CurrentSolution.AddProject(prjInfo)
            .AddMetadataReferences(_projectId, References)
            .AddDocument(DocumentId, "Demo.cs", string.Empty)
            .AddDocument(DocumentId.CreateNewId(_projectId), "GlobalUsing.cs", GlobalUsings);
        Workspace.TryApplyChanges(solution);
    }

    public const string ResourcePrefix = "PixUI.Demo.Resources.Demos.";

    private readonly ProjectId _projectId;
    internal readonly DocumentId DocumentId;
    internal readonly RoslynWorkspace Workspace;
    internal readonly MetadataReference[] References;
    public Microsoft.CodeAnalysis.Document RoslynDocument => Workspace.CurrentSolution.GetDocument(DocumentId)!;

    internal readonly CSharpParseOptions ParseOptions =
        new CSharpParseOptions().WithLanguageVersion(LanguageVersion.Latest);

    internal readonly string GlobalUsings =
        "global using System;global using System.Threading.Tasks;global using System.Linq;global using System.Collections.Generic;global using PixUI;";

    public string WidgetTypeName { get; set; } = "HelloView";

    public readonly State<SidePadType> ActiveSidePad = SidePadType.Tree;
    internal readonly TreeController<DemoNode> TreeController = new();
    internal readonly DataGridController<Diagnostic> ProblemsController = new();
    internal readonly PreviewController PreviewController = new();

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

    internal async Task<IList<Diagnostic>> GetProblems()
    {
        var document = Workspace.CurrentSolution.GetDocument(DocumentId)!;
        var semanticModel = await document.GetSemanticModelAsync();
        var result = semanticModel!.GetDiagnostics()
            .ToList();
        ProblemsController.DataSource = result;
        return result;
    }

    internal void GotoProblem(Diagnostic diagnostic)
    {
        //TODO:
    }
}