using System;
using System.IO;
using PixUI.Demo.Agent;
using PixUI.Platform;

namespace PixUI.Demo;

internal sealed class MenuPad : View
{
    public MenuPad(PlaygroundContext context)
    {
        _context = context;

        Child = new Container()
        {
            Height = 45,
            FillColor = _bgColor,
            Child = new Row()
            {
                Children =
                [
                    new Container { Width = 50 },
                    new Expanded
                    {
                        Child = new MainMenu(BuildMenuItems())
                            { BackgroundColor = _bgColor, Color = Colors.White }
                    },
                ]
            }
        };
    }

    private readonly Color _bgColor = new Color(43, 49, 56);
    private readonly PlaygroundContext _context;

    private MenuItem[] BuildMenuItems() =>
    [
        MenuItem.SubMenu("File", MaterialIcons.Code, [
            MenuItem.Item("Open", MaterialIcons.FileOpen),
            MenuItem.Item("Save", MaterialIcons.SaveAs, SaveFile),
        ]),
        MenuItem.Item("AI Chat", MaterialIcons.Chat, OpenAIChat),
        MenuItem.Item("Tool", MaterialIcons.Build),
        MenuItem.Item("Help", MaterialIcons.Help),
    ];

    private async void SaveFile()
    {
        try
        {
            var doc = _context.RoslynDocument;
            var sourceText = await doc.GetTextAsync();
            using var ms = new MemoryStream();
            await using var textWriter = new StreamWriter(ms);
            sourceText.Write(textWriter);
            await textWriter.FlushAsync();
            ms.Position = 0;
            await FileDialog.SaveFileAsync(new SaveFileOptions() { Title = "保存", FileStream = ms });
        }
        catch (Exception e)
        {
            Notification.Error($"Save Error: {e.Message}");
        }
    }

    private static void OpenAIChat() =>
        Dialog.Show("AI Chat", _ => new AgentView(), size: new(600, 600));
}