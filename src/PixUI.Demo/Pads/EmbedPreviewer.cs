using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace PixUI.Demo;

internal sealed class EmbedPreviewer : View
{
    public EmbedPreviewer(PlaygroundContext context)
    {
        _context = context;
        _context.PreviewController.InvalidateAction = Run;

        Child = new Container { Child = MakeLoading() }.RefBy(ref _containerRef);
    }

    private readonly PlaygroundContext _context;
    private readonly Container _containerRef = null!;
    private ViewAssemblyLoader? _assemblyLoader;
    private static Widget MakeLoading() => new Center { Child = new Text("Loading....") };

    private async void Run(bool show)
    {
        _containerRef.Child?.Dispose();
        _containerRef.Child = null;
        _assemblyLoader?.Unload();
        _assemblyLoader = null;

        if (!show) return;

        try
        {
#if DEBUG
            var ts = Stopwatch.GetTimestamp();
#endif
            var asmData = await BuildPreviewAssembly(_context);
            _assemblyLoader = new ViewAssemblyLoader();
            var asm = _assemblyLoader.LoadViewAssembly(asmData);
            var widgetType = asm.GetType(_context.WidgetTypeName);
            if (widgetType == null)
                throw new Exception($"Can't find preview widget type: {_context.WidgetTypeName}");

            //先判断是否有静态预览方法
            Widget widget;
            var previewMethod = widgetType.GetMethod("Preview", BindingFlags.Static | BindingFlags.Public);
            if (previewMethod != null)
                widget = (Widget)previewMethod.Invoke(null, null)!;
            else
                widget = (Widget)Activator.CreateInstance(widgetType)!;
            widget.DebugLabel = asm.FullName;

#if DEBUG
            Console.WriteLine(
                $"Load preview widget: {widget.GetType()}, ms={Stopwatch.GetElapsedTime(ts).TotalMilliseconds}");
#endif

            _containerRef.Child = widget;
        }
        catch (Exception e)
        {
            _containerRef.Child = new Center
            {
                Child = new Text($"Has Error:{e.Message}\n{e.StackTrace}") { MaxLines = 20 }
            };
        }

        //TODO:Task.Delay临时解决关闭再打开预览时，加入InvalidQueue被合并后的Splitter的面板不重新布局的问题
        await Task.Delay(1);
        _containerRef.Relayout();
        //Sync outline view
        _context.PreviewController.CurrentWidget = _containerRef.Child;
        _context.PreviewController.RefreshOutlineAction?.Invoke();
    }

    private static async Task<byte[]> BuildPreviewAssembly(PlaygroundContext context)
    {
        var doc = context.Workspace.CurrentSolution.GetDocument(context.DocumentId);
        if (doc == null) throw new Exception("Document doesn't exist");

        var syntaxTree = await doc.GetSyntaxTreeAsync();
        var version = (int)(DateTime.Now - DateTime.UnixEpoch).TotalSeconds;
        var asmVersion = $"{version >> 24}.{(version >> 16) & 0xFF}.{version & 0xFFFF}";
        var usingAndVersionTree = SyntaxFactory.ParseSyntaxTree(
            context.GlobalUsings +
            $"using System.Reflection;using System.Runtime.CompilerServices;using System.Runtime.Versioning;[assembly: AssemblyVersion(\"{asmVersion}\")]",
            context.ParseOptions);
        var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, false)
            .WithNullableContextOptions(NullableContextOptions.Enable)
            .WithOptimizationLevel(OptimizationLevel.Debug);

        //开始编译运行时代码
        var compilation = CSharpCompilation.Create(null)
            .AddReferences(context.References)
            .AddSyntaxTrees(syntaxTree!, usingAndVersionTree)
            .WithOptions(options);

        using var dllStream = new MemoryStream(1024);
        var emitResult = compilation.Emit(dllStream);
        if (!emitResult.Success)
            throw new Exception("Compile failed");

        var asmData = dllStream.ToArray();
        return asmData;
    }
}

internal sealed class ViewAssemblyLoader : AssemblyLoadContext
{
    public ViewAssemblyLoader() : base(true) { }

    public Assembly LoadViewAssembly(byte[] asmData)
    {
        if (asmData == null)
            throw new ArgumentNullException(nameof(asmData));

        using var oms = new MemoryStream(asmData);
        return LoadFromStream(oms);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
#if DEBUG
        Console.WriteLine($"ViewAssemblyLoader.Load: {assemblyName.Name}");
#endif
        //目前实现是所有依赖的其他模型已经打包入预览用的程序集
        return null;
    }
}