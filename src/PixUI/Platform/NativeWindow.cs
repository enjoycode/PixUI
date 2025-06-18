#if !__WEB__
namespace PixUI.Platform
{
    public abstract class NativeWindow : UIWindow
    {
        protected bool IsActive = true;
        protected DisplayParams RequestedDisplayParams;
        protected NativeWindowContext? WindowContext;

        protected NativeWindow(Widget child) : base(child) { }

        public override float Width => WindowContext?.Width / ScaleFactor ?? 0;
        public override float Height => WindowContext?.Height / ScaleFactor ?? 0;

        public abstract bool Attach(BackendType backendType);

        protected void OnBackendCreated()
        {
            if (WindowContext == null) return;

            //TODO: eg set title

            Show(); //must show window first
            OnFirstShow();
        }

        protected void Detach()
        {
            WindowContext?.Dispose();
            WindowContext = null;
        }

        protected internal sealed override Canvas GetOnscreenCanvas()
            => WindowContext!.GetOnScreenCanvas();

        protected internal sealed override Canvas GetOffscreenCanvas()
            => WindowContext!.GetOffScreenCanvas();

        protected internal sealed override void Present()
            => WindowContext?.SwapBuffers();

        protected abstract void Show();

        public void OnResize(int width, int height)
        {
            if (WindowContext == null) return;

            WindowContext.Resize(width, height);
            //加入无效队列，准备重新布局并绘制
            RootWidget.CachedAvailableWidth = Width;
            RootWidget.CachedAvailableHeight = Height;
            InvalidQueue.Add(RootWidget, InvalidAction.Relayout, null);
            //System.Console.WriteLine($"Resize: {width} {height}");
        }

        public enum BackendType
        {
            Raster,
            Metal,
        }
    }
}
#endif