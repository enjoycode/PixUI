using System.Threading.Tasks;

namespace PixUI.Dynamic.Design;

public sealed class DesignCanvas : View, IDynamicView
{
    public DesignCanvas(DesignController controller)
    {
        _designController = controller;

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

        return ((IDynamicDataSetStateValue)state.Value).GetRuntimeDataSet();
    }
}