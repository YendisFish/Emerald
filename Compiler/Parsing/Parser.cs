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
                            decl.type = ASTNodeType.RULESET_DECL;

                            ReadRuleset(ref decl, ref tree);
                            continue;
                        }

                        case "class":
                        {
                            ASTClass cls = new();
                            cls.nspace = tree[_cur_ns_ind];
                            cls.type = ASTNodeType.CLASS;

                            string name = toks[_pos + 1]._value;
                            cls.name = name;
                            _pos = _pos + 2;
                            ReadClass(ref cls, ref tree, name);

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
        bool pos_is_in = true;
        while(pos_is_in)
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

                        case "unsafe":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.UNSAFE;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }

                        case "unmanaged":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.UNMANAGED;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }

                        case "stackpreferred":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.STACKPREFERRED;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }

                        case "stackrequired":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.STACKREQUIRED;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }

                        case "heappreferred":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.HEAPPREFERRED;
                            inst.type = ASTNodeType.RULESET_INSTRUCTION;

                            decl.Add(inst);

                            _pos = _pos + 1;
                            continue;
                        }

                        case "heaprequired":
                        {
                            ASTRulesetInstruction inst = new();
                            inst.ruleset = decl;
                            inst.keyword = Keywords.HEAPREQUIRED;
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
                    break;
                }

                case TokenType.RCBRACE:
                {
                    pos_is_in = !pos_is_in;
                    break;
                }

                case TokenType.LCBRACE:
                {
                    break;
                }

                default:
                {
                    Console.WriteLine(toks[_pos]._tp);
                    Console.WriteLine(_pos);
                    throw new Exception("Unexpected semantics in ruleset!");
                }
            }

            _pos = _pos + 1;
        }
    }

    public void ReadClass(ref ASTClass cls, ref AbstractSyntaxTree tree, string name)
    {
        bool pos_is_in = true;
        while(pos_is_in)
        {
            switch(toks[_pos]._tp)
            {
                case TokenType.WORD:
                {
                    switch(toks[_pos]._value)
                    {
                        case "int":
                        {
                            break;
                        }

                        case "char":
                        {
                            break;
                        }

                        case "bool":
                        {
                            break;
                        }

                        case "long":
                        {
                            break;
                        }

                        case "short":
                        {
                            break;
                        }

                        case "float":
                        {
                            break;
                        }

                        case "byte":
                        {
                            break;
                        }
                    }

                    _pos = _pos + 1;
                    continue;
                }

                case TokenType.LCBRACE:
                {
                    _pos = _pos + 1;
                    continue;
                }
            }

            _pos = _pos + 1;
        }
    }
}