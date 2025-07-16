namespace PixUI.Diagram;

public interface IDesignToolbox
{

    IDesignToolboxItem? SelectedItem { get; }

    void ClearSelectedItem();

}