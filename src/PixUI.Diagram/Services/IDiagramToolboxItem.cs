namespace PixUI.Diagram;

public interface IDiagramToolboxItem
{

    bool IsConnection { get; }

    DiagramItem Create();

}