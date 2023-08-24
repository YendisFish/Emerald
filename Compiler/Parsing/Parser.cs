using Emerald.Types;
using Emerald.AST;
using System.Runtime.InteropServices;
using System.Data.Common;

namespace Emerald.Parsing;

public class Parser
{
    public List<Token> toks { get; set; } = new();

    public Parser(ref List<Token> tokens)
    {
        toks = tokens;
    }

    public AbstractSyntaxTree Parse()
    {
        AbstractSyntaxTree tree = new AbstractSyntaxTree();

        tree.nodes.Add(ParseRuleset());

        List<StructDeclarationNode> structs = ParseStructs();
        tree.nodes.AddRange(structs);

        List<FunctionDeclarationNode> nodes = ParseFunctionDeclarations();
        for (int i = 0; i < nodes.Count; i++)
        {
            //ParseFunctionBody(nodes[i]);
            tree.nodes.Add(nodes[i]);
        }

        return tree;
    }

    public List<StructDeclarationNode> ParseStructs()
    {
        List<StructDeclarationNode> structs = new();

        for (int i = 0; i < toks.Count; i++)
        {
            if (toks[i]._value == "struct")
            {
                StructDeclarationNode node = ParseStruct(i);
                structs.Add(node);
            }
        }

        return structs;
    }

    public StructDeclarationNode ParseStruct(int start)
    {
        StructDeclarationNode node = new();
        //parse the boilerplate of the struct

        start = start + 1;
        node.structName = toks[start]._value;
        start = start + 1;

        if (toks[start]._value == "<")
        {
            node.canBeGeneric = true;

            List<Token> n = new();
            start = FindMatchingClosingVBracket(start, ref n);

            foreach (Token val in n)
            {
                if (val._tp == TokenType.WORD)
                {
                    node.genericNames.Add(val._value);
                }
            }

            //parse n into its generic types
        }

        int end = FindMatchingClosingCBracket(start - 1);

        for (; start <= end; start++)
        {
            if (toks[start]._tp == TokenType.WORD && toks[start + 1]._tp == TokenType.WORD || toks[start]._tp == TokenType.WORD && toks[start + 1]._tp == TokenType.OPERATOR)
            {
                //parse a struct field
                StructFieldNode field = new();
                start = ParseFieldDeclaration(start, ref field);
                node.vars.Add(field);
            }

            if (toks[start]._tp == TokenType.WORD && toks[start + 1]._tp == TokenType.LPAREN)
            {
                //parse struct constructor
            }
        }

        return node;
    }

    public int ParseFieldDeclaration(int start, ref StructFieldNode node)
    {
        List<Token> field = new();
        int end = FindExpressionEnd(start, out field);

        node.type = toks[start]._value;
        start++;

        for (; start < end; start++)
        {
            if (toks[start]._tp == TokenType.OPERATOR && toks[start]._value == "*")
            {
                node.type = node.type + "*";
            }
            else if (toks[start]._tp == TokenType.OPERATOR && toks[start]._value == "<")
            {
                //take care of generic type
            }
            else
            {
                node.name = toks[start]._value;
                break;
            }
        }

        return start;
    }

