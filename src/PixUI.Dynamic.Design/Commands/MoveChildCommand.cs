namespace PixUI.Dynamic.Design;

public sealed class MoveChildCommand : DesignCommand
{
    public MoveChildCommand(MoveChildAction action)
    {
        _action = action;
    }

    private readonly MoveChildAction _action;

    public override bool Run(DesignController controller)
    {
        if (controller.Selection.Count != 1)
            return false; //暂只支持单个移动

        var current = controller.FirstSelected!;

        DesignElement? parentContainer;
        Widget? child;
        string? slotName;
        if (current.Meta is { IsReversedWrapElement: true }) //本身是反向包装的
        {
            //eg: RowElement->Row->Expanded->ExpandedElement
            parentContainer = current.Parent!.Parent!.Parent as DesignElement;
            slotName = current.SlotName;
            child = current.Target;
        }
        else if (current.Parent is DesignElement { Meta.IsReversedWrapElement: true } parentElement) //上级是反向包装的
        {
            //eg: RowElement->Row->Expanded->ExpandedElement->ButtonElement
            parentContainer = current.Parent!.Parent!.Parent!.Parent as DesignElement;
            slotName = parentElement.SlotName;
            child = parentElement.Target;
        }
        else
        {
            //eg: RowElement->Row->ButtonElement
            parentContainer = current.Parent!.Parent as DesignElement;
            slotName = current.SlotName;
            child = current;
        }

        if (parentContainer == null || parentContainer.Meta == null)
            return false;

        var slot = parentContainer.Meta.GetSlot(slotName);
        if (slot.ContainerType != ContainerType.MultiChild) return false;

        var res = slot.TryMoveChild(parentContainer.Target!, child!, _action);
        parentContainer.Invalidate(InvalidAction.Relayout);
        controller.RaiseOutlineChanged();
        return res;
    }

    public override void Undo(DesignController controller)
    {
        throw new System.NotImplementedException();
    }
}