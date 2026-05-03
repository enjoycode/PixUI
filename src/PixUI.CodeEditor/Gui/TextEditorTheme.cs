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

    public IPaint BracketHighlightPaint = Paint.Create(new Color(255, 255, 0, 100));

    public Color LineNumberColor = new(0xFF606366);

    public ITextStyle TextStyle = PixUI.TextStyle.Create(0xFFDCDCDC, 1f);

    public ITextStyle FoldedTextStyle = PixUI.TextStyle.Create(0xFFA9B7C7, 1f);

    #region ====Token TextStyle====

    private ITextStyle _tokenErrorStyle = PixUI.TextStyle.Create(Colors.Red, 1);

    private ITextStyle _tokenTypeStyle = PixUI.TextStyle.Create(0xFF67DBF1, 1);

    private ITextStyle _tokenNumberStyle = PixUI.TextStyle.Create(0xFF6996BD, 1);

    private ITextStyle _tokenStringStyle = PixUI.TextStyle.Create(0xFF98C379, 1);

    private ITextStyle _tokenKeywordStyle = PixUI.TextStyle.Create(0xFFCC7927, 1);

    private ITextStyle _tokenCommentStyle = PixUI.TextStyle.Create(0xFF5F984F, 1);

    private ITextStyle _tokenVariableStyle = PixUI.TextStyle.Create(0xFFDCDCDC, 1);

    private ITextStyle _tokenFunctionStyle = PixUI.TextStyle.Create(0xFFFFC763, 1);

    /// <summary>
    /// 根据TokenType获取缓存的TextStyle
    /// </summary>
    /// <returns>Don't dispose result</returns>
    public ITextStyle GetTokenStyle(TokenType tokenType) =>
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