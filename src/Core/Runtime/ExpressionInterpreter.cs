using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace C_Double_Flat.Core.Runtime
{
    public class ExpressionInterpreter
    {
        public static Value Interpret(ExpressionNode node)
        {
            return new ExpressionInterpreter(node).Private_Interpret();
        }


        private ExpressionNode node;

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
            }

            return output;
        }
        private Value Subtract()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left);
            Value right = ExpressionInterpreter.Interpret(node.Right);
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);

            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) - Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;

            return output;
        }

        private Value Add()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left);
            Value right = ExpressionInterpreter.Interpret(node.Right);
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
            Value left = ExpressionInterpreter.Interpret(node.Left);
            Value right = ExpressionInterpreter.Interpret(node.Right);
            
            left = ValueHelper.CastValue(left, ValueType.NUMBER);
            right = ValueHelper.CastValue(right, ValueType.NUMBER);


            Value output = new Value();

            output.Data = (Convert.ToDouble(left.Data) * Convert.ToDouble(right.Data)).ToString();
            output.DataType = ValueType.NUMBER;


            return output;
        }

        private Value Divide()
        {
            Value left = ExpressionInterpreter.Interpret(node.Left);
            Value right = ExpressionInterpreter.Interpret(node.Right);
            
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
