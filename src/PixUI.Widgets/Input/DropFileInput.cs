using System;

namespace PixUI;

public sealed class DropFileInput : SingleChildWidget, IDroppable
{
    public DropFileInput()
    {
        Width = 150;
        Height = 60;
        MouseRegion = new MouseRegion(opaque: true);

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
    private readonly Action<IDataTransferItem>? _onDrop;

    public Action<IDataTransferItem> OnDrop
    {
        init => _onDrop = value;
    }

    bool IDroppable.AllowDrop(IDataTransferItem item)
    {
        //TODO: 检查类型及大小限制
        return item is FileDataTransferItem;
    }

    void IDroppable.OnDragOver(DragEvent dragEvent, Point local) { }

    void IDroppable.OnDrop(IDataTransferItem item) => _onDrop?.Invoke(item);

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //Draw background
        using var rect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H), 5, 5);
        var paint = PixUI.Paint.Shared(new Color(0xFFF2F2F2));
        canvas.DrawRRect(rect, paint);

        base.Paint(canvas, area);

        //Draw dash border
        paint = PixUI.Paint.Shared(Colors.Gray, PaintStyle.Stroke, 2f);
        using var dash = PathEffect.CreateDash(new[] { 5f, 5f }, 10);
        paint.PathEffect = dash;
        canvas.DrawRRect(rect, paint);
    }
}