#if !__WEB__
namespace PixUI
{
    public partial class SkiaApi
    {
#if __IOS__ || __TVOS__ || __WATCHOS__
		private const string SKIA = "@rpath/libskia.framework/libskia";
#else
        private const string SKIA = "skia";
#endif
    }
}
#endif