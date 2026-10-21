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
        //_codeEditorController.ContextMenuBuilder = e => ContextMenuService.BuildContextMenu(designContext, e);
        _codeEditorController.Document.DocumentChanged += OnDocumentChanged;
        _delayDocChangedTask = new DelayTask(300, RunDelayTask);

        Child = new Splitter()
        {
            Panel1 = new CodeEditorWidget(_codeEditorController),
            Panel2 = new PreviewPad()
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
        //TODO:
    }
}