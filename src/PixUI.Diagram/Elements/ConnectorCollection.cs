using System.Collections.ObjectModel;

namespace PixUI.Diagram;

/// <summary>
/// A custom observable collection of connectors.
/// </summary>
public sealed class ConnectorCollection : ObservableCollection<IConnector>
{
    /// <summary>
    /// Gets the IConnector with at specified position.
    /// </summary>
    public IConnector? this[string name] => this.SingleOrDefault(c => c.Name == name);

    /// <summary>
    /// Determines whether the collection contains a connector with the specified name.
    /// </summary>
    /// <param name="name">The name of a connector.</param>
    /// <returns>
    ///   <c>true</c> if it's contained in the collection; otherwise, <c>false</c>.
    /// </returns>
    public bool Contains(string name) => this.Any(c => c.Name == name);
}