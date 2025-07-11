namespace PixUI.Diagram;

public interface IDesignToolboxItem
{

    bool IsConnection { get; }

    DiagramItem Create();

}