using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C_Double_Flat.Core.Runtime
{
    public partial class Interpreter
    {
        
        private Value InterpretExpression(ExpressionNode node)
        {
            if (node.GetType() == typeof(ConditionNode))
            {
                return Check((ConditionNode)node) ? new Value("true", ValueType.BOOL) : new Value("false", ValueType.BOOL);
            }
            Value output = new Value();

            switch (node.Type)
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
                case TokenType.FUNC_CALL:
                    output = CallFunction(node);
                    break;
            }

            return output;
        }
 

        private Value CallFunction(ExpressionNode node)
        {
            FuncCallNode call = (FuncCallNode)node;
            

            if (Interpreter.Functions.TryGetValue(call.Value, out IFunction function))
            {
                List<Value> args = new List<Value>();
                foreach (ExpressionNode expnode in call.Args) args.Add(InterpretExpression(expnode));
                return function.Run(args);
            }
            return Value.Default;
        }

        private Value GetValue(ExpressionNode node)
        {
            Value output = new Value();
            if(!scopedVars.TryGetValue(node.Value, out output))
            {
                if(!Interpreter.globalVars.TryGetValue(node.Value, out output))
                {
                    return Value.Default;
                }
            }
            return output;
        }

        private Value Subtract(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);

            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) - Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;

            return output;
        }

        private Value Add(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);
            ValueHelper.ResolveType(out left, out right, left, right);

            Value output = new Value();

            switch(left.DataType)
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
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);


            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) * Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;


            return output;
        }

        private Value Divide(ExpressionNode node)
        {
            Value left = InterpretExpression(node.Left);
            Value right = InterpretExpression(node.Right);
            
            left =  ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);

            if (Convert.ToDouble(right.Data) == 0)
            {
                throw new DivideByZeroException();
            }

            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) / Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;

            return output;
        }
    }
}
