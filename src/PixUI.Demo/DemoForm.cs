namespace PixUI.Demo
{
    public sealed class DemoForm : View
    {
        public DemoForm()
        {
            Child = new Form()
            {
                //Padding = EdgeInsets.All(20),
                LabelWidth = 50,
                Columns = 2,
                Children =
                {
                    new("姓名:", new TextInput("")),
                    new("性别:", new TextInput("")),
                    new("电话:", new TextInput("")),
                    new("城市:", new TextInput("")),
                    new("住址:", new TextInput(""), 2),
                }
            };
        }
    }
}