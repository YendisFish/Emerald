using Emerald.Parsing;
using Emerald.Lexing;
using Emerald.Types;
using Emerald.AST;

Token[] toks = new Token[] {};

Lexer lex = new("namespace HelloWorld { ruleset { selfalloc; stackpreferred; } void Main(string[] args) { char *ptr = \"Hello World!\"; print(*ptr) } }"); //i dont want to write any external files rn
lex.Parse(out toks); //this should operate pretty quickly due to the pointer

try
{
    foreach(Token t in toks.Where(x => x._value.Length > 0))
    {
        if(t._tp != TokenType.WHITESPACE)
        {
            Console.WriteLine($"Type = {Convert.ToString(t._tp)} | Pos {t._positions[0] + 1} - {t._positions[t._positions.Length - 1] + 1} | Value = {t._value}");
        }
    }
} catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

Console.WriteLine("------------------------------------------------------------");