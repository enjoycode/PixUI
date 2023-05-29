using System;

namespace PixUI.Demo
{
    public sealed class DemoDialog : Dialog
    {
        public DemoDialog()
        {
            Width = 400;
            Height = 300;
            Title.Value = "Demo Dialog";
        }

        private readonly State<string> _user = "";
        private readonly State<string> _password = "";

        protected override Widget BuildBody()
        {
            return new Container()
            {
                Padding = EdgeInsets.All(20),
                Child = new Column(HorizontalAlignment.Center, 20)
                {
                    Children = new Widget[]
                    {
                        new Input(_user) { HintText = "User" },
                        new Input(_password) { HintText = "Password", IsObscure = true }
                    }
                }
            };
        }
    }
}