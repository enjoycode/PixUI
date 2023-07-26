namespace PixUI.Demo
{
    public sealed class DemoForm : View
    {
        public DemoForm()
        {
            Child = new Form()
            {
                Padding = EdgeInsets.All(20),
                LabelWidth = 80,
                Columns = 2,
                Children =
                {
                    new("姓名:", new Input("")),
                    new("性别:", new Input("")),
                    new("电话:", new Input("")),
                    new("城市:", new Input("")),
                    new("住址:", new Input(""), 2),
                }
            };
        }
    }
}