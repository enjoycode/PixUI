// ReSharper disable UnusedMember.Global

using System;
using AppKit;
using CoreGraphics;
using Foundation;
using ObjCRuntime;

namespace PixUI.Platform.Mac
{
    // https://developer.apple.com/library/archive/documentation/TextFonts/Conceptual/CocoaTextArchitecture/TextEditing/TextEditing.html#//apple_ref/doc/uid/TP40009459-CH3

    public sealed class TextInputView : NSView, INSTextInputClient
    {
        private NSString? _markedText;
        private NSRange _markedRange;
        private NSRange _selectedRange;
        private Rect _inputRect = Rect.Empty;

        public override bool IsOpaque => true;

        // public override bool CanBecomeKeyView => true;

        public override bool AcceptsFirstResponder() => true;

        private readonly NSString[] _validAttributesForMarkedText = Array.Empty<NSString>();

        private MacWindow MacWindow => ((MainView)Window.ContentView!).MacWindow;

        public TextInputView() : base(CGRect.Empty) { }

        internal void SetInputRect(Rect rect) => _inputRect = rect;

        public bool HasMarkedText
        {
            [Export("hasMarkedText")] get => _markedText != null;
        }

        public NSRange MarkedRange
        {
            [Export("markedRange")] get => _markedRange;
        }

        public NSRange SelectedRange
        {
            [Export("selectedRange")] get => _selectedRange;
        }

        public NSString[] ValidAttributesForMarkedText
        {
            [Export("validAttributesForMarkedText")]
            get => _validAttributesForMarkedText;
        }

        // public NSWindowLevel WindowLevel
        // {
        //     [Export("windowLevel")] get => NSWindowLevel.Normal;
        // }

        // public nint ConversationIdentifier
        // {
        //     [Export("conversationIdentifier")] get => new nint(Handle.ToInt64());
        // }

        [Export("doCommandBySelector:")]
        public void DoCommand(Selector by)
        {
            switch (by.Name)
            {
                case "deleteBackward:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Back));
                    break;
                case "insertNewline:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Return));
                    break;
                case "insertTab:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Tab));
                    break;
                case "insertBacktab:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Tab | Keys.Shift));
                    break;
                case "moveRight:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Right));
                    break;
                case "moveLeft:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Left));
                    break;
                case "moveUp:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Up));
                    break;
                case "moveDown:":
                    MacWindow.OnKeyDown(KeyEvent.UseDefault(Keys.Down));
                    break;
                default:
                    Console.WriteLine($"DoCommand: {by.Name}");
                    break;
            }
        }

        [Export("attributedSubstringForProposedRange:actualRange:")]
        public NSAttributedString? GetAttributedSubstring(NSRange proposedRange,
            out NSRange actualRange)
        {
            actualRange = proposedRange;
            return null;
        }

        [Export("characterIndexForPoint:")]
        public nuint GetCharacterIndex(CGPoint point) => 0;

        [Export("firstRectForCharacterRange:actualRange:")]
        public CGRect GetFirstRect(NSRange characterRange, out NSRange actualRange)
        {
            actualRange = characterRange;

            var contentRect = Window.ContentRectFor(Window.Frame);
            var windowHeight = contentRect.Size.Height;
            var rect = new CGRect(_inputRect.Left,
                windowHeight - _inputRect.Top - _inputRect.Height,
                _inputRect.Width, _inputRect.Height);
            rect = Window.ConvertRectToScreen(rect);

            return rect;
        }

        [Export("insertText:replacementRange:")]
        public void InsertText(NSObject text, NSRange replacementRange)
        {
            if (text is NSString nsString)
            {
                MacWindow.OnTextInput(nsString);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [Export("setMarkedText:selectedRange:replacementRange:")]
        public void SetMarkedText(NSObject text, NSRange selectedRange,
            NSRange replacementRange)
        {
            Console.WriteLine("SetMarkedText");
        }

        [Export("unmarkText")]
        public void UnmarkText()
        {
            _markedText?.Dispose();
            _markedText = null;
        }

        public override void KeyDown(NSEvent theEvent)
        {
            //临时判断Command键是否按下，用于拦截快捷键
            if ((theEvent.ModifierFlags & NSEventModifierMask.CommandKeyMask) == NSEventModifierMask.CommandKeyMask)
            {
                var keyData = MacKeyCodeMap.ConvertKeyCode(theEvent.KeyCode, theEvent.ModifierFlags);
                //base.KeyDown(theEvent);
                MacWindow.OnKeyDown(KeyEvent.UseDefault(keyData));
                return;
            }

            var consumed = InputContext.HandleEvent(theEvent);
            if (!consumed)
                base.KeyDown(theEvent);
        }
    }
}