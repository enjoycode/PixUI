namespace PixUI.Dynamic.Design;

public sealed class SelectParentCommand : DesignCommand
{
    public override bool Run(DesignController controller)
    {
        if (controller.Selection.Count != 1) return false;

        var selectedItem = controller.FirstSelected!;
        if (selectedItem.IsRoot) return false;

        //TODO:考虑忽略反向包装的元素
        var parentElement = selectedItem.Parent!.FindParent(t => t is DesignElement);
        if (parentElement == null) return false;

        controller.Select((DesignElement)parentElement);
        return true;
    }

    public override void Undo(DesignController controller)
    {
        throw new System.NotImplementedException();
    }
}