using Microsoft.CodeAnalysis.Host;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Storage;

namespace PixUI.CodeEditor.Roslyn;

[ExportWorkspaceServiceFactory(typeof(IPersistentStorageConfiguration), ServiceLayer.Host)]
public sealed class WebPersistentStoreConfigurationFactory : IWorkspaceServiceFactory
{
    public IWorkspaceService CreateService(HostWorkspaceServices workspaceServices)
    {
        return new WebPersistentStorageConfiguration();
    }
}

internal sealed class WebPersistentStorageConfiguration : IPersistentStorageConfiguration
{
    public string? TryGetStorageLocation(SolutionKey solutionKey)
    {
        return null;
    }

    public bool ThrowOnFailure => false;
}