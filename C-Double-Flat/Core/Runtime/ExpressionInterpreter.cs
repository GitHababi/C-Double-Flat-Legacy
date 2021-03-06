using System;
using System.Collections.Generic;
using C_Double_Flat.Errors;

namespace C_Double_Flat.Core.Runtime
{
    public partial class Interpreter
    {

        private Value InterpretExpression(ExpressionNode node)
        {

            Value output = new();
            if (node.GetType() == typeof(ConditionNode))
            {
                return Check((ConditionNode)node) ? new Value("true", ValueType.BOOL) : new Value("false", ValueType.BOOL);
            }
            else if (node.GetType() == typeof(FunctionCallNode))
            {
                return CallFunction(node);
            }
            else switch (node.Type)
            {
                case TokenType.ADD:
                    output = Add(node);
                    break;
                case TokenType.SUB:
                    output = Subtract(node);
                    break;
                case TokenType.MUL:
                    output = Multiply(node);
                    break;
                case TokenType.DIV:
                    output = Divide(node);
                    break;
                case TokenType.STRING:
                    output.Data = node.Value;
                    output.DataType = ValueType.STRING;
                    break;
                case TokenType.BOOL:
                    output.Data = node.Value;
                    output.DataType = ValueType.BOOL;
                    break;
                case TokenType.NUMBER:
                    output.DataType = ValueType.NUMBER;
                    output.Data = node.Value;
                    break;
                case TokenType.IDENTIFIER:
                    output = GetValue(node);
                    break;
                case TokenType.ASNAME:
                    output = GetValueFromAsName(node);
                    break;
            }

            return output;
        }


        private Value GetValueFromAsName(ExpressionNode node)
        {
            string id = InterpretExpression(node.Left).Data;
            if (id.MatchesArray(TokenHelper.IllegalIdentifiers)) throw new InvalidAsNameException(node.Position);
            Token tok = new(TokenType.IDENTIFIER, id, PositionHelper.None);
            return GetValue(new(tok));
        }

        private Value CallFunction(ExpressionNode node)
        {
            FunctionCallNode call = (FunctionCallNode)node;
            string name;

            if (call.Type == TokenType.IDENTIFIER) name = call.Value;
            else {  name = InterpretExpression(call.Left.Left).Data; }

            if (Interpreter.Functions.TryGetValue(name, out IFunction function))
            {
                List<Value> args = new();
                call.Args.ForEach(expnode => args.Add(InterpretExpression(expnode)));

                return function.Run(args);
            }
            return Value.Default;
        }

        private Value GetValue(ExpressionNode node)
        {
            if (!scopedVars.TryGetValue(node.Value, out Value output)) // First try to find a local definition
            {
                if (!Interpreter.GlobalVars.TryGetValue(node.Value, out output)) // Then a global definition
                {
                    if (Functions.TryGetValue(node.Value, out IFunction function)) // then call a function with no args
                    {
                        return function.Run(new List<Value>());
                    }
                    return Value.Default;
                }
            }
            return output;
        }

        private Value Subtract(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);

            left = left.CastValue(ValueType.NUMBER);
            right = right.CastValue(ValueType.NUMBER);

            Value output = new()
            {
                Data = (Convert.ToDouble(left.Data) - Convert.ToDouble(right.Data)).ToString(),
                DataType = ValueType.NUMBER
            };

            return output;
        }

        private Value Add(ExpressionNode node)
        {
            ValueHelper.ResolveType(out Value left, out Value right, InterpretExpression(node.Left), InterpretExpression(node.Right));

            Value output = new();

            switch (left.DataType)
            {
                case ValueType.NUMBER:
                    output.Data = (Convert.ToDouble(left.Data) + Convert.ToDouble(right.Data)).ToString();
                    output.DataType = ValueType.NUMBER;
                    break;
                case ValueType.STRING:
                    output.Data = left.Data + right.Data;
                    output.DataType = ValueType.STRING;
                    break;
            }

            return output;
        }

        private Value Multiply(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);

            left = left.CastValue(ValueType.NUMBER);
            right = right.CastValue(ValueType.NUMBER);


            Value output = new()
            {
                Data = (Convert.ToDouble(left.Data) * Convert.ToDouble(right.Data)).ToString(),
                DataType = ValueType.NUMBER
            };


            return output;
        }

        private Value Divide(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);

            left = left.CastValue(ValueType.NUMBER);
            right = right.CastValue(ValueType.NUMBER);

            if (Convert.ToDouble(right.Data) == 0)
            {
                throw new DivideByZeroException();
            }

            Value output = new()
            {
                Data = (Convert.ToDouble(left.Data) / Convert.ToDouble(right.Data)).ToString(),
                DataType = ValueType.NUMBER
            };

            return output;
        }
    }
}
