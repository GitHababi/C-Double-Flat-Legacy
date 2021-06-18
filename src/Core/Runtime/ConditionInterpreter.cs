using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core.Runtime
{
    public class ConditionInterpreter
    {
        public static bool Check(ConditionNode node, ref Dictionary<string, Value> scope)
        {
            Value l = ExpressionInterpreter.Interpret(node.Left, ref scope);
            Value r = ExpressionInterpreter.Interpret(node.Right, ref scope);
            switch (node.Type)
            {
                case TokenType.EQAL:
                    return Equal(l, r);
                case TokenType.NOT_EQAL:
                    return !Equal(l, r);
                case TokenType.LESS:
                    return LessThan(l, r);
                case TokenType.GRTR_EQ:
                    return !LessThan(l, r);
                case TokenType.GRTR:
                    return GreaterThan(l, r);
                case TokenType.LESS_EQ:
                    return !GreaterThan(l, r);
                default:
                    return false;
            }
        }
        private static bool Equal(Value l, Value r)
        {
            return l.Data == r.Data;
        }
        private static bool LessThan(Value l, Value r)
        {
            l = ValueHelper.CastValue(l, ValueType.NUMBER);
            r = ValueHelper.CastValue(r, ValueType.NUMBER);

            return (Convert.ToDouble(l.Data) < Convert.ToDouble(r.Data));
        }

        private static bool GreaterThan(Value l, Value r)
        {
            l = ValueHelper.CastValue(l, ValueType.NUMBER);
            r = ValueHelper.CastValue(r, ValueType.NUMBER);

            return (Convert.ToDouble(l.Data) > Convert.ToDouble(r.Data));
        }

    }
}
