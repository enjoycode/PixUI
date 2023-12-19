using System;
using System.Threading.Tasks;

namespace PixUI.Dynamic.Design;

public sealed class DesignCanvas : View, IDynamicContext
{
    public DesignCanvas(DesignController controller)
    {
        _designController = controller;
        _designController.DesignCanvas = this;

        Child = new Container
        {
            FillColor = new Color(0xFFA2A2A2),
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

    DynamicState? IDynamicContext.FindState(string name) => _designController.FindState(name);

    protected internal override void BeforePaint(Canvas canvas, bool onlyTransform = false,
        IDirtyArea? dirtyArea = null)
    {
        canvas.Save();
        canvas.Translate(X, Y);
        //注意忽略onlyTransform参数始终clip
        canvas.ClipRect(Rect.FromLTWH(0, 0, W, H), ClipOp.Intersect, false);
    }

    protected internal override void AfterPaint(Canvas canvas)
    {
        canvas.Restore();
    }
}