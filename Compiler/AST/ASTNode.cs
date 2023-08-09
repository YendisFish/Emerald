using Emerald.Types;

namespace Emerald.AST;

public abstract class ASTNode { }

public abstract class ExpressionNode : ASTNode { }

public abstract class StatementNode : ASTNode 
{ 
    public ExpressionNode baseExpression { get; set; }
}

public class NamespaceNode : ASTNode
{
    public string name { get; set; } = "";
    public RulesetNode ruleset { get; set; }
}

public class RulesetNode : ASTNode
{
    public bool selfAlloc { get; set; } = false;
    public bool stackPreferred { get; set; } = false;
    public bool heapPreferred { get; set; } = false;
}

#region Functions

public class FunctionDeclarationNode : ASTNode
{
    public string returnType { get; set; } = "";
    public string name { get; set; } = "";
    public List<FunctionParameterNode> parameters { get; set; } = new();
    public List<ASTNode> body { get; set; } = new();
}

public class FunctionParameterNode : ASTNode
{
    public string type { get; set; } = "";
    public string name { get; set; } = "";
}

#endregion

#region Statements

public class VarDeclarationNode : StatementNode
{
    public string type { get; set; } = "";
    public string name { get; set; } = "";
    public AssignmentNode assignment { get; set; }
}

//use the baseExpression value to set this value
public class AssignmentNode : StatementNode
{
    public VariableExpressionNode node { get; set; }
    public ExpressionNode value { get; set; }
}

public class IfStatementNode : StatementNode
{
    public ExpressionNode condition { get; set; }
    public List<ASTNode> body { get; set; } = new();
}

public class ForLoopNode : StatementNode
{
    public AssignmentNode initialization { get; set; }
    public ExpressionNode condition { get; set; }
    public AssignmentNode iteration { get; set; }
    public List<ASTNode> body { get; set; } = new();
}

public class WhileLoopNode : StatementNode
{
    public ExpressionNode condition { get; set; }
    public List<ASTNode> body { get; set; }
}

public class ReturnStatementNode : StatementNode
{
    public ExpressionNode value { get; set; }
}

public class InlineAssemblyNode : StatementNode
{
    public string asm { get; set; }
    public Dictionary<string, string> variableMappings { get; set; } = new();
}

#endregion

#region Expressions

public class TextValueNode : ExpressionNode
{
    public object? value { get; set; }
    public string type { get; set; } = "";
}

public class VariableExpressionNode : ExpressionNode
{
    public string varName { get; set; }
    public string type { get; set; } = "";
}

public class MethodCallNode : ExpressionNode
{
    public string functionName { get; set; } = "";
    public List<ExpressionNode> arguments { get; set; } = new();
}

public class IncrementNode : ExpressionNode
{
    public string varName { get; set; } = "";
}

public class DecrementNode : ExpressionNode
{
    public string varName { get; set; } = "";
}

public class LogicalNotNode : ExpressionNode
{
    public ExpressionNode reference { get; set; }
}

public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode left { get; set; }
    public ExpressionNode right { get; set; }
    public string Operator { get; set; } = "";
}

#endregion