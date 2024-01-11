using System;
using PixUI;

namespace CodeEditor;

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
    public Color LineHighlightColor = new(150, 150, 150, 20);

    /// <summary>
    /// 选择的文本(背景色)
    /// </summary>
    public Color SelectionColor = new(167, 209, 255, 50);

    public Color TextBgColor = new(0xFF2B2B2B);

    public Color LineBgColor = new(0xFF313335);

    public Paint BracketHighlightPaint = new() { Color = new Color(255, 255, 0, 100) };

    public Color LineNumberColor = new(0xFF606366);

    public TextStyle TextStyle = new() { Color = new Color(0xFFDCDCDC), Height = 1 };

    public TextStyle FoldedTextStyle = new()
    {
        Color = new Color(0xFFA9B7C7),
        Height = 1,
        //LetterSpacing = 1.5,
        //Background = new Paint() {Color=new Color(0xFF3A3A3A)}
    };

    #region ====Token TextStyle====

    private TextStyle _tokenErrorStyle = new() { Color = Colors.Red, Height = 1 };

    private TextStyle _tokenTypeStyle = new() { Color = 0xFF67DBF1, Height = 1 };

    private TextStyle _tokenNumberStyle = new() { Color = 0xFF6996BD, Height = 1 };

    private TextStyle _tokenStringStyle = new() { Color = 0xFF98C379, Height = 1 };

    private TextStyle _tokenKeywordStyle = new() { Color = 0xFFCC7927, Height = 1 };

    private TextStyle _tokenCommentStyle = new() { Color = 0xFF5F984F, Height = 1 };

    private TextStyle _tokenVariableStyle = new() { Color = 0xFFDCDCDC, Height = 1 };

    private TextStyle _tokenFunctionStyle = new() { Color = 0xFFFFC763, Height = 1 };

    /// <summary>
    /// 根据TokenType获取缓存的TextStyle
    /// </summary>
    /// <returns>Don't dispose result</returns>
    public TextStyle GetTokenStyle(TokenType tokenType) =>
        tokenType switch
        {
            TokenType.Error => _tokenErrorStyle,
            TokenType.Type => _tokenTypeStyle,
            TokenType.BuiltinType => _tokenTypeStyle,
            TokenType.LiteralNumber => _tokenNumberStyle,
            TokenType.LiteralString => _tokenStringStyle,
            TokenType.Constant => _tokenKeywordStyle,
            TokenType.Keyword => _tokenKeywordStyle,
            TokenType.Comment => _tokenCommentStyle,
            TokenType.Variable => _tokenVariableStyle,
            TokenType.Function => _tokenFunctionStyle,
            _ => TextStyle
        };

    #endregion
}