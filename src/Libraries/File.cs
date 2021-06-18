using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Libraries;
using C_Double_Flat.Errors;
using System.IO;
namespace C_Double_Flat.Libraries
{
    class File_Read : IFunction
    {
        public Value Run(List<Value> values)
        {
            string output = "";
            if (values.Count < 1) throw new ArgumentCountException(1);
            try
            {
                output = File.ReadAllText(values[0].Data);
            }
            catch { }
            return new Value(output, Core.ValueType.STRING);
        }
    }

    class File_Save : IFunction
    {
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2);
            try
            {
                File.WriteAllText(values[0].Data, values[1].Data);
            }
            catch { }
            return Value.Default;
        }
    }
}
