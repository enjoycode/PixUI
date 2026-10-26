public class DemoCard : View
{
    public DemoCard()
    {
        Child = new Center()
        {
            Child = new Card()
            {
                Child = new Container() { Width = 300, Height = 200, FillColor = Colors.Green }
            }
        };
    }
}