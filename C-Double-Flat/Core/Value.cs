using System;

namespace C_Double_Flat.Core
{
    public struct Value
    {
        public ValueType DataType;
        public string Data;

        public Value(string data, ValueType type) { this.Data = data; this.DataType = type; }

        public Value(double number) { this.Data = number.ToString(); this.DataType = ValueType.NUMBER; }
        public Value(string str) { this.Data = str; this.DataType = ValueType.STRING; }
        public Value(bool boolean) { this.Data = boolean.ToString(); this.DataType = ValueType.BOOL; }

        public static Value Default = new(0);

        public override string ToString()
        {
            return String.Format("Type:{1} Value: {0}", Data, DataType);
        }

        public static explicit operator double(Value val)
        {
            if (val.DataType == ValueType.NUMBER) return Convert.ToDouble(val.CastValue(ValueType.NUMBER).Data);
            else return 0;
        }

        public static explicit operator bool(Value val)
        {
            if (val.DataType == ValueType.BOOL) return (val.Data == "true") ? true : false;
            else return false;
        }

        public static explicit operator string(Value val)
        {
            return val.Data;
        }

        public static explicit operator int(Value val)
        {
            return (int)Math.Round((double)val.CastValue(ValueType.NUMBER));
        }
    }

    public enum ValueType
    {
        STRING,
        BOOL,
        NUMBER
    }

    public static class ValueHelper
    {
        private readonly static ValueType[,] ResolvingTable = {
            { ValueType.STRING, ValueType.STRING, ValueType.STRING },
            { ValueType.STRING, ValueType.NUMBER, ValueType.NUMBER },
            { ValueType.STRING, ValueType.NUMBER, ValueType.NUMBER }
        };

        public static Value CastValue(this Value value, ValueType type)
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

        public static Value TokenToValue(Token token)
        {
            
            return token.Type switch
            {
                TokenType.NUMBER => new(token.Value, ValueType.NUMBER),
                TokenType.BOOL => new(token.Value, ValueType.BOOL),
                TokenType.STRING=> new(token.Value, ValueType.STRING),
                _ => throw new InvalidCastException()
            };
        }

        public static Token ValueToToken(Value value)
        {

            return value.DataType switch
            {
                ValueType.BOOL => new Token(TokenType.BOOL, value.Data, PositionHelper.None),
                ValueType.STRING => new Token(TokenType.STRING, value.Data, PositionHelper.None),
                ValueType.NUMBER => new Token(TokenType.NUMBER, value.Data, PositionHelper.None),
                _ => TokenHelper.None,
            };
        }

        
        // This is awful, but i don't feel like fixing it. Idk if this is even used anywhere bc this can be replaced.

        public static void ResolveType(out Value out1, out Value out2, Value inp1, Value inp2)
        {
            ValueType toCast = ResolvingTable[(int)inp1.DataType, (int)inp2.DataType];
            out1 = inp1.CastValue(toCast);
            out2 = inp2.CastValue(toCast);
        }


    }
}
