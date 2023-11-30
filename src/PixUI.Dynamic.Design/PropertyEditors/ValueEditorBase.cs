namespace PixUI.Dynamic.Design;

public abstract class ValueEditorBase : SingleChildWidget
{

    protected ValueEditorBase(DesignController controller)
    {
        Controller = controller;
    }

    public readonly DesignController Controller;

}