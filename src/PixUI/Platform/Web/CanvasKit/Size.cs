#if __WEB__
namespace PixUI
{
    [TSType("PixUI.Point")]
    public struct Size
    {
        public Size(float width, float height)
        {
            Width = Height = 0;
        }

        public float Width { get; }

        public float Height { get; }
    }
}
#endif