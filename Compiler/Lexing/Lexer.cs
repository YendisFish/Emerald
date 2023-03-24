using Emerald.Types;

namespace Emerald.Lexing;

public class Lexer
{
    public int _pos { get; set; }
    public string _essay { get; set; }

    public Lexer(string contents /*The reason it doesn't read the string for you is for JIT purposes)*/)
    {
        _essay = contents; //we set the essay to the contents of the read file
        _pos = 0; //set the position to 0 to prepare for syntactical analysis
    }

    public void Parse(out Token[] tokenarr)
    {
        List<Token> tokens = new();
        string current_word = "";

        Token cur_t = new Token(TokenType.WORD, new int[0], "");

        while(_pos < _essay.Length)
        {
            /*
                The switch here is the real dark magic! Speeds
                up the program a LOT! And we need speed while
                tokenizing! It comes with a lot of string parsing
                though.
            */

            //match on separator chars
            switch(_essay[_pos])
            {
                case ' ':
                {
                    if(cur_t._value != "" && cur_t._tp == TokenType.WORD)
                    {
                        tokens.Add(cur_t);
                        cur_t = new Token(TokenType.WORD, new int[0], "");
                    }

                    cur_t = new Token(TokenType.WORD, new int[0], "");

                    Token newtok = new Token(TokenType.WHITESPACE, new int[1] { _pos }, " ");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case ',':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.COMMA, new int[1] { _pos }, ",");
                    tokens.Add(newtok);

                    _pos = _pos + 1;

                    continue;
                }

                case '.':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.DOT, new int[1] { _pos }, ".");

                    _pos = _pos + 1;
                    continue;
                }

                case '(':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.LPAREN, new int[1] { _pos }, "(");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case ')':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.RPAREN, new int[1] { _pos }, ")");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }
                
                case '[':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.LBRACKET, new int[1] { _pos }, "[");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case ']':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.RBRACKET, new int[1] { _pos }, "]");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }
                
                case '{':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.LCBRACE, new int[1] { _pos }, "{");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case '}':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.RCBRACE, new int[1] { _pos }, "}");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case '\"':
                {
                    tokens.Add(cur_t);

                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.STRINDICATOR, new int[1] { _pos }, "\"");
                    tokens.Add(newtok);

                    Token intern = new Token(TokenType.STRING, new int[0], "");

                    while(true)
                    {
                        int[] newp = new int[intern._positions.Length + 1];
                        for(int i = 0; i < newp.Length; i++)
                        {
                            if(_essay[_pos] >= intern._positions.Length)
                            {
                                newp[newp.Length - 1] = _pos;
                                break;
                            }

                            newp[i] = intern._positions[i];
                        }

                        if(_essay[_pos] == '\"')
                        {
                            tokens.Add(intern);
                            //tokens.Add(new Token(TokenType.STRINDICATOR, new int[1] { _pos }, "\"")); IDK WHY THIS WORKS!!!
                            break;
                        }

                        intern._value = intern._value + _essay[_pos];
                        _pos = _pos + 1;
                    }

                    _pos = _pos + 1;
                    continue;
                }

                case '\'':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.CHARINDICATOR, new int[1] { _pos }, "\'");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    tokens.Add(new Token(TokenType.CHAR, new int[1] { _pos }, "" + _essay[_pos]));

                    _pos = _pos + 1;
                    tokens.Add(new Token(TokenType.STRINDICATOR, new int[1] { _pos }, "\'"));

                    _pos = _pos + 1;
                    continue;
                }

                case ';':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.SEMICOLON, new int[1] { _pos }, ";");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                //we must also switch on operators
                case '+':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "+");
                    tokens.Add(newtok);

                    _pos = _pos + 1;
                    continue;
                }

                case '-':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "-");
                    tokens.Add(newtok);
                    
                    _pos = _pos + 1;
                    continue;
                }

                case '=':
                {
                    if(_essay[_pos + 1] != '=')
                    {
                        tokens.Add(cur_t);
                        cur_t = new Token((TokenType)0, new int[0], "");

                        Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "=");
                        tokens.Add(newtok);
                        
                        _pos = _pos + 1;
                        continue;
                    }

                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok2 = new Token(TokenType.OPERATOR, new int[1] { _pos }, "==");
                    tokens.Add(newtok2);
                        
                    _pos = _pos + 2;
                    continue;
                }

                case '%':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "%");
                    tokens.Add(newtok);
                    
                    _pos = _pos + 1;
                    continue;
                }

                case '/':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "/");
                    tokens.Add(newtok);
                    
                    _pos = _pos + 1;
                    continue;
                }

                case '*':
                {
                    tokens.Add(cur_t);
                    cur_t = new Token((TokenType)0, new int[0], "");

                    Token newtok = new Token(TokenType.OPERATOR, new int[1] { _pos }, "*");
                    tokens.Add(newtok);
                    
                    _pos = _pos + 1;
                    continue;
                }
            }

            cur_t._tp = TokenType.WORD;

            int[] places = new int[cur_t._positions.Length + 1];
            for(int i = 0; i < places.Length; i++)
            {
                if(i >= cur_t._positions.Length)
                {
                    places[places.Length - 1] = _pos;
                    break;
                }

                places[i] = cur_t._positions[i];
            }
            cur_t._positions = places;

            cur_t._value = cur_t._value + _essay[_pos];

            current_word = current_word + _essay[_pos];
            _pos = _pos + 1;
        }

        tokenarr = tokens.ToArray();
    }
}