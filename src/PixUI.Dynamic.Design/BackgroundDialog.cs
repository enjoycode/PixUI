using System;

namespace PixUI.Dynamic.Design;

/// <summary>
/// 设置动态组件背景的对话框
/// </summary>
public sealed class BackgroundDialog : Dialog
{
    //TODO: 暂简单实现只支持背景图片

    public BackgroundDialog()
    {
        Width = 300;
        Height = 180;
        Title.Value = "Background Settings";
    }

    private byte[]? _imgData;

    public DynamicBackground? GetBackground()
    {
        if (_imgData == null) return null;

        return new DynamicBackground() { ImageData = _imgData };
    }

    protected override Widget BuildBody()
    {
        return new Form()
        {
            LabelWidth = 80,
            Children =
            {
                new FormItem("Image:", new DropFileInput { OnDrop = OnDropFile })
            }
        };
    }

    private async void OnDropFile(IDataTransferItem item)
    {
        var file = (FileDataTransferItem)item;
        try
        {
            _imgData = new byte[file.Size];
            _ = await file.Stream.ReadAsync(_imgData, 0, file.Size);
            //检查是否图片
            using var image = Image.FromEncodedData(_imgData);
            if (image == null)
                _imgData = null;
        }
        catch (Exception e)
        {
            Log.Warn($"Image file error: {e.Message}");
        }
    }
}