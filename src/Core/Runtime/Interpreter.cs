using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core.Runtime
{
    public partial class Interpreter
    {
        public static Dictionary<string, Value> globalVars = new Dictionary<string, Value>();
        public static Dictionary<string, IFunction> Functions = new Dictionary<string, IFunction>();

        public static Value Interpret(List<Statement> Statements, bool isScoped = false)
        {
            return new Interpreter(Statements, isScoped).Private_Interpret();
        }

        /// <summary>
        /// This overload is specifically used in IF or LOOP statements so you can tell if something was actually returned.
        /// </summary>
        /// <param name="Statements"></param>
        /// <param name="scope"></param>
        /// <param name="isScoped"></param>
        /// <returns></returns>


        public static Value Interpret(List<Statement> Statements, ref Dictionary<string, Value> scope, out bool Returned, bool isScoped = false)
        {
            // my brain is melting
            Interpreter interpreter = new Interpreter(Statements, isScoped);
            interpreter.scopedVars = scope;
            Value output = interpreter.Private_Interpret();
            Returned = interpreter.Didreturned;
            scope = interpreter.scopedVars;
            return output;

        }

        private List<Statement> statements = new List<Statement>();
        private readonly bool isScoped;
        private bool Didreturned = false;
        public Dictionary<string, Value> scopedVars = new Dictionary<string, Value>();
        private int index = 0;


        private Interpreter(List<Statement> statements, bool isScoped)
        {
            this.statements = statements;
            this.isScoped = isScoped;
        }

        private Value Private_Interpret()
        {
            for (int i = 0; i < statements.Count; i++)
            {
                index = i;
                if (statements[index].GetType() == typeof(ASSIGN)) AssignVar();
                else if (statements[index].GetType() == typeof(EXPRESSION)) RunExpression();
                else if (statements[index].GetType() == typeof(FUNCTION)) { AssignFunction(); }
                else if (statements[index].GetType() == typeof(RETURN)) { return Return(); }
                else if (statements[index].GetType() == typeof(IF))
                {
                    Value output = IfStatement(out bool didReturn);
                    if (didReturn)
                    {
                        return output;
                    }
                }
                else if (statements[index].GetType() == typeof(LOOP))
                {
                    Value output = LoopStatement(out bool didReturn);
                    if (didReturn) { return output; }
                }
            }
            return Value.Default;
        }

        private void AssignVar()
        {
            ASSIGN assigner = (ASSIGN)statements[index];

            if (isScoped)
            {
                Value newValue = InterpretExpression(assigner.Value);
                scopedVars.Remove(assigner.Identifier.Value);
                scopedVars.Add(assigner.Identifier.Value, newValue);
            }
            else
            {
                Value newValue = InterpretExpression(assigner.Value);
                globalVars.Remove(assigner.Identifier.Value);
                globalVars.Add(assigner.Identifier.Value, newValue);
            }
        }

        private void RunExpression()
        {
            EXPRESSION expression = (EXPRESSION)statements[index];
            InterpretExpression(expression.Value);
        }

        private void AssignFunction()
        {
            FUNCTION func = (FUNCTION)statements[index];
            IFunction function = new User_Function(func.Arguments, func.Statements);
            Functions.Remove(func.Identifier.Value);
            Functions.Add(func.Identifier.Value, function);
        }

        private Value Return()
        {
            RETURN expression = (RETURN)statements[index];

            Didreturned = true;

            return InterpretExpression(expression.Value);
        }

        private Value IfStatement(out bool didReturn)
        {
            IF statement = (IF)statements[index];



            if (Check(statement.Condition))
            {
                return Interpret(statement.If, ref scopedVars, out didReturn, isScoped);
            }
            else
            {
                return Interpret(statement.Else, ref scopedVars, out didReturn, isScoped);
            }
        }

        private Value LoopStatement(out bool didReturn)
        {
            LOOP statement = (LOOP)statements[index];

            Value output = Value.Default;
            didReturn = false;

            while (Check(statement.Condition))
            {
                output = Interpret(statement.Statements, ref scopedVars, out didReturn, isScoped);
                if (didReturn) break;
            }

            return output;
        }


    }
}
