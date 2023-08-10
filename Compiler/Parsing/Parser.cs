using Emerald.Types;
using Emerald.AST;

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
        tree.nodes.AddRange(ParseFunctionDeclarations());
        
        return tree;
    }

    public RulesetNode ParseRuleset()
    {
        RulesetNode ruleset = new RulesetNode();

        for(int i = 0; i < toks.Count; i++)
        {
            if(toks[i]._tp == TokenType.WORD && toks[i]._value == "ruleset")
            {
                for(bool search = true; search != false; i++)
                {
                    switch(toks[i]._tp)
                    {
                        case TokenType.SEMICOLON:
                        {
                            if(toks[i - 1]._value == "ruleset")
                            {
                                ruleset.rungc = true;
                                ruleset.stackPreferred = true;
                                ruleset.heapPreferred = false;
                                
                                search = false;
                                break;
                            } else {
                                continue;
                            }
                        }

                        case TokenType.WORD:
                        {
                            if(toks[i]._value == "heappreferred")
                            {
                                ruleset.heapPreferred = true;
                            }

                            if(toks[i]._value == "stackpreferred")
                            {
                                ruleset.stackPreferred = true;
                            }

                            if(toks[i]._value == "rungc")
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
            } else {
                throw new Exception("Could not parse ruleset! It looks like you are missing a semicolon or braces!");
            }
        }

        return ruleset;
    }

    public List<FunctionDeclarationNode> ParseFunctionDeclarations()
    {
        List<FunctionDeclarationNode> nodes = new();

        for(int i = 0; i < toks.Count; i++)
        {
            if(toks[i]._tp == TokenType.WORD && toks[i + 1]._tp == TokenType.WORD && toks[i + 2]._tp == TokenType.LPAREN)
            {
                FunctionDeclarationNode func = new FunctionDeclarationNode();
                func.returnType = toks[i]._value;
                func.name = toks[i + 1]._value;
                
                int opened = i + 2;
                int closed = FindMatchingClosingParenthesis(i + 2);

                if(closed is -1) throw new Exception($"Function {toks[i + 1]._value} was never closed!");

                func.parameters = ParseFunctionParameters(opened, closed);

                nodes.Add(func);
            }
        }

        return nodes;
    }

    public int FindMatchingClosingParenthesis(int startIndex)
    {
        int nestCount = 0;
        for(int i = startIndex; i < toks.Count; i++)
        {
            if(toks[i]._tp == TokenType.LPAREN)
            {
                nestCount++;
            }

            if(toks[i]._tp == TokenType.RPAREN)
            {
                nestCount--;

                if(nestCount == 0)
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public List<FunctionParameterNode> ParseFunctionParameters(int start, int end)
    {
        List<FunctionParameterNode> nodes = new();
        List<Token> tokenSlice = toks.GetRange(start, end - start);

        FunctionParameterNode node = new();
        for(int i = 1; i < tokenSlice.Count - 1; i++)
        {
            if(tokenSlice[i]._tp == TokenType.COMMA)
            {
                nodes.Add(node);
                node = new();
            }

            if(tokenSlice[i]._tp == TokenType.OPERATOR)
            {
                node.type = node.type + "*";
            }

            if(tokenSlice[i]._tp == TokenType.WORD)
            {
                /*
                if(tokenSlice[i + 1]._tp == TokenType.WORD)
                {
                    node.type = tokenSlice[i]._value;
                } else if(tokenSlice[i + 1]._tp != TokenType.OPERATOR) {
                    node.name = tokenSlice[i]._value;
                } else if(tokenSlice[i + 1]._tp == TokenType.OPERATOR)
                {
                    node.type = tokenSlice[i]._value + "*";
                } else {
                    node.name = tokenSlice[i]._value;
                }*/

                
                if(tokenSlice[i + 1]._tp == TokenType.WORD || tokenSlice[i + 1]._tp == TokenType.OPERATOR)
                {
                    node.type = tokenSlice[i]._value;
                } else if(tokenSlice[i + 1]._tp == TokenType.COMMA) {
                    node.name = tokenSlice[i]._value;   
                }
            }
        }

        node.name = tokenSlice[tokenSlice.Count - 1]._value;
        nodes.Add(node);

        return nodes;
    }
}
