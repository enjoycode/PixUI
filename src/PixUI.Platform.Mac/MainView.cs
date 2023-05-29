using System;
using AppKit;

namespace PixUI.Platform.Mac
{
    internal sealed class MainView : NSView
    {
        internal readonly MacWindow MacWindow;

        // A TrackingArea prevents us from capturing events outside the view
        private NSTrackingArea? _trackingArea;

        public MainView(MacWindow macWindow)
        {
            MacWindow = macWindow;
            UpdateTrackingAreas();
        }

        public override void UpdateTrackingAreas()
        {
            if (_trackingArea != null)
            {
                RemoveTrackingArea(_trackingArea);
                _trackingArea = null;
            }

            const NSTrackingAreaOptions options = NSTrackingAreaOptions.MouseEnteredAndExited |
                                                  NSTrackingAreaOptions.ActiveInKeyWindow |
                                                  NSTrackingAreaOptions.EnabledDuringMouseDrag |
                                                  NSTrackingAreaOptions.CursorUpdate |
                                                  NSTrackingAreaOptions.InVisibleRect |
                                                  NSTrackingAreaOptions.AssumeInside;
            _trackingArea = new NSTrackingArea(Bounds, options, this, null);
            AddTrackingArea(_trackingArea!);

            base.UpdateTrackingAreas();
        }

        public override bool IsOpaque => true;

        public override bool CanBecomeKeyView => true;

        public override bool AcceptsFirstResponder() => true;

        // public override void DrawRect(CGRect dirtyRect)
        // {
        //     Console.WriteLine($"MainView.DrawRect: ${dirtyRect}");
        // }

        #region ====Mouse Events====

        public override void CursorUpdate(NSEvent theEvent)
        {
            //忽略不处理 base.CursorUpdate(theEvent);
        }

        public override void MouseMoved(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;
            if (rect.Contains(pos)) //暂排除窗体之外的事件
            {
                MacWindow.OnPointerMove(PointerEvent.UseDefault(PointerButtons.None, (float)pos.X,
                    (float)(rect.Height - pos.Y), (float)theEvent.DeltaX, (float)theEvent.DeltaY));
            }
            else
            {
                MacWindow.OnPointerMoveOutWindow();
            }
        }

        public override void MouseDragged(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;
            if (rect.Contains(pos)) //暂排除窗体之外的事件
            {
                MacWindow.OnPointerMove(PointerEvent.UseDefault(PointerButtons.Left, (float)pos.X,
                    (float)(rect.Height - pos.Y), (float)theEvent.DeltaX, (float)theEvent.DeltaY));
            }
        }

        public override void MouseDown(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;

            MacWindow.OnPointerDown(PointerEvent.UseDefault(PointerButtons.Left, (float)pos.X,
                (float)(rect.Height - pos.Y), 0, 0));
        }

        public override void RightMouseDown(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;

            MacWindow.OnPointerDown(PointerEvent.UseDefault(PointerButtons.Right, (float)pos.X,
                (float)(rect.Height - pos.Y), 0, 0));
        }

        public override void MouseUp(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;

            MacWindow.OnPointerUp(PointerEvent.UseDefault(PointerButtons.Left, (float)pos.X,
                (float)(rect.Height - pos.Y), 0, 0));
        }

        public override void RightMouseUp(NSEvent theEvent)
        {
            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;

            MacWindow.OnPointerUp(PointerEvent.UseDefault(PointerButtons.Right, (float)pos.X,
                (float)(rect.Height - pos.Y), 0, 0));
        }

        public override void ScrollWheel(NSEvent theEvent)
        {
            if (theEvent.ScrollingDeltaX == 0 && theEvent.ScrollingDeltaY == 0)
                return;

            var view = MacWindow.NSWindow!.ContentView;
            var rect = view!.Frame;
            var pos = theEvent.LocationInWindow;

            var e = ScrollEvent.Make((float)pos.X, (float)(rect.Height - pos.Y),
                -(float)theEvent.ScrollingDeltaX, -(float)theEvent.ScrollingDeltaY); //暂方向相反
            MacWindow.OnScroll(e);
        }

        #endregion

        #region ====Keyboard Events====

        public override void KeyDown(NSEvent theEvent)
        {
            //TODO:临时解决文本输入时MouseDown改变NSWindow.FirstResponder
            if (Subviews.Length != 0)
            {
                Window.MakeFirstResponder(Subviews[0]);
                Subviews[0].KeyDown(theEvent);
                //Subviews[0].InterpretKeyEvents(new NSEvent[] { theEvent });
            }
            else
            {
                var keyData =
                    MacKeyCodeMap.ConvertKeyCode(theEvent.KeyCode, theEvent.ModifierFlags);
                //base.KeyDown(theEvent);
                MacWindow.OnKeyDown(KeyEvent.UseDefault(keyData));
            }
        }

        public override void KeyUp(NSEvent theEvent)
        {
            var keyData = MacKeyCodeMap.ConvertKeyCode(theEvent.KeyCode, theEvent.ModifierFlags);
            // base.KeyUp(theEvent);
            MacWindow.OnKeyUp(KeyEvent.UseDefault(keyData));
        }

        #endregion
    }
}