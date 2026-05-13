namespace PixUI.Diagram;

public interface IDiagramToolbox
{

    IDiagramToolboxItem? SelectedItem { get; }

    void ClearSelectedItem();

}