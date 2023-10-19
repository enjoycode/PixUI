namespace PixUI.Dynamic.Design;

public sealed class DeleteElementsCommand : DesignCommand
{
    public override bool Run(DesignController controller)
    {
        //TODO: maybe check can be deleted
        DesignElement? lastParentElement = null;

        foreach (var element in controller.Selection)
        {
            if (element.IsRoot)
            {
                element.ClearMeta();
                element.Invalidate(InvalidAction.Relayout);
                controller.OnSelectionChanged();
                break; //ignore others
            }

            DesignElement parentElement;
            DesignElement childElement;
            Widget childWidget;
            if (element.Meta is { IsReversedWrapElement: true }) //本身是反向包装的
            {
                childElement = element;
                childWidget = element.Parent!;
                parentElement = (DesignElement)childWidget.Parent!.Parent!;
            }
            else if (element.Parent is DesignElement reversed) //上级是反向包装的
            {
                childElement = reversed;
                childWidget = reversed.Parent!;
                parentElement = (DesignElement)childWidget.Parent!.Parent!;
            }
            else
            {
                childWidget = childElement = element;
                parentElement = (DesignElement)element.Parent!.Parent!;
            }

            var slot = parentElement.Meta!.GetSlot(childElement.SlotName);
            if (slot.ContainerType == ContainerType.MultiChild)
            {
                if (slot.TryRemoveChild(parentElement.Target!, childWidget))
                {
                    parentElement.Invalidate(InvalidAction.Relayout);
                    lastParentElement = parentElement;
                }
            }
            else
            {
                if (slot.TrySetChild(parentElement.Target!, null))
                {
                    parentElement.Invalidate(InvalidAction.Relayout);
                    lastParentElement = parentElement;
                }
            }
        }

        if (lastParentElement != null)
            controller.Select(lastParentElement);
        return true;
    }

    public override void Undo(DesignController controller)
    {
        throw new System.NotImplementedException();
    }
}