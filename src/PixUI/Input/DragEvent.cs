using System;

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

public sealed class DragEvent : IDisposable
{
    public DropEffect DropEffect { get; set; }
    public IDataTransferItem TransferItem { get; set; } = null!;
    public Image DragHintImage { get; set; } = null!;

    public void Dispose()
    {
        if (DragHintImage != null!)
            DragHintImage.Dispose();
    }
}