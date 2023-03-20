using Emerald.Types;
using Emerald.AST;

namespace Emerald.Parsing;

public class Parser
{
    public Token[] toks { get; set; }
    public int _pos { get; set; } = 0;
    public int _cur_ns_ind { get; set; } = 0;

    public Parser(in Token[] arr)
    {
        toks = arr;
    }

    public void Parse(out AbstractSyntaxTree tree)
    {
        tree = new();

        while(_pos < toks.Length)
        {
            switch(toks[_pos]._tp)
            {
                case TokenType.WORD:
                {
                    switch(toks[_pos]._value)
                    {
                        case "namespace":
                        {
                            ASTNamespaceDeclaration decl = new(toks[_pos + 1]._value, null, null);
                            decl.type = ASTNodeType.NAMESPACE_DECL;
                            tree.AddRoot(decl);
                            
                            try
                            {
                                _cur_ns_ind = (int)ASTNamespaceDeclaration.IndexOf(toks[_pos + 1]._value, in tree)!;
                            } catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }

                            _pos = _pos + 1;
                            continue;
                        }

                        case "ruleset":
                        {
                            ASTRulesetDeclaration decl = new();
                            ((ASTNamespaceDeclaration)tree[_cur_ns_ind]).ruleset = decl;
                            decl.nspace = ((ASTNamespaceDeclaration)tree[_cur_ns_ind]);

                            if(toks[_pos + 1]._tp == TokenType.LCBRACE)
                            {
                                _pos = _pos + 1;
                            }

                            ReadRuleset(ref decl, ref tree);
                            continue;
                        }
                    }

                    break;
                }
            }

            _pos = _pos + 1;
        }
    }

    public void ReadRuleset(ref ASTRulesetDeclaration decl, ref AbstractSyntaxTree tree)
    {
        while(true)
        {
            switch(toks[_pos]._tp)
            {
                case TokenType.WORD:
                {
                    switch(toks[_pos]._value)
                    {
                        case "selfalloc":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.SELFALLOC;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }
                    }

                    break;
                }
                
                case TokenType.SEMICOLON:
                {
                    continue;
                }

                default:
                {
                    Console.WriteLine(toks[_pos]._tp);
                    throw new Exception("Unexpected semantics in ruleset!");
                }
            }

            _pos = _pos + 1;
        }
    }
}