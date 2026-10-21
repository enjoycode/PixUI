namespace PixUI.Demo;

internal enum SidePadType
{
    Tree,
    Outline,
}

internal sealed class NaviBar : View
{
    private readonly State<Color> _buttonColor = Colors.White;
    private readonly State<float> _buttonSize = 25;
    private readonly PlaygroundContext _context;

    public NaviBar(PlaygroundContext context)
    {
        _context = context;

        Child = new Container
        {
            Padding = EdgeInsets.Only(5, 0, 0, 0),
            Width = 45,
            FillColor = new Color(43, 49, 56),
            Child = new Column
            {
                Children =
                {
                    new Expanded
                    {
                        Child = new Column(HorizontalAlignment.Center, 5)
                        {
                            Children =
                            [
                                BuildButton(MaterialIcons.Folder, SidePadType.Tree),
                                BuildButton(MaterialIcons.AccountTree, SidePadType.Outline),
                            ]
                        }
                    },
                    BuildButton(MaterialIcons.ArrowLeft)
                }
            }
        };
    }

    private Button BuildButton(in IconData icon, SidePadType? type = null)
    {
        var color = _buttonColor;
        if (type != null)
            color = _context.ActiveSidePad.ToComputed(s =>
                s == type ? new Color(0xFF4AC5EA) : new Color(0xFF6A7785));

        return new Button(null, icon)
        {
            FontSize = _buttonSize,
            TextColor = color, Style = ButtonStyle.Transparent,
            OnTap = e => OnClick(type)
        };
    }

    private void OnClick(SidePadType? type)
    {
        if (type != null)
            _context.ActiveSidePad.Value = type.Value;
    }
}

internal sealed class SidePad : View
{
    public SidePad(PlaygroundContext designContext)
    {
        Child = new Container
        {
            DebugLabel = "SidePad",
            Padding = EdgeInsets.All(5),
            FillColor = new Color(0xFFF3F3F3),
            Child = new Conditional<SidePadType>(designContext.ActiveSidePad)
                .When(t => t == SidePadType.Tree, () => new TreePad(designContext))
                .When(t => t == SidePadType.Outline, () => new OutlinePad(designContext))
        };
    }
}