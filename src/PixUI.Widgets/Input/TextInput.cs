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
    public TextInput(BindableProperty<string> text) : this()
    {
        Text = text;
    }

    public required BindableProperty<string> Text
    {
        init
        {
            Editor.Text = value;
            //Readonly = value.Readonly;
        }
    }

    public TextWidgetStyle Style
    {
        get => Editor.Style;
        set => Editor.Style = value;
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