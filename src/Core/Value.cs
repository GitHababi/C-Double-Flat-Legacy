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

        public override string ToString()
        {
            return String.Format("Type:{1} Value: {0}",Data,DataType);
        }

    }

    public enum ValueType
    {
        STRING,
        BOOL,
        NUMBER
    }

    public static class  ValueHelper
    {
        private readonly static ValueType[,] ResolvingTable = {
            { ValueType.STRING, ValueType.STRING, ValueType.STRING },
            { ValueType.STRING, ValueType.NUMBER, ValueType.NUMBER },
            { ValueType.STRING, ValueType.NUMBER, ValueType.NUMBER }
        };

        public static Value TokenToValue(Token token)
        {
            Value output = new Value();

            switch (token.Type)
            {
                case TokenType.NUMBER:
                    output.DataType = ValueType.NUMBER;
                    output.Data = token.Value;
                    break;
                case TokenType.BOOL:
                    output.DataType = ValueType.BOOL;
                    output.Data = token.Value;
                    break;
                case TokenType.STRING:
                    output.DataType = ValueType.STRING;
                    output.Data = token.Value;
                    break;
            }

            return output;
        }

        public static Token ValueToToken(Value value)
        {

            switch (value.DataType)
            {
                case ValueType.BOOL:
                    return new Token(TokenType.BOOL, value.Data, PositionHelper.None);
                case ValueType.STRING:
                    return new Token(TokenType.STRING, value.Data, PositionHelper.None);
                case ValueType.NUMBER:
                    return new Token(TokenType.NUMBER, value.Data, PositionHelper.None);
            }

            return TokenHelper.None;
        }

        public static Value CastValue(Value value, ValueType type)
        {
            
            switch (value.DataType)
            {
                case ValueType.BOOL:
                    switch (type)
                    {
                        case ValueType.STRING:
                            break;
                        case ValueType.BOOL:
                            break;
                        case ValueType.NUMBER:
                            value.Data = (value.Data == "true") ? "1" : "0";
                            break;
                    }
                    break;
                case ValueType.NUMBER:
                    switch (type)
                    {
                        case ValueType.STRING:
                            break;
                        case ValueType.BOOL:
                            value.Data = "false";
                            break;
                        case ValueType.NUMBER:
                            break;

                    }
                    break;
                case ValueType.STRING:
                    switch (type)
                    {
                        case ValueType.STRING:
                            break;
                        case ValueType.BOOL:
                            value.Data = "false";
                            break;
                        case ValueType.NUMBER:
                            value.Data = "0";
                            break;
                    }
                    break;
            }
            value.DataType = type;
            return value;
        }

        public static void ResolveType(out Value out1, out Value out2, Value inp1, Value inp2)
        {
            ValueType toCast = ResolvingTable[(int)inp1.DataType, (int)inp2.DataType];
            out1 = CastValue(inp1, toCast);
            out2 = CastValue(inp2, toCast);
        }


    }
}
