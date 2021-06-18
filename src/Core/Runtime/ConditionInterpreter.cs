using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core.Runtime
{
    public partial class Interpreter
    {
        private bool Check(ConditionNode node)
        {
            Value l = InterpretExpression(node.Left);
            Value r = InterpretExpression(node.Right);
            switch (node.Type)
            {
                case TokenType.EQAL:
                    return l.Data == r.Data;
                case TokenType.NOT_EQAL:
                    return l.Data != r.Data;
                case TokenType.LESS:
                    l = ValueHelper.CastValue(l, ValueType.NUMBER);
                    r = ValueHelper.CastValue(r, ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) < Convert.ToDouble(r.Data));
                case TokenType.GRTR_EQ:
                    l = ValueHelper.CastValue(l, ValueType.NUMBER);
                    r = ValueHelper.CastValue(r, ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) >= Convert.ToDouble(r.Data));
                case TokenType.GRTR:
                    l = ValueHelper.CastValue(l, ValueType.NUMBER);
                    r = ValueHelper.CastValue(r, ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) > Convert.ToDouble(r.Data));
                case TokenType.LESS_EQ:
                    l = ValueHelper.CastValue(l, ValueType.NUMBER);
                    r = ValueHelper.CastValue(r, ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) <= Convert.ToDouble(r.Data));
                default:
                    return false;
            }
        }

    }
}
