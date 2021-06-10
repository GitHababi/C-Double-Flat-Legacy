using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public class Value
    {
        public ValueType DataType;
        public string Data;

        public Value() { }
        public Value(string data, ValueType type) { this.Data = data; this.DataType = type; }

        public static Value Default = new Value("0",ValueType.NUMBER);
    }

    public enum ValueType
    {
        STRING,
        BOOL,
        NUMBER
    }
}
