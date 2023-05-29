using System;

namespace PixUI;

public sealed class IfConditional : Conditional<bool>
{
    public IfConditional(State<bool> state, Func<Widget> trueBuilder,
        Func<Widget>? falseBuilder = null) : base(state)
    {
        When(v => v, trueBuilder);
        if (falseBuilder != null)
            When(v => !v, falseBuilder);
    }
}