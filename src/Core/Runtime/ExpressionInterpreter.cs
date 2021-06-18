using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C_Double_Flat.Core.Runtime
{
    public class ExpressionInterpreter
    {
        public static Value Interpret(ExpressionNode node, ref Dictionary<string, Value> scope)
        {
            ExpressionInterpreter interpreter = new ExpressionInterpreter(node);
            interpreter.scopeVars = scope;
            return interpreter.Private_Interpret();
        }


        private ExpressionNode node;
        private Dictionary<string, Value> scopeVars = new Dictionary<string, Value>();

        private ExpressionInterpreter(ExpressionNode node)
        {
            this.node = node;
        }

        private Value Private_Interpret()
        {
            Value output = new Value();

            switch (node.Type)
            {
                case TokenType.ADD:
                    output = Add();
                    break;
                case TokenType.SUB:
                    output = Subtract(); 
                    break;
                case TokenType.MUL:
                    output = Multiply();
                    break;
                case TokenType.DIV:
                    output = Divide();
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
                    output = GetValue();
                    break;
                case TokenType.FUNC_CALL:
                    output = CallFunction();
                    break;
            }

            return output;
        }

        private Value CallFunction()
        {
            FuncCallNode call = (FuncCallNode)node;


            if (Interpreter.Functions.TryGetValue(call.Value, out IFunction function))
            {
                return function.Run(call.Args, this.scopeVars);
            }
            return Value.Default;
        }

        private Value GetValue()
        {
            Value output = new Value();
            if(!scopeVars.TryGetValue(node.Value, out output))
            {
                if(!Interpreter.globalVars.TryGetValue(node.Value, out output))
                {
                    return Value.Default;
                }
            }
            return output;
        }

        private Value Subtract()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left, ref this.scopeVars);
            Value right = ExpressionInterpreter.Interpret(node.Right, ref this.scopeVars);
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);

            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) - Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;

            return output;
        }

        private Value Add()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left, ref this.scopeVars);
            Value right = ExpressionInterpreter.Interpret(node.Right, ref this.scopeVars);
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

        private Value Multiply()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left, ref this.scopeVars);
            Value right = ExpressionInterpreter.Interpret(node.Right, ref this.scopeVars);
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);


            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) * Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;


            return output;
        }

        private Value Divide()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left, ref this.scopeVars);
            Value right = ExpressionInterpreter.Interpret(node.Right, ref this.scopeVars);
            
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
