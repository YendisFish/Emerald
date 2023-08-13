using System.Net.Http.Headers;
using Emerald.Types;

namespace Emerald.AST;

public class ASTNode { }

public class ExpressionNode : ASTNode { }

public class StatementNode : ASTNode 
{ 
    public ExpressionNode baseExpression { get; set; } = new();
}

public class ImportNode : ASTNode
{
    public string importPath { get; set; } = "";
}

public class RulesetNode : ASTNode
{
    public bool rungc { get; set; } = false;
    public bool stackPreferred { get; set; } = false;
    public bool heapPreferred { get; set; } = false;
}

public class ConstantNode : ASTNode
{
    public string name { get; set; } = "";
    public string type { get; set; } = "";
    public object value { get; set; } = "";
}

#region Classes

public class ClassNode : ASTNode
{
    public string typename { get; set; } = "";
    public bool isGeneric { get; set; } = false;
    public ConstructorNode constructor { get; set; } = new();
}

public class ConstructorNode : ASTNode
{
    public bool isdefault { get; set; } = false;
    public List<ASTNode> body { get; set; } = new();
}

#endregion

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
    public AssignmentNode assignment { get; set; } = new();
}

//use the baseExpression value to set this value
public class AssignmentNode : StatementNode
{
    public VariableExpressionNode node { get; set; } = new();
    public ExpressionNode value { get; set; } = new();
}

public class IfStatementNode : StatementNode
{
    public ExpressionNode condition { get; set; } = new();
    public List<ASTNode> body { get; set; } = new();
}

public class ForLoopNode : StatementNode
{
    public AssignmentNode initialization { get; set; } = new();
    public ExpressionNode condition { get; set; } = new();
    public AssignmentNode iteration { get; set; } = new();
    public List<ASTNode> body { get; set; } = new();
}

public class SwitchNode : StatementNode
{
    public VarDeclarationNode var { get; set; } = new();
    public List<SwitchCaseNode> cases { get; set; } = new();
}

public class SwitchCaseNode : StatementNode
{
    public BinaryExpressionNode condition { get; set; } = new();
    public List<ASTNode> body { get; set; } = new();
    public bool isfallback { get; set; } = new();
}

public class WhileLoopNode : StatementNode
{
    public ExpressionNode condition { get; set; } = new();
    public List<ASTNode> body { get; set; } = new();
}

public class ReturnStatementNode : StatementNode
{
    public ExpressionNode value { get; set; } = new();
}

public class InlineAssemblyNode : StatementNode
{
    public string asm { get; set; } = "";
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
    public string varName { get; set; } = "";
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
    public ExpressionNode reference { get; set; } = new();
}

public class BinaryExpressionNode : ExpressionNode
{
    public ExpressionNode left { get; set; } = new();
    public ExpressionNode right { get; set; } = new();
    public string Operator { get; set; } = "";
}

#endregion