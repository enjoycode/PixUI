using LiveCharts;
using LiveChartsCore;
using PixUI;
using PixUI.Platform.Wasm;

Console.WriteLine("Hello World!");

//初始化默认字体
// using var httpClient = new HttpClient();
// var fontUrl =
//     "https://github.com/enjoycode/PixUI/raw/refs/heads/main/src/PixUI.Platform.Blazor/wwwroot/fonts/MiSans-Regular.woff2";
// await using var fontDataStream = await httpClient.GetStreamAsync(fontUrl);
// using var fontData = SKData.Create(fontDataStream);
// FontCollection.Instance.RegisterTypeface(fontData!, FontCollection.DefaultFamilyName, false);
using var fontDataStream =
    typeof(Program).Assembly.GetManifestResourceStream("PixUI.Demo.Wasm.Fonts.NotoMono-Regular.woff2");
using var fontData = SKData.Create(fontDataStream!);
FontCollection.Instance.RegisterTypeface(fontData!, FontCollection.DefaultFamilyName, false);

WasmApplication.Run(() => new DemoChart());

internal sealed class DemoChart : View
{
    private readonly CartesianChart? _chart;

    public DemoChart()
    {
        Child = new Column()
        {
            Spacing = 10,
            Children =
            [
                new Card
                {
                    Width = 400,
                    Height = 300,
                    Child = new CartesianChart
                    {
                        Series = MakeSeries(),
                    }.RefBy(ref _chart!)
                },
                new Button("Hello World!") { Icon = MaterialIcons.Home, OnTap = _ => ChangeData() }
            ]
        };
    }

    private void ChangeData()
    {
        _chart!.Series = MakeSeries();
    }

    private ISeries[] MakeSeries()
    {
        return
        [
            new ColumnSeries<float> { Values = GetRandomData(), },
            new LineSeries<float> { Values = GetRandomData(), Fill = null }
        ];
    }

    private float[] GetRandomData()
    {
        var rand = Random.Shared;
        var result = new float[7];
        for (var i = 0; i < result.Length; i++)
        {
            result[i] = rand.Next(1, 30);
        }

        return result;
    }
}

#region ====TestCode====

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

#endregion