    public RulesetNode ParseRuleset()
    {
        RulesetNode ruleset = new RulesetNode();

        for (int i = 0; i < toks.Count; i++)
        {
            if (toks[i]._tp == TokenType.WORD && toks[i]._value == "ruleset")
            {
                for (bool search = true; search != false; i++)
                {
                    switch (toks[i]._tp)
                    {
                        case TokenType.SEMICOLON:
                            {
                                if (toks[i - 1]._value == "ruleset")
                                {
                                    ruleset.rungc = true;
                                    ruleset.stackPreferred = true;
                                    ruleset.heapPreferred = false;

                                    search = false;
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }

                        case TokenType.WORD:
                            {
                                if (toks[i]._value == "heappreferred")
                                {
                                    ruleset.heapPreferred = true;
                                }

                                if (toks[i]._value == "stackpreferred")
                                {
                                    ruleset.stackPreferred = true;
                                }

                                if (toks[i]._value == "rungc")
                                {
                                    ruleset.rungc = true;
                                }

                                continue;
                            }

                        case TokenType.RCBRACE:
                            {
                                search = false;
                                break;
                            }
                    }
                }

                break;
            }
            else
            {
                throw new Exception("Could not parse ruleset! It looks like you are missing a semicolon or braces!");
            }
        }

        return ruleset;
    }

    public List<FunctionDeclarationNode> ParseFunctionDeclarations()
    {
        List<FunctionDeclarationNode> nodes = new();

        for (int i = 0; i < toks.Count; i++)
        {
            if (toks[i]._tp == TokenType.WORD && toks[i + 1]._tp == TokenType.WORD && toks[i + 2]._tp == TokenType.LPAREN ||
            toks[i]._tp == TokenType.WORD && toks[i + 1]._tp == TokenType.OPERATOR && toks[FindOperatorChainEnd(i + 1)]._tp == TokenType.WORD && toks[FindOperatorChainEnd(i + 1) + 1]._tp == TokenType.LPAREN)
            {
                FunctionDeclarationNode func = new FunctionDeclarationNode();
                func.returnType = toks[i]._value;

                i++;

                int numPtrs = FindOperatorChainEnd(i) - i;

                for (; numPtrs > 0; numPtrs--)
                {
                    func.returnType = func.returnType + "*";
                }

                i = FindOperatorChainEnd(i);
                func.name = toks[i]._value;

                int opened = i + 1;
                int closed = FindMatchingClosingParenthesis(opened);

                if (closed is -1) throw new Exception($"Function {toks[i + 1]._value} was never closed!");

                if (closed != opened + 1)
                {
                    func.parameters = ParseFunctionParameters(opened, closed);
                }
                else
                {
                    func.parameters = new();
                }

                nodes.Add(func);
            }
        }

        return nodes;
    }

    public int FindOperatorChainEnd(int start)
    {
        for (; start < toks.Count; start++)
        {
            if (toks[start]._tp != TokenType.OPERATOR)
            {
                return start;
            }
        }

        return -1;
    }

    public int FindMatchingClosingParenthesis(int startIndex)
    {
        int nestCount = 0;
        for (int i = startIndex; i < toks.Count; i++)
        {
            if (toks[i]._tp == TokenType.LPAREN)
            {
                nestCount++;
            }

            if (toks[i]._tp == TokenType.RPAREN)
            {
                nestCount--;

                if (nestCount == 0)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public int FindMatchingClosingParenthesis(int startIndex, ref List<Token> tokens)
    {
        int nestCount = 0;
        for (int i = startIndex; i < tokens.Count; i++)
        {
            if (tokens[i]._tp == TokenType.LPAREN)
            {
                nestCount++;
            }

            if (tokens[i]._tp == TokenType.RPAREN)
            {
                nestCount--;

                if (nestCount == 0)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public int FindMatchingClosingCBracket(int startIndex)
    {
        int nestCount = 0;
        for (int i = startIndex; i < toks.Count; i++)
        {
            if (toks[i]._tp == TokenType.LCBRACE)
            {
                nestCount++;
            }

            if (toks[i]._tp == TokenType.RCBRACE)
            {
                nestCount--;

                if (nestCount == 0)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public int FindMatchingClosingVBracket(int startIndex, ref List<Token> token)
    {
        for (; startIndex < toks.Count; startIndex++)
        {
            if (toks[startIndex]._value == ">")
            {
                token.Add(toks[startIndex]);
                return startIndex;
            }
            else
            {
                token.Add(toks[startIndex]);
            }
        }

        return -1;
    }

    public List<FunctionParameterNode> ParseFunctionParameters(int start, int end)
    {
        List<FunctionParameterNode> nodes = new();
        List<Token> tokenSlice = toks.GetRange(start, end - start);

        FunctionParameterNode node = new();
        for (int i = 1; i < tokenSlice.Count - 1; i++)
        {
            if (tokenSlice[i]._tp == TokenType.COMMA)
            {
                nodes.Add(node);
                node = new();
            }

            if (tokenSlice[i]._tp == TokenType.OPERATOR)
            {
                node.type = node.type + "*";
            }

            if (tokenSlice[i]._tp == TokenType.WORD)
            {
                if (tokenSlice[i + 1]._tp == TokenType.WORD || tokenSlice[i + 1]._tp == TokenType.OPERATOR)
                {
                    node.type = tokenSlice[i]._value;
                }
                else if (tokenSlice[i + 1]._tp == TokenType.COMMA)
                {
                    node.name = tokenSlice[i]._value;
                }
            }
        }

        node.name = tokenSlice[tokenSlice.Count - 1]._value;
        nodes.Add(node);

        return nodes;
    }

    public void ParseFunctionBody(FunctionDeclarationNode decl)
    {
        int start = FindMatchingClosingParenthesis(NavigateToFunction(decl.name)) + 2; //this puts us in such a place that our next token is the first token in the function body

        for (int i = start; i < toks.Count; i++)
        {
            if (toks[i]._value == "return")
            {
                continue;
            }

            if (toks[i]._value == "while")
            {
                continue;
            }

            if (toks[i]._value == "if")
            {
                continue;
            }

            if (toks[i]._value == "switch")
            {
                continue;
            }

            if (toks[i]._tp == TokenType.WORD && toks[i + 1]._tp == TokenType.WORD || toks[i]._tp == TokenType.WORD && toks[i + 1]._tp == TokenType.OPERATOR && toks[i + 1]._value == "*")
            {
                VarDeclarationNode node = new();
                i = ParseVarDeclaration(ref node, i);
                decl.body.Add(node);
            }

            if (toks[i]._tp == TokenType.RCBRACE)
            {
                break;
            }
        }
    }

    public int NavigateToFunction(string name)
    {
        for (int i = 0; i < toks.Count; i++)
        {
            if (toks[i]._tp == TokenType.WORD && toks[i]._value == name)
            {
                return i;
            }
        }

        return -1;
    }

    public int ParseVarDeclaration(ref VarDeclarationNode node, int start)
    {
        node.type = toks[start]._value;

        start = start + 1;

        if (toks[start]._tp == TokenType.WORD)
        {
            node.name = toks[start + 1]._value;
            start = start + 1;
        }
        else
        {
            for (bool readingops = true; readingops; start++)
            {
                if (toks[start]._tp == TokenType.WORD)
                {
                    readingops = false;
                    break;
                }
                else
                {
                    node.type = node.type + "*";
                }
            }
        }

        node.name = toks[start]._value;

        start = start + 1; //may need to be removed

        if (toks[start]._tp == TokenType.OPERATOR && toks[start]._value == "=")
        {
            start = start + 1;

            Console.WriteLine(toks[start]._value);

            ExpressionNode expr = new ExpressionNode();
            start = ParseExpression(ref expr, start, node.type);

            VariableExpressionNode varNode = new();
            varNode.varName = node.name;
            varNode.type = node.type;

            AssignmentNode asmNode = new();
            asmNode.node = varNode;
            asmNode.value = expr;

            node.assignment = asmNode;
        }
        else
        {
            throw new Exception($"Could not parse variable declaration for variable {node.name}");
        }

        return start;
    }

    public int ParseExpression(ref ExpressionNode expr, int start, string type)
    {
        List<Token> n;
        start = FindExpressionEnd(start, out n);

        if (start is not -1 && n.Count < 2)
        {
            switch (n[0]._tp)
            {
                case TokenType.STRING:
                {
                    expr = new TextValueNode();
                    ((TextValueNode)expr).type = "string";
                    ((TextValueNode)expr).value = n[0]._value;

                    break;
                }

                case TokenType.CHAR:
                {
                    expr = new TextValueNode();
                    ((TextValueNode)expr).type = "char";
                    ((TextValueNode)expr).value = n[0]._value[0];

                    break;
                }

                case TokenType.WORD: //handle literally every other type of value
                {
                    long checker = long.Parse(n[0]._value);
                    
                    try
                    {
                        if(checker > int.MaxValue)
                        {
                            expr = new TextValueNode();
                            ((TextValueNode)expr).type = "int";
                            ((TextValueNode)expr).value = int.Parse(n[0]._value);
                        } else {
                            expr = new TextValueNode();
                            ((TextValueNode)expr).type = "long";
                            ((TextValueNode)expr).value = long.Parse(n[0]._value);
                        }
                    } catch
                    {
                        try
                        {
                            VariableExpressionNode? node = expr as VariableExpressionNode;

                            if(node is null) throw new Exception();

                            node!.type = type;
                            node.varName = n[0]._value;
                        } catch
                        {
                            throw new Exception($"Could not parse expression! Token: {n[0]._value}");
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            ParseComplicatedExpression(ref expr, ref n); // add args for func
        }

        return start;
    }

    public void ParseComplicatedExpression(ref ExpressionNode expr, ref List<Token> n)
    {
        List<Token[]> tokenSegments = new();

        for(int i = 0; i < n.Count; i++)
        {
            int prec = 0;
            if(n[i]._tp == TokenType.OPERATOR)
            {
                if(n[i]._value == "&&" || n[i]._value == "<" || n[i]._value == ">" || n[i]._value == "||")
                {
                    throw new Exception("Ternaries are stupid!");
                }

                if(n[i]._value == "*" && n[i]._metadata != "mult")
                {
                    continue;
                }

                prec = DeterminePEMDAS(n[i]._value);
            }
            
            //this is how we know we are reading a function
            if(n[i]._tp == TokenType.WORD && n[i + 1]._value == "(")
            {
                i = FindMatchingClosingParenthesis(i + 1, ref n);
                continue;
            }
        }
    }

    public int DeterminePEMDAS(string val)
    {
        switch(val)
        {
            case "(":
            {
                return 1;
            }

            case "*":
            {
                return 2;
            }

            case "/":
            {
                return 3;
            }

            case "+":
            {
                return 4;
            }

            case "-":
            {
                return 5;
            }
        }

        return -1; 
    }

    public Tuple<Token[], Token[]> SplitToks(List<Token> list, int splitIndex)
    {
        List<Token> slice1 = new();
        List<Token> slice2 = new();

        for(int i = 0; i < list.Count; i++)
        {
            if(i >= splitIndex)
            {
                slice2.Add(list[i]);
                continue;
            }

            slice1.Add(list[i]);
        }

        return new(slice1.ToArray(), slice2.ToArray());
    }

    public int FindExpressionEnd(int start, out List<Token> segment)
    {
        segment = new();

        int parenthesisCount = 0;
        for (int i = start; i < toks.Count; i++)
        {
            if (toks[i]._value == ";")
            {
                return i;
            }

            if (toks[i]._tp == TokenType.LPAREN)
            {
                parenthesisCount = parenthesisCount + 1;
            }

            if (toks[i]._tp == TokenType.RPAREN)
            {
                parenthesisCount = parenthesisCount - 1;

                if (parenthesisCount < 1)
                {
                    return i;
                }
            }

            segment.Add(toks[i]);
        }

        return -1;
    }
}