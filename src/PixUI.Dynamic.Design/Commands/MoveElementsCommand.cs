namespace PixUI.Dynamic.Design;

public sealed class MoveElementsCommand : DesignCommand
{
    public MoveElementsCommand(float dx, float dy)
    {
        _dx = dx;
        _dy = dy;
    }

    private readonly float _dx;
    private readonly float _dy;
    
    public override bool Run(DesignController controller)
    {
        //判断选择的是否可以移动，目前仅针对Stack下的Positioned组件,
        //另需要注意如果位置属性绑定了状态不可手工移动

        var canMove = true;
        foreach (var element in controller.Selection)
        {
            if (element.Target is Positioned ||
                element.Parent is DesignElement { Target: Positioned })
                continue;
            canMove = false;
            break;
        }

        if (!canMove) return false;


        foreach (var element in controller.Selection)
        {
            var moveable = element.Target is Positioned ? element : (DesignElement)element.Parent!;
            var target = moveable.Child as DesignElement;
            var positioned = (Positioned)moveable.Target!;
            var oldX = positioned.Left?.Value ?? positioned.X;
            var oldY = positioned.Top?.Value ?? positioned.Y;

            // Log.Debug($"old={oldX}, {oldY} delta={e.DeltaX}, {e.DeltaY}");
            moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Left", oldX + _dx));
            controller.NotifyLayoutPropertyChanged?.Invoke("Left");
            moveable.SetPropertyValue(moveable.Data.SetPropertyValue("Top", oldY + _dy));
            controller.NotifyLayoutPropertyChanged?.Invoke("Top");
            //clear Right & Bottom value
            if (moveable.Data.TryGetPropertyValue("Right", out var _))
            {
                target?.SetPropertyValue(target.Data.SetPropertyValue("Width", moveable.W));
                moveable.Data.RemovePropertyValue("Right");
                moveable.RemovePropertyValue("Right");
                controller.NotifyLayoutPropertyChanged?.Invoke("Width");
                controller.NotifyLayoutPropertyChanged?.Invoke("Right");
            }

            if (moveable.Data.TryGetPropertyValue("Bottom", out var _))
            {
                target?.SetPropertyValue(target.Data.SetPropertyValue("Height", moveable.H));
                moveable.Data.RemovePropertyValue("Bottom");
                moveable.RemovePropertyValue("Bottom");
                controller.NotifyLayoutPropertyChanged?.Invoke("Height");
                controller.NotifyLayoutPropertyChanged?.Invoke("Bottom");
            }
        }

        return true;
    }

    public override void Undo(DesignController controller)
    {
        throw new System.NotImplementedException();
    }
}