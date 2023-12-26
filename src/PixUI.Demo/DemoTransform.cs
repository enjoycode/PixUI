using System;

namespace PixUI.Demo.Mac
{
    public sealed class DemoTransform : View
    {
        public DemoTransform()
        {
            var m2 = Matrix4.CreateIdentity();
            //m2.Scale(1, 0.5f);
            m2.Translate(50, 50);
            m2.RotateZ(20 * Matrix3.DegreesToRadians);
            // var m1 = Matrix4.CreateRotationDegrees(0, 0, 1, 45);

            Child = new Container
            {
                DebugLabel = "DemoTransform.Green",
                Width = 300,
                Height = 300,
                FillColor = Colors.Green,
                Child = new Transform(m2, new Offset(0, 0), false)
                {
                    // Child = new HitTestWidget()
                    // {
                    //     DebugLabel = "DemoTransform.Red",
                    //     Width = 100,
                    //     Height = 100,
                    // }
                    Child = new Column
                    {
                        Children =
                        {
                            new TextInput("Hello"),
                            new Button("Click")
                        }
                    }
                }
            };
        }
    }

    internal sealed class HitTestWidget : Widget, IMouseRegion
    {
        public MouseRegion MouseRegion { get; }

        public HitTestWidget()
        {
            MouseRegion = new MouseRegion(null, false);
            MouseRegion.PointerDown += e =>
            {
                var winPt = LocalToWindow(e.X, e.Y);
                Console.WriteLine(
                    $">Transformed Hit: local=({e.X}, {e.Y}) win=({winPt.X}, {winPt.Y})");
            };
        }

        protected override bool HitTest(float x, float y, HitTestResult result)
        {
            if (!ContainsPoint(x, y)) return false;

            var winPt = LocalToWindow(x, y);
            Console.WriteLine(
                $"HitTest in Transformed: local=({x}, {y}) win=({winPt.X}, {winPt.Y})");

            result.Add(this);
            return true;
        }

        public override void Layout(float availableWidth, float availableHeight)
        {
            var width = Math.Min(availableWidth, Width?.Value ?? float.MaxValue);
            var height = Math.Min(availableHeight, Height?.Value ?? float.MaxValue);
            SetSize(width, height);
        }

        public override void Paint(Canvas canvas, IDirtyArea? area = null)
        {
            canvas.DrawRect(Rect.FromLTWH(0, 0, W, H), PixUI.Paint.Shared(Colors.Red));
        }
    }
}