namespace PixUI.Dynamic.Design;

public sealed class InputBorderEditor : ValueEditorBase
{
    public InputBorderEditor(State<InputBorder?> state, DesignElement element) : base(element)
    {
        _state = state;
        _element = element;

        Child = new Button("...") { Width = float.MaxValue, OnTap = OnTap };
    }

    private readonly State<InputBorder?> _state;
    private readonly DesignElement _element;

    private async void OnTap(PointerEvent _)
    {
        var dlg = new InputBorderDialog(_element);
        if (_state.Value != null)
        {
            var border = _state.Value;
            dlg.IsOutline.Value = border is OutlineInputBorder;
            dlg.Color.Value = border.BorderSide.Color;
            dlg.BorderWidth.Value = border.BorderSide.Width;
            if (dlg.IsOutline.Value)
                dlg.Radius.Value = ((OutlineInputBorder)border).BorderRadius.TopLeft.X;
        }

        var res = await dlg.ShowAsync();
        if (res != DialogResult.OK) return;

        var borderSide = new BorderSide(dlg.Color.Value, dlg.BorderWidth.Value);
        if (dlg.IsOutline.Value)
            _state.Value = new OutlineInputBorder(borderSide, BorderRadius.Circular(dlg.Radius.Value));
        else
            _state.Value = new UnderlineInputBorder(borderSide);
    }
}

public sealed class InputBorderDialog : Dialog
{
    public InputBorderDialog(DesignElement element)
    {
        Title.Value = "Input Border Settings";
        Width = 300;
        Height = 280;

        _element = element;
    }

    private readonly DesignElement _element;
    internal readonly State<bool> IsOutline = true;
    internal readonly State<Color> Color = new Color(0xFF9B9B9B);
    internal readonly State<float> BorderWidth = 1f;
    internal readonly State<float> Radius = 4f;

    protected override Widget BuildBody() => new Container
    {
        Padding = EdgeInsets.All(20),
        Child = new Form()
        {
            LabelWidth = 60,
            Children =
            {
                new("Type:", new Row
                {
                    Children =
                    {
                        new Radio(IsOutline),
                        new Text("Outline"),
                        new Radio(IsOutline.ToReversed()),
                        new Text("Underline"),
                    }
                }),
                new("Color:", new ColorEditor(Color.ToNullable(), _element)),
                new("Width:", new NumberInput<float>(BorderWidth)),
                new("Radius:", new NumberInput<float>(Radius)), //TODO: visible for Outline only
            }
        }
    };
}