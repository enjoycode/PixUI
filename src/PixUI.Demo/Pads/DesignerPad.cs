using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using PixUI.CodeEditor;
using PixUI.CodeEditor.Roslyn;
using DocumentEventArgs = PixUI.CodeEditor.DocumentEventArgs;

namespace PixUI.Demo;

internal sealed class DesignerPad : View
{
    public DesignerPad(PlaygroundContext context)
    {
        _context = context;
        _textBuffer = new RoslynSourceText(_context.Workspace, _context.DocumentId);
        _codeEditorController = new CodeEditorController("Demo.cs", _textBuffer,
            new RoslynSyntaxParser(_textBuffer),
            new RoslynCompletionProvider(_context.Workspace),
            _context.DocumentId);

        //事件处理
        //_codeEditorController.ContextMenuBuilder = e => ContextMenuService.BuildContextMenu(designContext, e);
        _codeEditorController.Document.DocumentChanged += OnDocumentChanged;
        _delayDocChangedTask = new DelayTask(300, RunDelayTask);
        _context.TreeController.SelectionChanged += OnDemoChanged;

        Child = new Splitter()
        {
            Panel1 = new CodeEditorWidget(_codeEditorController),
            Panel2 = new PreviewPad(_context)
        };
    }

    private readonly RoslynSourceText _textBuffer;
    private readonly PlaygroundContext _context;
    private readonly CodeEditorController _codeEditorController;
    private readonly DelayTask _delayDocChangedTask;

    protected override async void OnMounted()
    {
        base.OnMounted();

        var initCode = """
                       public class HelloView : View
                       {
                           public HelloView()
                           {
                               Child = new Center() { Child = new Text("Hello World!") };
                           }
                       }
                       """;

        await _textBuffer.Open();
        _codeEditorController.Document.Open(initCode);
    }

    private void OnDocumentChanged(DocumentEventArgs e) => _delayDocChangedTask.Run();

    private async void RunDelayTask()
    {
        //TODO: Folding

        //检查语法语义错误
        try
        {
            var problems = await _context.GetProblems();
            if (problems.All(p => p.Severity != DiagnosticSeverity.Error) /*&& !_hidePreviewer.Value*/)
                _context.PreviewController.Invalidate(true /*!_hidePreviewer.Value*/);
        }
        catch (Exception e)
        {
            Notification.Error($"GetProblems error: {e.Message}");
        }
    }

    private void OnDemoChanged()
    {
        var selectedNode = _context.TreeController.FirstSelectedNode;
        if (selectedNode == null || !selectedNode.Data.IsLeaf) return;

        var demoNode = selectedNode.Data;
        _context.WidgetTypeName = $"Demo{demoNode.Name}";
        var resource = $"{PlaygroundContext.ResourcePrefix}{demoNode.Parent!.Name}.Demo{demoNode.Name}.cs";
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource)!;
        using var textReader = new StreamReader(stream);
        var sourceCode = textReader.ReadToEnd();
        _codeEditorController.Replace(sourceCode);
    }
}