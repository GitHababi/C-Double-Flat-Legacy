using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public abstract class Statement
    {
        public StatementType Type = StatementType.NONE;
        public object ReturnSelf() { return this; }
    }

    public class IF : Statement
    {
        public ConditionNode Condition;
        public new StatementType Type = StatementType.IF;
        public List<Statement> If;
        public ELSE Else;
    }

    public class ELSE : Statement
    {
        public List<Statement> Statements;
    }

    public class ASSIGN : Statement
    {
        public new StatementType Type = StatementType.ASSIGN;
        public Token Identifier;
        public ExpNode Value;
    }

    public class RETURN : Statement
    {
        public new StatementType Type = StatementType.RETURN;
        public ExpNode Value;
    }

    public class LOOP : Statement
    {
        public new StatementType Type = StatementType.LOOP;
        public ConditionNode Condition;
        public List<Statement> Statements;
    }

    public class FUNCTION : Statement
    {
        public new StatementType Type = StatementType.FUNCTION_ASSIGN;
        public List<Statement> Statements;
        public List<Token> Arguments;
    }

    public enum StatementType
    {
        FUNCTION_ASSIGN,
        ASSIGN,
        EXPRESSION,
        IF,
        ELSE,
        LOOP,
        RETURN,
        NONE
    }
}
