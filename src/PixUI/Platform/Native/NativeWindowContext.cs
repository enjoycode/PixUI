#if !__WEB__
using System;

namespace PixUI.Platform
{
    public abstract class NativeWindowContext : IDisposable
    {
        protected GRContext? GrContext;

        /// <summary>
        /// Canvas physics pixel width 实际像素宽度
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Canvas physics pixel height 实际像素高度
        /// </summary>
        public int Height { get; protected set; }

        protected DisplayParams DisplayParams;
        protected int SampleCount = 1;
        protected int StencilBits = 0;
        protected readonly NativeWindow NativeWindow;

        protected NativeWindowContext(NativeWindow nativeWindow,  DisplayParams displayParams)
        {
            NativeWindow = nativeWindow;
            DisplayParams = displayParams;
        }

        public virtual bool IsGpuContext => true;

        /// <summary>
        /// 获取OnScreen画布(用于画Overlay并显示)
        /// </summary>
        /// <returns>注意返回的是未缩放的，因为有些平台底层使用多个Buffer</returns>
        public abstract Canvas GetOnScreenCanvas();

        /// <summary>
        /// 获取OffScreen画布(用于画Widgets)
        /// </summary>
        /// <returns>注意返回的是已经根据DevicePixelRatio缩放过的</returns>
        public abstract Canvas GetOffScreenCanvas();

        public abstract void SwapBuffers();

        /// <summary>
        /// 窗体改变大小后重新改变底层画布大小
        /// </summary>
        /// <param name="width">实际像素</param>
        /// <param name="height">实际像素</param>
        public abstract void Resize(int width, int height);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                GrContext?.Dispose();
                GrContext = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
#endif