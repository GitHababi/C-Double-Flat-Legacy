using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Errors;

// I should be working on this
// but i am lazy

namespace C_Double_Flat.Core.Runtime
{
    public class ExpressionInterpreter
    {
        public static Value Interpret(ExpressionNode Root, ref Dictionary<string, Value> vars)
        {
            return new ExpressionInterpreter(Root, ref vars).privateInterpret();
        }

        private Dictionary<string, Value> vars;
        private ExpressionNode Root;

        private ExpressionInterpreter(ExpressionNode Root, ref Dictionary<string, Value> vars) 
        {
            this.Root = Root;
            this.vars = vars;
        } 

        private Value privateInterpret()
        {
            if 
                (
                Root.Type == TokenType.NUMBER ||
                Root.Type == TokenType.STRING ||
                Root.Type == TokenType.BOOL ||
                Root.Type == TokenType.IDENTIFIER
                ) return Utilities.GetValue(Root, ref vars);

            if (Root.Equals(typeof(FuncCallNode))) ;

            switch (Root.Type)
            {
                case TokenType.ADD:
                case TokenType.MUL:
                case TokenType.SUB:
                case TokenType.DIV:

            }
        }
    }
}
