namespace PixUI.AIChatBox;

public abstract class MessageElement
{
    internal abstract float Height { get; }
    
    internal abstract void OnLayout(float availableWidth);
    
    internal abstract void OnPaint(ICanvas canvas, ChatBoxController controller);
}