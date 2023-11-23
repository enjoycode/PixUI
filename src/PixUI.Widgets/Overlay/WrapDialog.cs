using System;

namespace PixUI;

internal sealed class WrapDialog : Dialog
{
    public WrapDialog(string title, Func<Dialog, Widget> bodyBuilder,
        Func<Dialog, Widget>? footerBuilder = null, Size? size = null)
    {
        _bodyBuilder = bodyBuilder;
        _footerBuilder = footerBuilder;

        Title.Value = title;
        Width = size?.Width ?? 400;
        Height = size?.Height ?? 300;
    }

    private readonly Func<Dialog, Widget> _bodyBuilder;
    private readonly Func<Dialog, Widget>? _footerBuilder;

    protected override Widget BuildBody() => _bodyBuilder(this);

    protected override Widget BuildFooter() =>
        _footerBuilder == null ? new Container { Width = 0, Height = 0 } : _footerBuilder(this);
}