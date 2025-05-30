using System;
using System.Diagnostics.CodeAnalysis;

namespace PixUI;

public sealed class TextInput : InputBase<EditableText>
{
    public TextInput()
    {
        Editor = new EditableText();
    }

    [SetsRequiredMembers]
    public TextInput(State<string> text) : this()
    {
        Text = text;
    }

    public required State<string> Text
    {
        init
        {
            Editor.Text = value;
            Readonly = value.Readonly;
        }
    }

    public State<float>? FontSize
    {
        get => Editor.FontSize;
        set => Editor.FontSize = value;
    }

    public State<Color>? TextColor
    {
        get => Editor.TextColor;
        set => Editor.TextColor = value;
    }

    public Widget? Prefix
    {
        set => PrefixWidget = value;
    }

    public Widget? Suffix
    {
        set => SuffixWidget = value;
    }

    public override State<bool>? Readonly
    {
        get => Editor.Readonly;
        set => Editor.Readonly = value;
    }

    public bool IsObscure
    {
        set => Editor.IsObscure = value;
    }

    public string? HintText
    {
        get => Editor.HintText;
        set => Editor.HintText = value;
    }

    public Action<string> OnCommitChanges
    {
        set => Editor.CommitChanges = value;
    }
}