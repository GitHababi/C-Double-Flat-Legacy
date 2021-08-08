using System;
using System.Collections.Generic;

namespace C_Double_Flat.Core
{
    public abstract class Statement
    {
        public StatementType Type = StatementType.NONE;
        public object ReturnSelf() { return this; }
    }

    public class RUN : Statement
    {
        public new StatementType Type = StatementType.RUN;
        public ExpressionNode Path;
        public override string ToString()
        {
            return "- RUN SCRIPT AT: " + Path;
        }
    }

    public class NONE : Statement
    {
        public new StatementType Type = StatementType.NONE;
        public override string ToString()
        {
            return "- NO-OP STATEMENT";
        }
    }

    public class IF : Statement
    {
        public ConditionNode Condition;
        public new StatementType Type = StatementType.IF;
        public List<Statement> If;
        public List<Statement> Else;

        public override string ToString()
        {
            string output = "- IF ";
            output += Condition + " THEN :";
            if (If != null) foreach (Statement statement in If) output += "\n\t" + statement;

            if (Else != null)
            {
                output += "\n OTHERWISE: ";
                foreach (Statement s in Else) output += "\n\t" + s;
            }

            return output;
        }
    }


    public class ASSIGN : Statement
    {
        public new StatementType Type = StatementType.ASSIGN;
        public Token Identifier;
        public bool IsAsName = false;
        public ExpressionNode AsNameAssigner;
        public ExpressionNode Value;

        public override string ToString()
        {
            if (!IsAsName) return String.Format("- ASSIGN '{0}' TO {1}", Identifier.Value, Value);
            else return String.Format("- ASSIGN {0} TO {1}",AsNameAssigner,Value);
        }
    }

    public class RETURN : Statement
    {
        public new StatementType Type = StatementType.RETURN;
        public ExpressionNode Value;
        public override string ToString()
        {
            return String.Format("- RETURN {0}", Value);
        }
    }

    public class LOOP : Statement
    {
        public new StatementType Type = StatementType.LOOP;
        public ConditionNode Condition;
        public List<Statement> Statements;

        public override string ToString()
        {
            string output = "- LOOP WHILE: " + Condition;


            foreach (Statement st in Statements)
            {
                output += "\n\t" + st;
            }
            return output;
        }
    }

    public class REPEAT : Statement
    {
        public new StatementType Type = StatementType.REPEAT;
        public ExpressionNode Amount;
        public List<Statement> Statements;
        public override string ToString()
        {
            string output = "- LOOP TIMES OF: " + Amount;


            foreach (Statement st in Statements)
            {
                output += "\n\t" + st;
            }
            return output;
        }
    }

    public class FUNCTION : Statement
    {
        public new StatementType Type = StatementType.FUNCTION_ASSIGN;
        public List<Statement> Statements;
        public List<Token> Arguments;
        public Token Identifier;
        public bool IsAsName = false;
        public ExpressionNode AsNameAssigner;

        public override string ToString()
        {
            string name = (IsAsName) ? AsNameAssigner.ToString() : Identifier.Value ;
            string output = String.Format("- FUNCTION {0} WITH ARGUMENTS: ", name);

            foreach (Token t in Arguments)
            {
                output += "'" + t.Value + "'";
            }

            foreach (Statement st in Statements)
            {
                output += "\n\t" + st;
            }
            return output;
        }
    }

    public class EXPRESSION : Statement
    {
        public new StatementType Type = StatementType.EXPRESSION;
        public ExpressionNode Value;

        public override string ToString()
        {
            return "- EXPRESSION: " + Value;

        }
    }

    public enum StatementType
    {
        FUNCTION_ASSIGN,
        EXPRESSION,
        ASSIGN,
        IF,
        LOOP,
        RETURN,
        RUN,
        REPEAT,
        NONE
    }
}
