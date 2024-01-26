namespace PixUI;

public enum DropEffect
{
    None,
    MoveBefore,
    MoveIn,
    MoveAfter,
    Copy,
    Link,
}

public sealed class DragEvent
{
    public DropEffect DropEffect { get; set; }
}