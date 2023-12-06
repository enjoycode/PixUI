using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace PixUI;

public sealed class NumberInput<T> : InputBase<EditableText> where T : struct, INumber<T>, IMinMaxValue<T>
{
    public NumberInput() { }

    [SetsRequiredMembers]
    public NumberInput(T numberValue) : this(new RxValue<T>(numberValue)) { }

    [SetsRequiredMembers]
    public NumberInput(State<T> number) : this(number.ToNullable()) { }

    [SetsRequiredMembers]
    public NumberInput(State<T?> number) => Number = number;

    private readonly State<T?> _nullable = null!;

    public required State<T?> Number
    {
        init
        {
            _nullable = Bind(value, OnNullableChanged);
            Editor = new EditableText(_nullable.ToString());
            Editor.PreviewInput = OnPreviewInput;
            Editor.CommitChanges = OnCommitChanges;
        }
    }

    public override State<bool>? Readonly
    {
        get => Editor.Readonly;
        set => Editor.Readonly = value;
    }

    public int Decimals { get; init; } = 0;

    private bool OnPreviewInput(string text)
    {
        if (!T.TryParse(text, null, out var result))
            return false;

        //TODO:检查超出值范围
        //TODO:检查小数范围
        return true;
    }

    private void OnCommitChanges(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            _nullable.Value = null;
            return;
        }

        if (!T.TryParse(text, null, out var result))
            return;

        _nullable.Value = result;
    }

    private void OnNullableChanged(State state)
    {
        Editor.Text.Value = state.ToString() ?? string.Empty;
    }
}