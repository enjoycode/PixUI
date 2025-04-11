using System.Collections.Generic;

namespace PixUI;

public sealed class Inspector : Widget
{
    private Inspector() { }

    private Widget _target = null!;

    #region ====Show & Remove Method====

    public static Inspector? Show(Widget target)
    {
        if (!target.IsMounted) return null;

        var overlay = target.Overlay!;
        var inspector = overlay.FindEntry(w => w is Inspector);
        if (inspector == null)
        {
            var instance = new Inspector();
            instance._target = target;
            overlay.Show(instance);
            return instance;
        }
        else
        {
            var instance = (Inspector)inspector;
            instance._target = target;
            instance.Repaint();
            return instance;
        }
    }

    public void Remove()
    {
        ((Overlay)Parent!).Remove(this);
    }

    #endregion

    #region ====Widget Overrides====

    protected internal override bool HitTest(float x, float y, HitTestResult result)
    {
        return false; //Can't hit now
    }

    public override void Layout(float availableWidth, float availableHeight)
    {
        //do nothing
    }

    public override void Paint(Canvas canvas, IDirtyArea? area = null)
    {
        var path = new List<Widget>();
        var temp = _target;
        while (temp.Parent != null)
        {
            path.Add(temp.Parent);
            temp = temp.Parent;
        }

        canvas.Save();
        for (var i = path.Count - 1; i >= 0; i--) //TODO:考虑跳过根节点
        {
            temp = path[i];
            canvas.Translate(temp.X, temp.Y);
            if (temp is IScrollable scrollable)
                canvas.Translate(-scrollable.ScrollOffsetX, -scrollable.ScrollOffsetY);
            else if (temp is Transform transform)
                canvas.Concat(transform.EffectiveTransform); //TODO:考虑画未变换前的边框
        }

        //draw bounds border
        var bounds = Rect.FromLTWH(_target.X + 0.5f, _target.Y + 0.5f, _target.W - 1f,
            _target.H - 1f);
        var borderColor = new Color(0x807F7EBE);
        var fillColor = new Color(0x80BDBDFC);
        canvas.DrawRect(bounds, PixUI.Paint.Shared(fillColor));
        canvas.DrawRect(bounds, PixUI.Paint.Shared(borderColor, PaintStyle.Stroke));

        //draw bounds text
        // var text = $"X: {_target.X} Y: {_target.Y} W: {_target.W} H: {_target.H}";
        // using var ph = TextPainter.BuildParagraph(text, float.PositiveInfinity, 12, Colors.Red);
        // canvas.DrawParagraph(ph, bounds.Left + 1, bounds.Top + 1);
        canvas.Restore();
    }

    #endregion
}