using System;

namespace PixUI;

public enum DropEffect
{
    None,
    Move,
    Copy,
    Link,
}

public enum DropPosition
{
    In,
    Up,
    Down,
    Left,
    Right
}

public sealed class DragEvent : IDisposable
{
    private Image? _dragHintImage;
    private Image? _dropHintImage;
    
    public DropEffect DropEffect { get; set; }
    public DropPosition DropPosition { get; set; }
    public IDataTransferItem TransferItem { get; set; } = null!;

    public Image DragHintImage
    {
        get => _dragHintImage!;
        set
        {
            _dragHintImage?.Dispose();
            _dragHintImage = value;
        }
    }

    public Image? DropHintImage
    {
        get => _dropHintImage;
        set
        {
            _dropHintImage?.Dispose();
            _dropHintImage = value;
        }
    }

    public void Dispose()
    {
        _dragHintImage?.Dispose();
        _dropHintImage?.Dispose();
    }
}