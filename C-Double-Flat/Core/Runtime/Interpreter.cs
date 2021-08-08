using C_Double_Flat.Core.Parser;
using System.Collections.Generic;
using System.IO;
using System;

namespace C_Double_Flat.Core.Runtime
{
    public partial class Interpreter
    {
        public static Dictionary<string, Value> GlobalVars = new();
        public static Dictionary<string, IFunction> Functions = Core.Standard.Standard.Library;


        /// <summary>
        /// Interpret a parsed set of statements.
        /// </summary>
        /// <param name="Statements">The statements to interpret</param>
        /// <param name="dir">The location of where interpreting should occur (if not specified, use Program.ProgramLocation)</param>
        /// <param name="isScoped">Only used in nested calls like within functions, (not needed for majority of cases).</param>
        /// <returns></returns>

        public static Value Interpret(List<Statement> Statements, string dir, bool isScoped = false)
        {
            return new Interpreter(Statements, isScoped, dir).Private_Interpret();
        }

        private static Value Interpret(List<Statement> Statements, ref Dictionary<string, Value> scope, string dir, out bool Returned, bool isScoped = false)
        {
            Interpreter interpreter = new(Statements, isScoped, dir)
            {
                scopedVars = scope
            };
            Value output = interpreter.Private_Interpret();
            Returned = interpreter.Didreturned;
            scope = interpreter.scopedVars;
            return output;

        }

        private readonly List<Statement> statements = new();
        private readonly bool isScoped;
        private bool Didreturned = false;
        public Dictionary<string, Value> scopedVars = new();
        private int index = 0;
        public string currentDir;

        private Interpreter(List<Statement> statements, bool isScoped, string currentDir)
        {
            this.statements = statements;
            this.isScoped = isScoped;
            this.currentDir = currentDir;
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
                else if (statements[index].GetType() == typeof(REPEAT))
                {
                    Value output = RepeatStatement(out bool didReturn);
                    if (didReturn) return output;
                }
                else if (statements[index].GetType() == typeof(RUN)) RunRun();
            }
            return Value.Default;
        }

        private void RunRun()
        {
            RUN runningstatement = (RUN)statements[index];
            string incomplete_path = InterpretExpression(runningstatement.Path).CastValue(ValueType.STRING).Data;

            string fullstring = "";
            string path = "";
            try
            {
                path = Path.Combine(currentDir, incomplete_path);
                fullstring = File.ReadAllText(path);
            }
            catch { }
            Interpreter.Interpret(StatementParser.Parse(Lexer.Tokenize(fullstring), false), path);

        }

        private void AssignVar()
        {
            ASSIGN assigner = (ASSIGN)statements[index];
            string name;

            if (assigner.IsAsName)
            {
                name = InterpretExpression(assigner.AsNameAssigner.Left).Data;
            }
            else
            {
                name = assigner.Identifier.Value;
            }

            if (isScoped)
            {
                Value newValue = InterpretExpression(assigner.Value);
                if (!GlobalVars.TryGetValue(name, out Value _)) // if the global doesnt exist, do it locally
                {
                    scopedVars.Remove(name);
                    scopedVars.Add(name, newValue);
                }
                else
                {
                    GlobalVars.Remove(name);
                    GlobalVars.Add(name, newValue);
                }

            }
            else
            {
                Value newValue = InterpretExpression(assigner.Value);
                GlobalVars.Remove(name);
                if (Functions.ContainsKey(name)) Functions.Remove(name);
                GlobalVars.Add(name, newValue);
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

            string id;
            if (func.IsAsName)
            {
                id = InterpretExpression(func.AsNameAssigner.Left).Data;
            }
            else
            {
                id = func.Identifier.Value;
            }

            IFunction function = new User_Function(func.Arguments, func.Statements);
            Functions.Remove(id);
            if (GlobalVars.TryGetValue(id, out Value _)) GlobalVars.Remove(id);
            Functions.Add(id, function);
        }

        private Value Return()
        {
            RETURN expression = (RETURN)statements[index];

            Didreturned = true;

            return InterpretExpression(expression.Value);
        }

        private Value RepeatStatement(out bool didReturn)
        {
            REPEAT statement = (REPEAT)statements[index];

            Value output = Value.Default;
            didReturn = false;

            int amount = Convert.ToInt32(Math.Floor(Convert.ToDouble(InterpretExpression(statement.Amount).CastValue( ValueType.NUMBER).Data)));
            
            for (int x = 0; x < amount; x++)
            {
                output = Interpret(statement.Statements, ref scopedVars, currentDir, out didReturn, isScoped);
                if (didReturn) break;
            }
            return output;
        }

        private Value IfStatement(out bool didReturn)
        {
            IF statement = (IF)statements[index];



            if (Check(statement.Condition))
            {
                return Interpret(statement.If, ref scopedVars, currentDir, out didReturn, isScoped);
            }
            else
            {
                return Interpret(statement.Else, ref scopedVars, currentDir, out didReturn, isScoped);
            }
        }

        private Value LoopStatement(out bool didReturn)
        {
            LOOP statement = (LOOP)statements[index];

            Value output = Value.Default;
            didReturn = false;

            while (Check(statement.Condition))
            {
                output = Interpret(statement.Statements, ref scopedVars, currentDir, out didReturn, isScoped);
                if (didReturn) break;
            }

            return output;
        }


    }
}
