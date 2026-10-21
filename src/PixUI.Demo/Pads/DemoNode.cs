using System.Collections.Generic;

namespace PixUI.Demo;

internal sealed class DemoNode
{
    public required DemoNode? Parent { get; init; }

    public required string Name { get; init; }

    public List<DemoNode> Children { get; } = [];

    public bool IsLeaf => Children.Count == 0;
}