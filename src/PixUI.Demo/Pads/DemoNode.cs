using System.Collections.Generic;

namespace PixUI.Demo;

internal sealed class DemoNode
{
    public required string Name { get; init; }

    public required List<DemoNode> Children { get; init; }

    public bool IsLeaf => Children.Count == 0;
}