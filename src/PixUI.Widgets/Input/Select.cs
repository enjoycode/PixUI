using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace PixUI;

/// <summary>
/// 弹出选择列表，仅支持单选
/// </summary>
public abstract class SelectBase<T> : InputBase<Widget>
{
    protected SelectBase(bool filterable)
    {
        Editor = filterable
            ? new EditableText()
            : new SelectText();

        _iconColor = new RxProxy<Color>(() => TextColor?.Value ?? new Color(0xff5f6368));
        SuffixWidget = new ExpandIcon(new FloatTween(0, 0.5f).Animate(_expandAnimation), color: _iconColor);

        if (Editor is IMouseRegion mouseRegion)
            mouseRegion.MouseRegion.PointerTap += OnEditorTap;
        if (Editor is IFocusable focusable)
            focusable.FocusNode.FocusChanged += OnFocusChanged;
    }

    private readonly State<T?> _selectedValue = null!;
    private readonly ListPopupItemBuilder<T>? _optionBuilder = null;
    private readonly OptionalAnimationController _expandAnimation = new();
    private readonly State<Color> _iconColor;
    private ListPopup<T>? _listPopup;
    private bool _showing;
    private Func<T, string>? _labelGetter;

    public bool Filterable { get; init; }

    public State<T?> Value
    {
        get => _selectedValue;
        init
        {
            _selectedValue = value;
            BindEditorTextState();
        }
    }

    public T[] Options { get; set; } = [];

    public Task<T[]> OptionsAsyncGetter
    {
        set => GetOptionsAsync(value);
    }

    public Func<T, string> LabelGetter
    {
        set
        {
            _labelGetter = value;
            BindEditorTextState();
        }
    }

    public State<Color>? TextColor
    {
        get => ((TextBase)Editor).TextColor;
        set
        {
            ((TextBase)Editor).TextColor = value;
            _iconColor.NotifyValueChanged();
        }
    }

    public override State<bool>? Readonly
    {
        get
        {
            if (Editor is EditableText editableText) return editableText.Readonly;
            return ((SelectText)Editor).Readonly;
        }
        set
        {
            if (Editor is EditableText editableText) editableText.Readonly = value;
            else ((SelectText)Editor).Readonly = value;
        }
    }

    private void BindEditorTextState()
    {
        if (_selectedValue == null!) return;

        var editor = (TextBase)Editor;
        editor.Text = _labelGetter == null
            ? _selectedValue.ToStateOfString()
            : _selectedValue.ToComputed(v => v == null ? string.Empty : _labelGetter(v));
    }

    private void OnFocusChanged(FocusChangedEvent e)
    {
        if (!e.IsFocused)
            HidePopup();
    }

    private void OnEditorTap(PointerEvent e)
    {
        if (_showing) HidePopup();
        else ShowPopup();
    }

    private void ShowPopup()
    {
        if (_showing || Options.Length == 0) return;
        _showing = true;

        var optionBuilder =
            _optionBuilder ??
            ((data, _, _, isSelected) =>
            {
                var color = RxComputed<Color>.Make(isSelected, v => v ? Colors.White : Colors.Black);
                return new Text(_labelGetter != null ? _labelGetter(data) : data?.ToString() ?? "")
                    { TextColor = color };
            });
        _listPopup = new ListPopup<T>(Overlay!, optionBuilder, W + 8, Theme.DefaultFontSize + 8);
        _listPopup.DataSource = new List<T>(Options);
        _listPopup.AutoFitInWindowWidth = true;
        //初始化选中的
        if (_selectedValue.Value != null)
            _listPopup.InitSelect(_selectedValue.Value!);
        _listPopup.OnSelectionChanged = OnSelectionChanged;
        _listPopup.Show(this, new Offset(-4, 0), Popup.DefaultTransitionBuilder);
        _expandAnimation.Parent = _listPopup.AnimationController;
    }

    private void HidePopup()
    {
        if (!_showing) return;
        _showing = false;

        _listPopup?.Hide();
        _listPopup = null;
    }

    private async void GetOptionsAsync(Task<T[]> builder)
    {
        Options = await builder;
    }

    private void OnSelectionChanged(T? data)
    {
        _showing = false;
        _listPopup = null;
        _selectedValue.Value = data;
    }
}

public sealed class Select<T> : SelectBase<T>
{
    public Select() : base(false) { }

    [SetsRequiredMembers]
    public Select(State<T?> value) : this()
    {
        Value = value;
    }
}

public sealed class EnumSelect<T> : SelectBase<T> where T : struct, Enum
{
    public EnumSelect() : base(false) { }

    [SetsRequiredMembers]
    public EnumSelect(State<T> value) : this()
    {
        Value = value;
        //TODO:use DisplayNameAttribute to build DisplayText
        Options = Enum.GetValues<T>();
    }
}

internal sealed class SelectText : TextBase, IMouseRegion, IFocusable
{
    public MouseRegion MouseRegion { get; } = new();
    public FocusNode FocusNode { get; } = new();

    private State<bool>? _readonly;

    public State<bool>? Readonly
    {
        get => _readonly;
        set => Bind(ref _readonly, value, RepaintOnStateChanged);
    }

    protected override bool ForceHeight => true;

    public override void Layout(float availableWidth, float availableHeight)
    {
        var maxSize = CacheAndGetMaxSize(availableWidth, availableHeight);

        BuildParagraph(Text.Value, maxSize.Width);

        var fontHeight = (FontSize?.Value ?? Theme.DefaultFontSize) + 4;
        SetSize(maxSize.Width, Math.Min(maxSize.Height, fontHeight));
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        if (Text.Value.Length == 0) return;
        canvas.DrawParagraph(CachedParagraph!, 0, 2 /*offset*/);
    }
}