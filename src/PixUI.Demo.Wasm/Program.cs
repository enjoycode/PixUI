using PixUI;
using PixUI.Platform.Wasm;

Console.WriteLine("Hello World!");

// //初始化默认字体
// await using var fontDataStream =
//     await BlazorApplication.HttpClient.GetStreamAsync("/fonts/MiSans-Regular.woff2");
// using var fontData = SKData.Create(fontDataStream);
// FontCollection.Instance.RegisterTypeface(fontData!, FontCollection.DefaultFamilyName, false);

WasmApplication.Run(() => new Center()
{
    Child = new Button("Hello World!") { Icon = MaterialIcons.Home }
});

// internal sealed class DemoWidget : Widget
// {
//     public override void Paint(Canvas canvas, IDirtyArea? area = null)
//     {
//         Log.Info($"Width={W}, Height={H} ClipBound={canvas.ClipBounds}");
//         
//         const float s = 30f;
//         var paint = PixUI.Paint.Shared(Colors.Red);
//         paint.Style = PaintStyle.Stroke;
//         paint.StrokeWidth = 2;
//
//         canvas.DrawRect(0, 0, W, H, paint);
//
//         canvas.DrawRect(0, 0, s, s, paint);
//         canvas.DrawRect(W - s, 0, s, s, paint);
//         canvas.DrawRect(0, H - s, s, s, paint);
//         canvas.DrawRect(W - s, H - s, s, s, paint);
//
//         var rrect = RRect.FromRectAndRadius(Rect.FromLTWH((W - s) / 2f, (H - s) / 2f, s, s), 10, 10);
//         canvas.DrawRRect(rrect, paint);
//     }
// }