namespace PixUI.Dynamic.Design;

public abstract class DesignCommand
{
    public abstract bool Run(DesignController controller);

    public abstract void Undo(DesignController controller);
}