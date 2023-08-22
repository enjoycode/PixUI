using System;
using System.Numerics;

namespace PixUI;

public sealed class NumberInput<T> : InputBase<EditableText> where T : struct, INumber<T>, IMinMaxValue<T>
{
    public NumberInput(T numberValue) : this(new RxValue<T>(numberValue)) { }

    public NumberInput(State<T> number) : base(new EditableText(number.ToString()))
    {
        _noneNullable = Bind(number, BindingOptions.None);
        _editor.PreviewInput = OnPreviewInput;
        _editor.CommitChanges = OnCommitChanges;
    }

    public NumberInput(State<T?> number) : base(new EditableText(number.ToString()))
    {
        _nullable = Bind(number, BindingOptions.None);
        _editor.PreviewInput = OnPreviewInput;
        _editor.CommitChanges = OnCommitChanges;
    }

    private readonly State<T>? _noneNullable;
    private readonly State<T?>? _nullable;

    public override State<bool>? Readonly
    {
        get => _editor.Readonly;
        set => _editor.Readonly = value;
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
        if (!T.TryParse(text, null, out var result))
            return;

        if (_nullable != null) _nullable.Value = result;
        else _noneNullable!.Value = result;
    }

    public override void OnStateChanged(State state, BindingOptions options)
    {
        if (ReferenceEquals(state, _editor.Text))
            _editor.Text.Value = state.ToString() ?? string.Empty;
        base.OnStateChanged(state, options);
    }
}