using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public interface IFunction
    {
        Value Run(List<ExpressionNode> Inputs);
    }

    public class User_Function : IFunction
    {
        public User_Function(List<Token> Args, List<Statement> Statements)
        {
            this.args = Args;
            this.statements = Statements;
        }

        private List<Statement> statements;
        private List<Token> args;


        public Value Run(List<ExpressionNode> Inputs)
        {
            List < ASSIGN > arguments = new List<ASSIGN>();
            for (int i = 0; i < args.Count; i++)
            {
                ASSIGN a = new ASSIGN();
                a.Identifier = args[i];
                a.Value = Inputs[i];
            }
            return null; /* Whenever statement interpreting is indroduced, this will need to edit. */
        }
    }
}
