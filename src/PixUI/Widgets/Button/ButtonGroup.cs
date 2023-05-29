using System;

namespace PixUI;

public sealed class ButtonGroup : MultiChildWidget<Button>
{
    public ButtonGroup() {}
        
    #region ====Widget Overrides====

    public override void Layout(float availableWidth, float availableHeight)
    {
        var width = CacheAndCheckAssignWidth(availableWidth);
        var height = CacheAndCheckAssignHeight(availableHeight);

        var xPos = 0f;
        State<float> buttonHeight = Math.Min(height, Button.DefaultHeight); //暂强制同高
        for (var i = 0; i < _children.Count; i++)
        {
            _children[i].Height = buttonHeight;
            _children[i].Shape = ButtonShape.Square;
            _children[i].Layout(Math.Max(0, width - xPos), buttonHeight.Value);
            _children[i].SetPosition(xPos, 0);

            xPos += _children[i].W;
        }

        SetSize(xPos, buttonHeight.Value);
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        //clip to round rectangle
        using var rrect = RRect.FromRectAndRadius(Rect.FromLTWH(0, 0, W, H),
            Button.StandardRadius, Button.StandardRadius);
        using var path = new Path();
        path.AddRRect(rrect);
        canvas.Save();
        canvas.ClipPath(path, ClipOp.Intersect, true);

        base.Paint(canvas, area);

        //画分隔条
        var paint = PaintUtils.Shared(Colors.White, PaintStyle.Stroke, 1);
        for (var i = 1; i < _children.Count; i++)
        {
            var x = _children[i].X - 0.5f;
            canvas.DrawLine(x, 0, x, H, paint);
        }

        canvas.Restore();
    }

    #endregion
}