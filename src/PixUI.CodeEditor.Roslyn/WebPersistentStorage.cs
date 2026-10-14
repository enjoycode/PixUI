using System.Composition;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;

namespace PixUI.CodeEditor.Roslyn;

[Obsolete("Obsolete")]
[ExportWorkspaceServiceFactory(typeof(IPersistentStorageService), ServiceLayer.Host)]
public sealed class WebPersistentStoreServiceFactory : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        Console.WriteLine("Create WebPersistentStorageService");
        return new WebPersistentStorageService();
    }
}

[Obsolete("Obsolete")]
internal sealed class WebPersistentStorageService : IPersistentStorageService
{
    public IPersistentStorage GetStorage(Solution solution) => new WebPersistentStorage();
    
    
    private sealed class WebPersistentStorage : IPersistentStorage
    {
        public Task<Stream?> ReadStreamAsync(string name, CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult<Stream?>(null);

        public Task<Stream?> ReadStreamAsync(Project project, string name,
            CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult<Stream?>(null);

        public Task<Stream?> ReadStreamAsync(Microsoft.CodeAnalysis.Document document, string name,
            CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult<Stream?>(null);

        public Task<bool> WriteStreamAsync(string name, Stream stream,
            CancellationToken cancellationToken = new CancellationToken())
            => Task.FromResult(false);

        public Task<bool> WriteStreamAsync(Project project, string name, Stream stream,
            CancellationToken cancellationToken = new CancellationToken()) => Task.FromResult(false);

        public Task<bool> WriteStreamAsync(Microsoft.CodeAnalysis.Document document, string name, Stream stream,
            CancellationToken cancellationToken = new CancellationToken()) => Task.FromResult(false);
    }
}

