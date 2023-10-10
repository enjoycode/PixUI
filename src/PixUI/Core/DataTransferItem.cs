using System.IO;

namespace PixUI;

public interface IDataTransferItem { }

public sealed class FileDataTransferItem : IDataTransferItem
{
    public FileDataTransferItem(string name, int size, string type, Stream stream)
    {
        Name = name;
        Size = size;
        Type = type;
        Stream = stream;
    }

    public readonly string Name;
    public readonly int Size;
    public readonly string Type;
    public readonly Stream Stream;

    public override string ToString() => Name;
}