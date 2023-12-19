namespace PixUI.Demo;

public sealed class DemoStack : View
{
    public DemoStack()
    {
        Child = new Stack
        {
            Children =
            {
                // Fill to all area
                new Positioned
                    { Left = 0, Top = 0, Right = 0, Bottom = 0, Child = new Container { FillColor = Colors.Gray } },
                // Dock Top
                new Positioned
                    { Left = 5, Top = 5, Right = 5, Height = 20, Child = new Container { FillColor = Colors.Red } },
                // Anchor Right & Bottom
                new Positioned
                    { Right = 5, Bottom = 5, Width = 50, Height = 50, Child = new Container { FillColor = Colors.Blue } },
                // Anchor Top & Left
                new Positioned
                    { Left = 100, Top = 100, Width = 50, Height = 50, Child = new Container { FillColor = Colors.Green } }
            }
        };
    }
}