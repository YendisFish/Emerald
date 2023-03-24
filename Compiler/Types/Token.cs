namespace Emerald.Types;

public /*record*/ class Token
{
    public TokenType _tp { get; set; }
    public int[] _positions { get; set; }
    public string _value { get; set; }

    public Token(TokenType tp, int[] positions, string value)
    {
        _tp = tp;
        _positions = positions;
        _value = value;
    }
}

public enum TokenType
{
    OPERATOR, // * can be a pointer operator OR a multiplication operator
    WORD,
    RPAREN,
    LPAREN,
    RBRACKET,
    LBRACKET,
    STRINDICATOR,
    CHARINDICATOR,
    COMPILERINDICATOR,
    LCBRACE,
    RCBRACE,
    SEMICOLON,
    WHITESPACE,
    STRING,
    CHAR,
    COMMA,
    DOT
}