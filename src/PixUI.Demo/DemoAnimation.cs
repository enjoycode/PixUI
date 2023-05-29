namespace PixUI.Demo.Mac
{
    public sealed class DemoAnimation : View
    {
        private readonly Rx<string> _title = "Hello World!";
        private readonly Rx<float> _fontSize = 30;
        private readonly Rx<Color> _color = Colors.Black;

        private readonly AnimationController _controller;
        private readonly Animation<Color> _colorAnimation;
        private readonly Animation<Offset> _offsetAnimation;
        private bool _forward = true;

        public DemoAnimation()
        {
            _controller = new AnimationController(1000);
            _controller.ValueChanged += OnAnimationValueChanged;
            _colorAnimation = new ColorTween(Colors.Black, Colors.Red).Animate(_controller);
            _offsetAnimation =
                new OffsetTween(new Offset(-1, 0), new Offset(1, 0)).Animate(_controller);

            Child = BuildChild();
        }

        private Widget BuildChild()
        {
            return new Column
            {
                Children = new Widget[]
                {
                    new Button("Play") { OnTap = _ => Play() },
                    new Text(_title) { FontSize = _fontSize, TextColor = _color },
                    new SlideTransition(_offsetAnimation)
                    {
                        Child = new Container
                        {
                            Width = 50,
                            Height = 50,
                            BgColor = Colors.Red
                        }
                    }
                }
            };
        }

        private void OnAnimationValueChanged()
        {
            _fontSize.Value = 30 + (float)(100 * _controller.Value);
            _color.Value = _colorAnimation.Value;
        }

        private void Play()
        {
            if (_forward)
                _controller.Forward();
            else
                _controller.Reverse();
            _forward = !_forward;
        }

        public override void Dispose()
        {
            _controller.Dispose();
            base.Dispose();
        }
    }
}