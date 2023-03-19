using Emerald.Lexing;
using Emerald.Types;

Token[] toks = new Token[] {};

Lexer lex = new("namespace HelloWorld { ruleset { stackprefferred; } void Main() { char *ptr = \"Hello World\"!; print(*ptr) } }"); //i dont want to write any external files rn
lex.Parse(out toks); //this should operate pretty quickly due to the pointer

try
{
    foreach(Token t in toks.Where(x => x._value.Length > 0))
    {
        Console.WriteLine($"Type = {Convert.ToString(t._tp)} | Pos {t._positions[0] + 1} - {t._positions[t._positions.Length - 1] + 1} | Value = {t._value}");
    }
} catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
