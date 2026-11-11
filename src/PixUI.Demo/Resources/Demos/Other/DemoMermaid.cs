using PixUI.Mermaid;

public class DemoMermaid : View
{
    public DemoMermaid()
    {
        var mermaid = """
                      graph TD
                      A[开始] --> B{判断}
                      B -->|条件1| C[处理1]
                      B -. 条件2 .-> D[处理2]
                      C --> E[结束]
                      D --> E
                      """;
        _picture = MermaidRenderer.RenderPicture(mermaid);
    }
    
    private IPicture? _picture;
    
    protected override void OnLayout(Size maxSize)
    {
        SetLayoutSize(maxSize);
    }
    
    public override void OnPaint(ICanvas canvas, IDirtyArea? area = null)
    {
        if (_picture == null)
            return;
        canvas.DrawPicture(_picture, 0, 0);
    }
}