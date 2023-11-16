namespace PixUI;

internal sealed class WrapDialog : Dialog
{
    public WrapDialog(string title, Widget body, Size? size = null)
    {
        _body = body;
        Title.Value = title;
        Width = size?.Width ?? 400;
        Height = size?.Height ?? 300;
    }

    private readonly Widget _body;

    protected override Widget BuildBody() => _body;

    protected override Widget BuildFooter()
    {
        //TODO:根据属性构建，暂返回空
        return new Container { Width = 0, Height = 0 };
    }
}