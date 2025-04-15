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
                element.Relayout();
                controller.OnSelectionChanged();
                controller.RaiseOutlineChanged();
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
                //直接删除上级 eg: Positioned or FormItem
                childElement = reversed;
                childWidget = reversed.Parent!;
                parentElement = (DesignElement)childWidget.Parent!.Parent!;

                // if (reversed.Target is Positioned) //同时删除Positioned
                // {
                //     childElement = reversed;
                //     childWidget = reversed.Parent!;
                //     parentElement = (DesignElement)childWidget.Parent!.Parent!;
                // }
                // else
                // {
                //     childWidget = childElement = element;
                //     parentElement = reversed;
                // }
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
                    parentElement.Relayout();
                    lastParentElement = parentElement;
                    controller.RaiseOutlineChanged();
                }
            }
            else
            {
                var isReversed = slot.ContainerType == ContainerType.SingleChildReversed;
                if (isReversed)
                {
                    parentElement.Child = null;
                    parentElement.Parent?.Parent?.Relayout(); //暂重布局上级的上级
                    lastParentElement = parentElement;
                    controller.RaiseOutlineChanged();
                }
                else
                {
                    if (slot.TrySetChild(parentElement.Target!, null))
                    {
                        parentElement.Relayout();
                        lastParentElement = parentElement;
                        controller.RaiseOutlineChanged();
                    }
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