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
        public Value Run(List<Value> Inputs)
        {
            string write = "";
            
            foreach (Value v in Inputs) write += v.Data;
            Console.WriteLine(write);
            return Value.Default;
        }
    }
    
    class Display_Prompt : IFunction
    {
        public Value Run(List<Value> Inputs)
        {
            return new Value(Console.ReadLine(),Core.ValueType.STRING);
        }
    }
    class Display_Clear : IFunction
    {
        public Value Run(List<Value> Inputs) { Console.Clear(); return Value.Default; }
    }
    
}
