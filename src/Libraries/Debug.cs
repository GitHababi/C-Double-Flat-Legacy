using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Core.Runtime;
namespace C_Double_Flat.Libraries
{
    class Debug_Functions : IFunction
    {
        public Value Run(List<ExpressionNode> values, Dictionary<string, Value> Scope)
        {
            foreach (string key in Interpreter.Functions.Keys)
            {
                Console.WriteLine(key);
            }
            return Value.Default;
        }
    }
    class Debug_Vars : IFunction
    {
        public Value Run(List<ExpressionNode> values, Dictionary<string, Value> Scope)
        {
            foreach (string key in Interpreter.globalVars.Keys)
            {
                Console.WriteLine(key);
            }
            return Value.Default;
        }
    }
}
