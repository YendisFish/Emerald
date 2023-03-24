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
                            cls.keyword = Keywords.CLASS;

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
                            if(toks[_pos + 2]._tp == TokenType.LPAREN)
                            {
                                ASTMethodDeclaration decl = new();
                                decl.type = ASTNodeType.METHOD_DECL;
                            } else if(toks[_pos + 1]._tp == TokenType.OPERATOR && toks[_pos + 1]._value == "*")
                            {
                                //create a pointer instance
                            } else
                            {
                                ASTVariable var = new();
                                var.type = ASTNodeType.VARIABLE;
                                var.vtype = VarType.INT;
                                
                                _pos = _pos + 1;
                                ReadProperty(ref var, ref tree, ref cls);
                            }

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

                        default:
                        {
                            if(toks[_pos]._value == name)
                            {
                                ASTMethodDeclaration decl = new();
                                decl.name = name;
                                
                                cls.fields?.Add(decl);
                                //ReadMethod(out decl, ref tree);
                            }

                            foreach(ASTNode nd in cls.fields ?? new())
                            {
                                switch(nd.type)
                                {
                                    case ASTNodeType.METHOD_DECL:
                                    {
                                        break;
                                    }
                                    case ASTNodeType.VARIABLE:
                                    {
                                        break;
                                    }
                                }
                            }

                            throw new Exception("Could not parse AST! Invalid keyword was found!");
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

                case TokenType.RCBRACE:
                {
                    pos_is_in = !pos_is_in;
                    break;
                }
            }

            _pos = _pos + 1;
        }
    }

    public void ReadProperty(ref ASTVariable var, ref AbstractSyntaxTree tree, ref ASTClass cls)
    {
        while(true)
        {
            switch(var.vtype)
            {
                case VarType.INT:
                {
                    if(toks[_pos]._tp == TokenType.WORD)
                    {
                        var.name = toks[_pos]._value;

                        if(toks[_pos + 1]._tp == TokenType.OPERATOR && toks[_pos + 1]._value == "=")
                        {
                            //This is going to take a FAT bit of code

                            /*
                                For reading assingments we need to do a few things:

                                1. If it is a function then we need to go down to the function
                                and make sure that it is of the same return type.

                                2. If it is an index from an array then we need to check that
                                array.

                                3. If it is a constant then it'll be pretty easy to set.
                            */
                        } else {
                            throw new Exception("Unexpected operator while assinging to intenger " + var.name + " in class " + cls.name);
                        }
                    } else {
                        throw new Exception("Unexpected variable initialization!");
                    }

                    break;
                }
            }

            _pos = _pos + 1;
        }
    }

    //this will leave you on the index after the semicolon
    public void ReadToSemicolon(ref List<Token> arr)
    {
        //clear the list since we are supposedly reading from the last semicolon!
        arr = new();

        while(true)
        {
            if(toks[_pos]._tp == TokenType.SEMICOLON)
            {
                _pos = _pos + 1;
                break;
            }

            arr.Add(toks[_pos]);
            _pos = _pos + 1;
        }
    }

    public void ReadPointerProperty() { }
    public void ReadVar() { }
    public void ReadPointer() { }
    public void ReadReference() { }

    public void ReadAmbiguousNamespaceContext() { }

    public void ReadAmbiguousClassContext(out ASTNode node, ref AbstractSyntaxTree tree, in ASTNode parent)
    {
        List<Token> section = new();


        ASTClass cls = (ASTClass)parent;

        while(true)
        {
            switch(toks[_pos]._tp)
            {
                case TokenType.WORD:
                {
                    foreach(ASTNode nd in cls.fields ?? new())
                    {
                        //first we check against it as a variable
                        ASTVariable var = (ASTVariable)nd;
                    }

                    break;
                }
            }

            _pos = _pos + 1;
        }
    }
}