using Emerald.Parsing;
using Emerald.Lexing;
using Emerald.Types;
using Emerald.AST;

Token[] toks = new Token[] {};

Lexer lex = new("ruleset; void Main(int arglen, char **args) { char ptr = 'a'; print(ptr) }"); //i dont want to write any external files rn
lex.Parse(out toks); //this should operate pretty quickly due to the pointer

List<Token> nowhitespace = new();

foreach(Token t in toks)
{
    if(t._tp == TokenType.WHITESPACE || t._value.Length == 0)
    {
        continue;
    }

    nowhitespace.Add(t);
}

foreach(Token t in nowhitespace)
{
    Console.WriteLine($"Type: {t._tp} | Value: {t._value}");
}

Console.WriteLine("------------------------------------------------------------");

Parser p = new Parser(ref nowhitespace);

AbstractSyntaxTree tree = p.Parse();
foreach(ASTNode node in tree.nodes)
{
    if(node.GetType() == typeof(RulesetNode))
    {
        RulesetNode ruleset = (RulesetNode)node;
        Console.WriteLine("Ruleset Node:");
        Console.WriteLine($"GC: {ruleset.rungc}");
        Console.WriteLine($"Stack Preferred: {ruleset.stackPreferred}");
        Console.WriteLine($"Heap Preferred: {ruleset.heapPreferred}");
    }

    if(node.GetType() == typeof(FunctionDeclarationNode))
    {
        FunctionDeclarationNode func = (FunctionDeclarationNode)node;
        Console.WriteLine("Function Declaration:");
        Console.WriteLine($"Return Type: {func.returnType}");
        Console.WriteLine($"Name: {func.name}");
        Console.WriteLine($"Function Params - ");
        foreach(FunctionParameterNode param in func.parameters)
        {
            Console.Write($"TP: {param.type} NM: {param.name} | ");
        }
        Console.WriteLine("\nFunction Body: ");
        foreach(ASTNode funcMember in func.body)
        {
            VarDeclarationNode varNode = (VarDeclarationNode)funcMember;
            Console.WriteLine($"TP: {varNode.type} | NM: {varNode.name}");
        }
    }
}

Console.WriteLine("------------------------------------------------------------");