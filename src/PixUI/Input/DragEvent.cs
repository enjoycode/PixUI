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
    Upper,
    Under,
    Left,
    Right
}

public sealed class DragEvent : IDisposable
{
    private IImage? _dragHintImage;
    private IImage? _dropHintImage;
    
    public DropEffect DropEffect { get; set; }
    public DropPosition DropPosition { get; set; }
    public IDataTransferItem TransferItem { get; set; } = null!;

    public IImage DragHintImage
    {
        get => _dragHintImage!;
        set
        {
            _dragHintImage?.Dispose();
            _dragHintImage = value;
        }
    }

    public IImage? DropHintImage
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