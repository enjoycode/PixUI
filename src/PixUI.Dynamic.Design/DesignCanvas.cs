using System;
using System.Threading.Tasks;

namespace PixUI.Dynamic.Design;

public sealed class DesignCanvas : View, IDynamicView
{
    public DesignCanvas(DesignController controller)
    {
        _designController = controller;
        _designController.DesignCanvas = this;

        Child = new Container
        {
            BgColor = new Color(0xFFA2A2A2),
            Padding = EdgeInsets.All(10),
            Child = new Card
            {
                Elevation = 10,
                Child = new Transform(Matrix4.CreateIdentity())
                {
                    Child = new DesignElement(controller, string.Empty)
                }
            }
        };
    }

    private readonly DesignController _designController;

    public ValueTask<object?> GetDataSet(string name)
    {
        var state = _designController.FindState(name);
        if (state == null || state.Type != DynamicStateType.DataSet || state.Value == null)
            return new ValueTask<object?>();

        return ((IDynamicDataSetState)state.Value).GetRuntimeDataSet();
    }

    public State GetState(string name)
    {
        var state = _designController.FindState(name);
        if (state == null)
            throw new Exception($"Can't find state: {name}");
        if (state.Type == DynamicStateType.DataSet)
            throw new Exception($"State is DataSet: {name}");
        return ((IDynamicValueState)state.Value!).GetRuntimeValue(state);
    }

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false, Rect? dirtyRect = null)
    {
        canvas.Save();
        if (X != 0 || Y != 0)
            canvas.Translate(X, Y);
        //注意忽略onlyTransform参数始终clip
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
    }
}