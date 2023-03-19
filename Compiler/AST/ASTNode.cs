using Emerald.Types;

namespace Emerald.AST;

/*
    I usually do not support inheritence,
    however, here it just makes sense.
*/

public class ASTNode
{
    public ASTNodeType type { get; set; }
}

public class ASTBinExpression : ASTNode
{
    public Operator op { get; set; }
    public Tuple<ASTNode, ASTNode>? vars { get; set; }
}

public class ASTUnExpression : ASTNode
{
    public Operator op { get; set; }
    public ASTNode? var { get; set; }
}

public class ASTDeclaration : ASTNode
{
    public string? name { get; set; }
}

public class ASTVariable : ASTNode
{
    public string? name { get; set; }
    public ASTNode? value { get; set; }
}

public class ASTValue : ASTNode
{
    public object? obj { get; set; }
    public VarType tp { get; set; }
}

public class ASTCall : ASTNode
{
    public ASTNode? method { get; set; }
    public List<ASTNode>? args { get; set; }
}

public class ASTReference : ASTNode
{
    public ASTNode? var { get; set; }
}

public class ASTDereference : ASTNode
{
    public ASTNode? reference { get; set; }
}

public class ASTRulesetInstruction : ASTNode
{
    public ASTNode? ruleset { get; set; }
    public Keywords keyword { get; set; }
}

public class ASTRulesetDeclaration : ASTNode
{
    public ASTNode? nspace { get; set; }
    public List<ASTNode>? rules { get; set; }
}

public class ASTNamespaceDeclaration : ASTNode
{
    public string? name { get; set; }
    public ASTNode? ruleset { get; set; }
    public List<ASTNode>? methods { get; set; }
}

public class ASTMethodDeclaration : ASTNode
{
    public string? name { get; set; }
    public ASTNode? args { get; set; }
    public ASTNode? nspace { get; set; }
    public ASTNode? nodes { get; set; }
}

public class ASTConst : ASTNode
{
    public object? value { get; set; }
    public VarType tp { get; set; }
}

public class ASTIf : ASTNode
{
    public ASTNode? method { get; set; }
    public ASTNode? comp { get; set; }
}

public class ASTWhile : ASTNode
{
    public ASTNode? method { get; set; }
    public ASTNode? comp { get; set; }
}

public class ASTFor : ASTNode
{
    public ASTNode? method { get; set; }
    public ASTNode? init { get; set; }
    public ASTNode? comp { get; set; }
    public ASTNode? expr { get; set; }
}

public enum ASTNodeType
{
    NAMESPACE_DECL,
    RULESET_DECL,
    RULESET_INSTRUCTION,
    METHOD_DECL,
    BIN_EXPRESSION,
    UN_EXPRESSION,
    DECLARATION,
    REFERENCE,
    DEREFERENCE,
    VARIABLE,
    VALUE,
    CONST,
    IF,
    WHILE,
    FOR,
    CALL
}