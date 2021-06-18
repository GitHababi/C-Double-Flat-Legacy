using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Core.Runtime;
using C_Double_Flat.Errors;

namespace C_Double_Flat.Libraries
{
    class Math_Rand : IFunction
    {
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2);

            int lower = (int)Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data));
            int upper = (int)Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[1], Core.ValueType.NUMBER).Data));

            Random random = new Random();
            return new Value(random.Next(lower, upper).ToString(),Core.ValueType.NUMBER);
        }
    }
    class Math_Round : IFunction
    {
        public Value Run(List<Value> values)
        {
            if (values.Count < 1) throw new ArgumentCountException(1);

            return new Value(Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data)).ToString(), Core.ValueType.NUMBER);
        }
    }
}
