using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Core.Runtime;
namespace C_Double_Flat.Libraries
{
    class Display_Echo : IFunction
    {
        public Value Run(List<ExpressionNode> values)
        {
            string write = "";
            List<Value> a = new List<Value>();
            
            foreach (ExpressionNode v in values) a.Add(ExpressionInterpreter.Interpret(v));
            foreach (Value v in a) write += v.Data;
            Console.WriteLine(write);
            return Value.Default;
        }
    }
    
    class Display_Prompt : IFunction
    {
        public Value Run(List<ExpressionNode> values)
        {
            return new Value(Console.ReadLine(),Core.ValueType.STRING);
        }
    }

}
