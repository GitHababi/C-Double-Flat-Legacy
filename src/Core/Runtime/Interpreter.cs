using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core.Runtime
{
    public class Interpreter
    {
        public static Dictionary<string, Value> globalVars;
        public static Dictionary<string, IFunction> Functions;
        
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


        public static Value Interpret(List<Statement> Statements, out Dictionary<string, Value> scope, out bool Returned, bool isScoped = false)
        {
            // my brain is melting
            Interpreter interpreter = new Interpreter(Statements, isScoped);
            Value output = interpreter.Private_Interpret();
            scope = interpreter.scopedVars;
            Returned = interpreter.Didreturned;
            return output;
            
        }

        private List<Statement> statements = new List<Statement>();
        private bool isScoped;
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
                else if (statements[index].GetType() == typeof(FUNCTION)) AssignFunction();
                else if (statements[index].GetType() == typeof(RETURN)) { return Return(); }
                else if (statements[index].GetType() == typeof(IF))
                {
                    bool didReturn = false;
                    Value output = IfStatement(out didReturn);
                    if (didReturn)
                    {
                        return output;
                    }
                }
                else if (statements[index].GetType() == typeof(LOOP))
                {
                    bool didReturn = false;
                    Value output = LoopStatement(out didReturn);
                    if (didReturn)
                    {
                        return output;
                    }
                }
            }
            return Value.Default;
        }

        private void AssignVar()
        {
            ASSIGN assigner = (ASSIGN)statements[index];

            if (isScoped) scopedVars.Add(assigner.Identifier.Value, ExpressionInterpreter.Interpret(assigner.Value));
            else globalVars.Add(assigner.Identifier.Value, ExpressionInterpreter.Interpret(assigner.Value));
        }

        private void RunExpression()
        {
            EXPRESSION expression = (EXPRESSION)statements[index];
            ExpressionInterpreter.Interpret(expression.Value);
        }

        private void AssignFunction()
        {
            FUNCTION func = (FUNCTION)statements[index];
            Functions.Add(func.Identifier.Value, new User_Function(func.Arguments, func.Statements));
        }

        private Value Return()
        {
            RETURN expression = (RETURN)statements[index];

            Didreturned = true;

            return ExpressionInterpreter.Interpret(expression.Value);
        }

        private Value IfStatement(out bool didReturn)
        {
            IF statement = (IF)statements[index];



            if (ConditionInterpreter.Check(statement.Condition))
            {
                return Interpret(statement.If, out scopedVars, out didReturn, isScoped);
            }
            else
            {
                return Interpret(statement.Else, out scopedVars, out didReturn, isScoped);
            }
        }

        private Value LoopStatement(out bool didReturn)
        {
            LOOP statement = (LOOP)statements[index];
            Value output = Value.Default;
            
            didReturn = false;
            while(ConditionInterpreter.Check(statement.Condition))
            {
                Interpret(statement.Statements, out scopedVars, out didReturn, isScoped);
                if (didReturn) break;
            }
            return output;
        }
    }
}
