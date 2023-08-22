using System.Numerics;

namespace PixUI.Dynamic.Design;

public sealed class NumberEditor<T> : SingleChildWidget where T : struct, INumber<T>, IMinMaxValue<T>
{
    public NumberEditor(State<T?> state)
    {
        Child = new NumberInput<T>(state);
    }
}