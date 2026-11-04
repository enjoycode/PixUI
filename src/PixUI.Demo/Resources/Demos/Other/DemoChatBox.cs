using PixUI.AIChatBox;

public class DemoChatBox : View
{
    public DemoChatBox()
    {
        Child = new Column
        {
              Children =
              [
                  new Expanded(new ChatBox(_controller)),
                  new Card
                  {
                      Padding = EdgeInsets.All(5),
                      Child = new Row
                      {
                          Spacing = 5,
                          Children = 
                          [
                              new Expanded(new TextInput(_input)),
                              new Button("Append") { OnTap = _ => Append() },
                          ]
                      }
                  }
              ]  
        };
    }
    
    private readonly ChatBoxController _controller = new();
    private readonly State<string> _input = string.Empty;
    
    private void Append()
    {
        if (string.IsNullOrEmpty(_input.Value)) return;
        
        _controller.AppendAIThinkContent(_input.Value);
    }
}