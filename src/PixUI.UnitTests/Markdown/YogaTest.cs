using System;
using Facebook.Yoga;
using NUnit.Framework;
using PixUI.Drawing.Skia;
using PixUI.Markdown;

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
        root.AppendChild(text1);
        // text1.Style.AlignSelf = Align.FlexStart; //交叉轴收缩宽
        text1.SetContext("Text1");
        text1.SetMeasureFunc(MeasureString);

        var text2 = new Node();
        root.AppendChild(text2);
        text2.SetContext("Text2: long long long long long long long line text2");

        var text3 = new Node();
        root.AppendChild(text3);
        text2.SetMeasureFunc(MeasureString);
        text3.SetContext("Text3");
        text3.SetMeasureFunc(MeasureString);

        LayoutAlgorithm.CalculateLayout(root, 200, float.NaN, Direction.LTR);
        DumpLayout(root);

        //修改宽度约束检查重新布局
        LayoutAlgorithm.CalculateLayout(root, 400, float.NaN, Direction.LTR);
        DumpLayout(root);
    }

    [Test(Description = "测试增量布局")]
    public void YogaIncrementalTest()
    {
        var root = new Node();
        root.SetContext("Root");

        var child1 = new Node();
        child1.SetContext("Child1");
        child1.SetStyleSize(100, 100);
        root.AppendChild(child1);

        var child1_1 = new Node();
        child1_1.SetContext("Child1_1");
        child1_1.SetStyleSize(50, 50);
        child1.AppendChild(child1_1);

        var child2 = new Node();
        child2.SetContext("Child2");
        child2.SetStyleSize(200, 200);
        root.AppendChild(child2);

        //第一次布局
        LayoutAlgorithm.CalculateLayout(root, float.NaN, float.NaN, Direction.LTR);
        ApplyLayout(root);
        DumpLayout(root);

        //修改第二子节点的大小检查增量布局
        child2.SetStyleSize(150, 150);
        LayoutAlgorithm.CalculateLayout(root, float.NaN, float.NaN, Direction.LTR);
        DumpLayout(root);
    }

    private static void ApplyLayout(Node node)
    {
        if (!node.HasNewLayout) return;

        node.HasNewLayout = false;
        foreach (var child in node.GetChildren())
            ApplyLayout(child);
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
        Console.Write($"New={node.HasNewLayout}");
        Console.WriteLine();

        if (node.GetChildCount() <= 0) return;
        foreach (var child in node.GetChildren())
        {
            DumpLayout(child, level + 1);
        }
    }
}