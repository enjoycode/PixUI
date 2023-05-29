using System;
using PixUI;

namespace CodeEditor
{
    public sealed class TextEditorTheme
    {
        public float FontSize = 15;

        /// <summary>
        /// 行间距
        /// </summary>
        public float LineSpace = 2;

        public Color CaretColor = Colors.Red;

        /// <summary>
        /// 高亮光标所在行(背景色)
        /// </summary>
        public Color LineHighlightColor = new Color(150, 150, 150, 20);

        /// <summary>
        /// 选择的文本(背景色)
        /// </summary>
        public Color SelectionColor = new Color(167, 209, 255, 50);

        public Color TextBgColor = new Color(0xFF2B2B2B);

        public Color LineBgColor = new Color(0xFF313335);

        public Paint BracketHighlightPaint = new Paint() { Color = new Color(255, 255, 0, 100) };

        public Color LineNumberColor = new Color(0xFF606366);

        public TextStyle TextStyle = new TextStyle() { Color = new Color(0xFFA9B7C7), Height = 1 };

        public TextStyle FoldedTextStyle = new TextStyle()
        {
            Color = new Color(0xFFA9B7C7),
            Height = 1,
            //LetterSpacing = 1.5,
            //Background = new Paint() {Color=new Color(0xFF3A3A3A)}
        };

        private TextStyle _tokenErrorStyle = new TextStyle()
            { Color = Colors.Red, Height = 1 };

        private TextStyle _tokenTypeStyle = new TextStyle()
            { Color = new Color(0xFF67DBF1), Height = 1 };

        private TextStyle _tokenNumberStyle = new TextStyle()
            { Color = new Color(0xFF6996BD), Height = 1 };

        private TextStyle _tokenStringStyle = new TextStyle()
            { Color = new Color(0xFF98C379), Height = 1 };

        private TextStyle _tokenKeywordStyle = new TextStyle()
            { Color = new Color(0xFFCC7927), Height = 1 };

        private TextStyle _tokenCommentStyle = new TextStyle()
            { Color = new Color(0xFF5F984F), Height = 1 };

        private TextStyle _tokenVariableStyle = new TextStyle()
            { Color = new Color(0xFFE06C75), Height = 1 };

        private TextStyle _tokenFunctionStyle = new TextStyle()
            { Color = new Color(0xFFFFC763), Height = 1 };

        /// <summary>
        /// 根据TokenType获取缓存的TextStyle
        /// </summary>
        /// <returns>Don't dispose result</returns>
        public TextStyle GetTokenStyle(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Error:
                    return _tokenErrorStyle;
                case TokenType.Type:
                    return _tokenTypeStyle;
                case TokenType.BuiltinType:
                    return _tokenTypeStyle;
                case TokenType.LiteralNumber:
                    return _tokenNumberStyle;
                case TokenType.LiteralString:
                    return _tokenStringStyle;
                case TokenType.Constant:
                case TokenType.Keyword:
                    return _tokenKeywordStyle;
                case TokenType.Comment:
                    return _tokenCommentStyle;
                case TokenType.Variable:
                    return _tokenVariableStyle;
                case TokenType.Function:
                    return _tokenFunctionStyle;
                default:
                    return TextStyle;
            }
        }
    }
}