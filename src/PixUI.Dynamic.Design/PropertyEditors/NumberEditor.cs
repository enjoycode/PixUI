using System.Numerics;

namespace PixUI.Dynamic.Design;

public sealed class NumberEditor<T> : ValueEditorBase where T : struct, INumber<T>, IMinMaxValue<T>
{
    // ReSharper disable once UnusedParameter.Local
    public NumberEditor(State<T?> state, DesignController controller) : base(controller)
    {
        Child = new NumberInput<T>(state);
    }
}