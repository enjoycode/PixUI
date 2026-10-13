using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PixUI.CodeEditor;
using PixUI.CodeEditor.Roslyn;

namespace PixUI.Demo.Mac;

public sealed class DemoCodeEditor : View
{
    private readonly CodeEditorController _controller;
    private readonly State<string> _scrollTo = "0";

    private const string SrcCode = @"
public sealed class Person
{
    public string Name { get; set; }

    public void SayHello(int n)
    {
        if (n > 0) {
            Write(1);
        } else {
            Write(2);
        }
        System.Console.WriteLine(Name);
    }
} //中国 */
";

    private readonly RoslynWorkspace _workspace;
    private readonly ProjectId _projectId;
    private readonly DocumentId _documentId;
    private readonly RoslynSourceText _sourceText;

    public DemoCodeEditor()
    {
        _workspace = new RoslynWorkspace();
        _projectId = ProjectId.CreateNewId();
        _documentId = DocumentId.CreateNewId(_projectId);

        var prjInfo = ProjectInfo.Create(_projectId, VersionStamp.Create(),
            "TestProject", "TestProject", LanguageNames.CSharp, null, null,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithNullableContextOptions(NullableContextOptions.Enable),
            new CSharpParseOptions().WithLanguageVersion(LanguageVersion.CSharp14));

        var solution = _workspace.CurrentSolution.AddProject(prjInfo)
            .AddMetadataReference(_projectId, MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddMetadataReference(_projectId, MetadataReference.CreateFromFile(typeof(Console).Assembly.Location))
            .AddDocument(_documentId, "Demo.cs", string.Empty);
        _workspace.TryApplyChanges(solution);
        _sourceText = new RoslynSourceText(_workspace, _documentId);
        _controller = new CodeEditorController("Demo.cs",
            _sourceText,
            new RoslynSyntaxParser(_sourceText),
            new RoslynCompletionProvider(_workspace),
            _documentId);

        Child = new Column
        {
            Children =
            {
                new Row(VerticalAlignment.Middle, 20)
                {
                    Children =
                    {
                        new TextInput(_scrollTo) { Width = 100 },
                        new Button("ScrollTo")
                        {
                            OnTap = _ => _controller.ScrollTo(int.Parse(_scrollTo.Value))
                        }
                    }
                },
                new Expanded
                {
                    Child = new Container
                    {
                        Padding = EdgeInsets.All(20),
                        Child = new CodeEditorWidget(_controller),
                    }
                }
            }
        };
    }

    protected override async void OnMounted()
    {
        base.OnMounted();

        try
        {
            await _sourceText.Open();
            _controller.Document.Open(SrcCode);
        }
        catch (Exception)
        {
            Notification.Error("Can't open RoslynSourceText");
        }
    }
}