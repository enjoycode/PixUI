using System;

namespace PixUI;

public sealed class DropFileInput : SingleChildWidget, IMouseRegion
{
    public DropFileInput()
    {
        Width = 150;
        Height = 60;
        MouseRegion = new MouseRegion(opaque: true, allowDrop: AllowDrop);

        State<Color> color = Colors.Gray;
        Child = new Center
        {
            Child = new Column
            {
                Children =
                {
                    new Icon(MaterialIcons.SaveAlt) { Size = 30, Color = color },
                    new Text("Drop file here") { TextColor = color },
                }
            }
        };
    }

    public MouseRegion MouseRegion { get; }

    public Action<IDataTransferItem> OnDrop
    {
        set => MouseRegion.Drop += value;
    }

    private bool AllowDrop(IDataTransferItem item)
    {
        //TODO: 检查类型及大小限制
        return item is FileDataTransferItem;
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //Draw background
        using var rect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H), 5, 5);
        var paint = PaintUtils.Shared(new Color(0xFFF2F2F2));
        canvas.DrawRRect(rect, paint);

        base.Paint(canvas, area);

        //Draw dash border
        paint = PaintUtils.Shared(Colors.Gray, PaintStyle.Stroke, 2f);
        using var dash = PathEffect.CreateDash(new[] { 5f, 5f }, 10);
        paint.PathEffect = dash;
        canvas.DrawRRect(rect, paint);
    }
}