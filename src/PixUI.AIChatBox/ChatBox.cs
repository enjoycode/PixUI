namespace PixUI.AIChatBox;

public sealed class ChatBox : View
{
    public ChatBox(ChatBoxController controller)
    {
        IsLayoutTight = false;
        Child = new Column()
        {
            Children =
            [
                new Expanded(new Container
                {
                    Padding = EdgeInsets.All(10),
                    Child = new ChatMessagesView(controller)
                }),
                new ChatInputView(controller)
            ]
        };
    }
}