using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core.Runtime;

namespace C_Double_Flat.Core
{
    public interface IFunction
    {
        string Description();
        Value Run(List<Value> Inputs);
    }

    public class User_Function : IFunction
    {
        public string Description()
        {
            return "A user defined function";
        }
        public User_Function(List<Token> Args, List<Statement> Statements)
        {
            this.args = Args;
            this.statements = Statements;
        }

        private List<Statement> statements;
        private List<Token> args;


        public Value Run(List<Value> Inputs)
        {
            List <Statement> arguments = new List<Statement>();
            for (int i = 0; i < args.Count; i++)
            {
                ASSIGN a = new ASSIGN();
                a.Identifier = args[i];
                a.Value = new ExpressionNode(ValueHelper.ValueToToken(Inputs[i]));
                arguments.Add(a);
            }
            arguments.AddRange(statements);
            return Interpreter.Interpret(arguments, Program.ProgramLocation, true);
        }
    }
}
