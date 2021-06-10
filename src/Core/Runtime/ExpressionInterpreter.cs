using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Errors;

namespace C_Double_Flat.Core.Runtime
{
    public class ExpressionInterpreter
    {
        private Value ExpNodeToValue(ExpressionNode final, ref Dictionary<string, Value> vars)
        {
            Value output = new Value();

            switch (final.Type)
            {
                case TokenType.NUMBER:
                    output.DataType = ValueType.NUMBER;
                    output.Data = final.Value;
                    break;
                case TokenType.STRING:
                    output.DataType = ValueType.STRING;
                    output.Data = final.Value;
                    break;
                case TokenType.BOOL:
                    output.DataType = ValueType.BOOL;
                    output.Data = final.Value;
                    break;
                case TokenType.IDENTIFIER:
                    try
                    {
                        vars.TryGetValue(final.Value, out output); //Search dictionary for identifier and value
                    }
                    catch
                    {
                        output.Data = "0";
                        output.DataType = ValueType.NUMBER;
                    }
                    break;
                default: throw new InvalidTokenException();
            }
            return output;
        }

        private ExpressionNode VarExpNodeToExpNode(ExpressionNode var, ref Dictionary<string, Value> vars)
        {
            if (var.Type != TokenType.IDENTIFIER) return var;
            
            

            ExpressionNode output = new ExpressionNode();
            
            try
            {
                Value v = new Value();
                vars.TryGetValue(var.Value, out v); // This is the part that might throw.

                output.Value = v.Data;

                switch(v.DataType)
                {
                    case ValueType.NUMBER:
                        output.Type = TokenType.NUMBER;
                        break;
                    case ValueType.BOOL:
                        output.Type = TokenType.BOOL;
                        break;
                    case ValueType.STRING:
                        output.Type = TokenType.STRING;
                        break;
                }
            }
            catch
            {
                output.Value = "0";

                output.Type = TokenType.NUMBER;
            }

            return output;
        }
    }
}
