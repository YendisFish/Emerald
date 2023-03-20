using Emerald.Parsing;
using Emerald.Lexing;
using Emerald.Types;
using Emerald.AST;

Token[] toks = new Token[] {};

Lexer lex = new("namespace HelloWorld { ruleset { selfalloc; } void Main() { char *ptr = \"Hello World\"!; print(*ptr) } }"); //i dont want to write any external files rn
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

Console.WriteLine("------------------------------------------------------------");

AbstractSyntaxTree tree = new();

//in order to prevent errors with whitespace we will remove all whitespace tokens since they are redundant
Token[] ntoks = toks.Where(x => x._tp != TokenType.WHITESPACE).ToArray();

Parser p = new Parser(in ntoks);
p.Parse(out tree);

foreach(ASTNode node in tree)
{
    if(node.type == ASTNodeType.NAMESPACE_DECL)
    {
        Console.WriteLine(((ASTNamespaceDeclaration)node).name);

        ASTNamespaceDeclaration decl = ((ASTNamespaceDeclaration)node);

        if(decl.ruleset != null)
        {
            Console.WriteLine("Found ruleset!");
        }
    }
}