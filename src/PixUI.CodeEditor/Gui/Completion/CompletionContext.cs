using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PixUI;

namespace CodeEditor
{
    internal sealed class CompletionContext
    {
        private const int StateIdle = 0;
        private const int StateShow = 1;
        private const int StateSuspendHide = 2;

        private readonly CodeEditorController _controller;
        private readonly ICompletionProvider? _provider;

        private int _completionStartOffset = -1; //开始自动完成时的光标位置
        private bool _startByTriggerChar; //是否由TriggerChar触发的
        private ListPopup<ICompletionItem>? _completionWindow;
        private int _state = StateIdle; //当前状态

        public CompletionContext(CodeEditorController controller, ICompletionProvider? provider)
        {
            _controller = controller;
            _provider = provider;
        }

        public void RunCompletion(string value)
        {
            if (_provider == null) return;

            //Get word at caret position (end with caret position)
            var word = GetWordAtPosition(_controller.TextEditor.Caret.Position);

            //是否已经显示
            if (_state == StateShow)
            {
                if (word == null) //if word is null, hide completion window
                {
                    HideCompletionWindow();
                }
                else
                {
                    UpdateFilter();
                    return;
                }
            }

            //开始显示
            if (word != null)
            {
                _completionStartOffset = word.Value.Offset;
                _startByTriggerChar = false;
                _state = StateShow;
                RunInternal(word.Value.Word);
            }
            else
            {
                var triggerChar = value[value.Length - 1];
                if (_provider.TriggerCharacters.Contains(triggerChar))
                {
                    _completionStartOffset = _controller.TextEditor.Caret.Offset;
                    _startByTriggerChar = true;
                    _state = StateShow;
                    RunInternal(string.Empty);
                }
            }
        }

#if __WEB__
        public async Task RunInternal(string filter)
#else
        private void RunInternal(string filter)
#endif
        {
#if __WEB__
                var items = await _provider!.ProvideCompletionItems(_controller.Document,
                        _controller.TextEditor.Caret.Offset, filter);
                ShowCompletionWindow(items, "");
#else
            Task.Run(async () =>
            {
                var items = await _provider!.ProvideCompletionItems(_controller.Document,
                    _controller.TextEditor.Caret.Offset, filter);
                UIApplication.Current.BeginInvoke(() => ShowCompletionWindow(items, filter));
            });
#endif
        }

        private CompletionWord? GetWordAtPosition(in TextLocation pos)
        {
            var lineSegment = _controller.Document.GetLineSegment(pos.Line);
            var token = lineSegment.GetTokenAt(pos.Column);
            if (token == null) return null;

            //排除无需提示的Token类型
            var tokenType = token.Value.Type;
            if (tokenType == TokenType.Comment || tokenType == TokenType.Constant ||
                tokenType == TokenType.LiteralNumber || tokenType == TokenType.LiteralString ||
                tokenType == TokenType.PunctuationBracket ||
                tokenType == TokenType.PunctuationDelimiter ||
                tokenType == TokenType.WhiteSpace || tokenType == TokenType.Operator)
                return null;

            var tokenStartColumn = token.Value.StartColumn;
            var len = pos.Column - tokenStartColumn;
            if (len <= 0) return null;
            var offset = lineSegment.Offset + tokenStartColumn;
            var tokenWord = _controller.Document.GetText(offset, len);
            return new CompletionWord(offset, tokenWord);
        }

        private void ShowCompletionWindow(IList<ICompletionItem>? list, string filter)
        {
            if (list == null || list.Count == 0)
            {
                _state = StateIdle;
                return;
            }

            if (_completionWindow == null)
            {
                _completionWindow = new ListPopup<ICompletionItem>(_controller.Widget.Overlay!,
                    BuildPopupItem, 250, 18, 8);
                _completionWindow.OnSelectionChanged = OnCompletionDone;
            }

            _completionWindow.DataSource = list;
            _completionWindow.TrySelectFirst();
            //TODO: set filter
            var caret = _controller.TextEditor.Caret;
            var lineHeight = _controller.TextEditor.TextView.FontHeight;
            var pt2Win = _controller.Widget.LocalToWindow(0, 0);
            _completionWindow.UpdatePosition(caret.CanvasPosX + pt2Win.X - 8,
                caret.CanvasPosY + lineHeight + pt2Win.Y);
            _completionWindow.Show();
        }

        private void HideCompletionWindow()
        {
            _completionWindow?.Hide();
            _state = StateIdle;
        }

        private void UpdateFilter()
        {
            var filter = _controller.Document.GetText(_completionStartOffset,
                _controller.TextEditor.Caret.Offset - _completionStartOffset);
            _completionWindow?.UpdateFilter(t => t.Label.StartsWith(filter));
            _completionWindow?.TrySelectFirst();
        }

        private void ClearFilter()
        {
            _completionWindow?.ClearFilter();
            _completionWindow?.TrySelectFirst();
        }

        /// <summary>
        /// 非文本输入激发的光标位置变更
        /// </summary>
        internal void OnCaretChangedByNoneTextInput()
        {
            if (_state != StateSuspendHide)
            {
                HideCompletionWindow();
                return;
            }

            //由后退键触发的
            var caret = _controller.TextEditor.Caret;
            if (caret.Offset <= _completionStartOffset)
            {
                if (caret.Offset == _completionStartOffset && _startByTriggerChar)
                {
                    _state = StateShow;
                    ClearFilter();
                }
                else
                {
                    HideCompletionWindow();
                }
            }
            else
            {
                _state = StateShow;
                UpdateFilter();
            }
        }

        internal void PreProcessKeyDown(KeyEvent e)
        {
            if (_state == StateShow)
            {
                if (e.KeyCode == Keys.Back)
                    _state = StateSuspendHide;
            }
        }

        private void OnCompletionDone(ICompletionItem? item)
        {
            HideCompletionWindow();

            if (item == null) return;

            _controller.TextEditor.InsertOrReplaceString(item.InsertText ?? item.Label,
                _controller.TextEditor.Caret.Offset - _completionStartOffset);
        }

        private static Widget BuildPopupItem(ICompletionItem item, int index, State<bool> isHover,
            State<bool> isSelected)
            => new CompletionItemWidget(item, isSelected);
    }
}