using System.Collections.Immutable;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Text;

namespace PixUI.CodeEditor.Roslyn;

public class RoslynWorkspace : Workspace
{
    // public bool Initialized { get; set; }
    // public BufferManager BufferManager { get; private set; }

    public RoslynWorkspace() : this(new HostServicesAggregator()) { }

    public RoslynWorkspace(HostServicesAggregator aggregator)
        : base(aggregator.CreateHostServices(), "Custom")
    {
        // BufferManager = new BufferManager(this);
        if (OperatingSystem.IsBrowser())
        {
            //this.Services.HostServices
            Console.WriteLine("");
        }
    }

    public override bool CanOpenDocuments => true;

    public override bool CanApplyChange(ApplyChangesKind feature) => true;

    public override void OpenDocument(DocumentId documentId, bool activate = true)
    {
        var doc = CurrentSolution.GetDocument(documentId);
        if (doc != null)
        {
            var text = doc.GetTextAsync(CancellationToken.None)
                .WaitAndGetResult(CancellationToken.None);
            OnDocumentOpened(documentId, text.Container, activate);
        }
        else
        {
            Log.Warn($"Can't open document: {documentId}");
        }
    }

    public async Task OpenDocumentAsync(DocumentId documentId, bool activate = true)
    {
        var doc = CurrentSolution.GetDocument(documentId);
        if (doc != null)
        {
            var text = await doc.GetTextAsync(CancellationToken.None);
            OnDocumentOpened(documentId, text.Container, activate);
        }
        else
        {
            Log.Warn($"Can't open document: {documentId}");
        }
    }

    public override void CloseDocument(DocumentId documentId)
    {
        var doc = CurrentSolution.GetDocument(documentId);
        if (doc != null)
        {
            var text = doc.GetTextAsync(CancellationToken.None)
                .WaitAndGetResult(CancellationToken.None);
            var version = doc.GetTextVersionAsync(CancellationToken.None)
                .WaitAndGetResult(CancellationToken.None);
            var loader = TextLoader.From(TextAndVersion.Create(text, version, doc.FilePath));
            OnDocumentClosed(documentId, loader);
        }
        else
        {
            Log.Warn($"Can't close document: {documentId}");
        }
    }

    internal void OnDocumentChanged(DocumentId documentId, SourceText text)
    {
        OnDocumentTextChanged(documentId, text, PreservationMode.PreserveIdentity);
    }
}

public sealed class HostServicesAggregator
{
    private readonly ImmutableArray<Assembly> _assemblies;

    public HostServicesAggregator( /*IEnumerable<IHostServicesProvider> hostServicesProviders*/)
    {
        var builder = ImmutableHashSet.CreateBuilder<Assembly>();

        // We always include the default Roslyn assemblies:
        if (OperatingSystem.IsBrowser())
        {
            builder.Add(typeof(WebPersistentStoreConfigurationFactory).Assembly);
        }

        foreach (var assembly in MefHostServices.DefaultAssemblies)
        {
            builder.Add(assembly);
        }

        _assemblies = builder.ToImmutableArray();
    }

    public HostServices CreateHostServices()
    {
        return MefHostServices.Create(_assemblies);
    }
}