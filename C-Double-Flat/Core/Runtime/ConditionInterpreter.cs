using System;

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
                    l = l.CastValue(ValueType.NUMBER);
                    r = r.CastValue(ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) < Convert.ToDouble(r.Data));
                case TokenType.GRTR_EQ:
                    l = l.CastValue(ValueType.NUMBER);
                    r = r.CastValue(ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) >= Convert.ToDouble(r.Data));
                case TokenType.GRTR:
                    l = l.CastValue(ValueType.NUMBER);
                    r = r.CastValue(ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) > Convert.ToDouble(r.Data));
                case TokenType.LESS_EQ:
                    l = l.CastValue(ValueType.NUMBER);
                    r = r.CastValue(ValueType.NUMBER);

                    return (Convert.ToDouble(l.Data) <= Convert.ToDouble(r.Data));
                default:
                    return false;
            }
        }

    }
}
