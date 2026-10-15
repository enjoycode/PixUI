using System;
using Facebook.Yoga;
using NUnit.Framework;
using PixUI.Drawing.Skia;

namespace PixUI.UnitTests.Markdown;

public class YogaTest
{
    [SetUp]
    public static void Setup()
    {
        Render.Init(new SkiaRender());
        Event.Subscribe((node, eventType, data) =>
        {
            // if (eventType ==  Facebook.Yoga.EventType.LayoutPassEnd) {
            //     var ld = data.GetData<Event.LayoutPassEndData>();
            //     Console.WriteLine($"Layouts: {ld?.LayoutData?.Layouts}");
            // }
            Console.WriteLine($"OnEvent[{eventType}]: [{node?.GetContext()}]");
        });
    }

    private static readonly ITextStyle _textStyle = TextStyle.Create();

    private static YGSize MeasureString(Node node, float availableWidth, MeasureMode widthMode,
        float availableHeight, MeasureMode heightMode)
    {
        Console.WriteLine(
            $"MeasureString: [{node.GetContext()}] W={availableWidth}[{widthMode}] H={availableHeight}[{heightMode}]");

        using var ps = ParagraphStyle.Create();
        using var pb = ParagraphBuilder.Create(ps);
        pb.PushStyle(_textStyle);
        pb.AddText(node.GetContext()!.ToString()!);
        pb.Pop();
        using var ph = pb.Build();
        ph.Layout(availableWidth);

        return new YGSize() { Width = ph.LongestLine, Height = ph.Height };
    }

    [Test]
    public void YogaFirstTest()
    {
        var root = new Node();
        root.SetContext("Root");
        root.Style.AlignItems = Align.FlexStart; //所有子对象交叉轴收缩

        var text1 = new Node();
        YGNodeAPI.YGNodeInsertChild(root, text1, 0);
        // text1.Style.AlignSelf = Align.FlexStart; //交叉轴收缩宽
        text1.SetContext("Text1");
        text1.SetMeasureFunc(MeasureString);

        var text2 = new Node();
        YGNodeAPI.YGNodeInsertChild(root, text2, 1);
        text2.SetContext("Text2: long long long long long long long line text2");

        var text3 = new Node();
        YGNodeAPI.YGNodeInsertChild(root, text3, 2);
        text2.SetMeasureFunc(MeasureString);
        text3.SetContext("Text3");
        text3.SetMeasureFunc(MeasureString);

        LayoutAlgorithm.CalculateLayout(root, 200, float.NaN, Direction.LTR);
        DumpLayout(root);

        // //修改宽度约束检查重新布局
        // LayoutAlgorithm.CalculateLayout(root, 400, float.NaN, Direction.LTR);
        // DumpLayout(root);

        //修改节点检查增量布局
        text2.SetContext("Text2: modified");
        text2.MarkDirtyAndPropagate();
        LayoutAlgorithm.CalculateLayout(root, 400, float.NaN, Direction.LTR);
        DumpLayout(root);
        
        // LayoutAlgorithm.CalculateLayout(root, 400, float.NaN, Direction.LTR);
        // DumpLayout(root);
    }

    private static void DumpLayout(Node node, int level = 0)
    {
        if (level == 0)
            Console.WriteLine("==============LayoutTree==================");
        for (var i = 0; i < level * 4; i++)
        {
            Console.Write(' ');
        }

        Console.Write($"[{node.GetContext()}] ");
        var layout = node.GetLayout();
        Console.Write($"X={layout.Position(PhysicalEdge.Left)} Y={layout.Position(PhysicalEdge.Top)} ");
        Console.Write($"W={layout.Dimension(Dimension.Width)} H={layout.Dimension(Dimension.Height)} ");
        Console.Write($"NL={node.HasNewLayout}");
        Console.WriteLine();

        if (node.GetChildCount() <= 0) return;
        foreach (var child in node.GetChildren())
        {
            DumpLayout(child, level + 1);
        }
    }
}