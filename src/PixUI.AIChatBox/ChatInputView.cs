namespace PixUI.AIChatBox;

internal sealed class ChatInputView : View
{
    //TODO: 暂简单实现
    public ChatInputView(ChatBoxController controller)
    {
        _controller = controller;

        Child = new Card()
        {
            Child = new Column
            {
                Children =
                [
                    new TextInput(_input) { Border = null },
                    new Row()
                    {
                        Children =
                        [
                            new Button(icon: MaterialIcons.Add)
                                { Style = ButtonStyle.Transparent, Shape = ButtonShape.Pills },
                            new Expanded(),
                            new Button(icon: MaterialIcons.Upload)
                            {
                                Style = ButtonStyle.Transparent, Shape = ButtonShape.Pills,
                                OnTap = _ => SendMessage()
                            }
                        ]
                    }
                ]
            }
        };
    }

    private readonly ChatBoxController _controller;
    private readonly State<string> _input = string.Empty;

    private void SendMessage()
    {
        var text = _input.Value;
        _input.Value = string.Empty;
        
        _controller.SendUserMessage(text);
    }
}