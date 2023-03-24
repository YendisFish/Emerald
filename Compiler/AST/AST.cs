namespace Emerald.AST;

public class AbstractSyntaxTree : List<ASTNode>
{
    public List<ASTNode> rootNodes { get; set; } = new(); //this is almost always expected to be a namespace
    public int Length => rootNodes.Count;

    public new ASTNode this[int index]
    {
        get {
            return rootNodes[index];
        }

        set {
            rootNodes[index] = value;
        }
    }

    public new IEnumerator<ASTNode> GetEnumerator()
    {
        return rootNodes.GetEnumerator();
    }

    public void AddRoot(ASTNode node)
    {
        rootNodes.Add(node);
    }

    public void SearchFor(ref ASTNode nd)
    {
        foreach(ASTNode node in this)
        {

        }
    }

    public void SearchClass() { }
    public void SearchMethod() { }
}