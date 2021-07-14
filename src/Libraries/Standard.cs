using C_Double_Flat.Core;
using C_Double_Flat.Core.Parser;
using C_Double_Flat.Core.Runtime;
using C_Double_Flat.Errors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace C_Double_Flat.Libraries
{
    class Standard
    {
        public static Dictionary<string, IFunction> Library = new()
        {
            { "help", new Libraries.REPL_Help() },
            { "exit", new Libraries.REPL_Exit() },
            { "disp_echo", new Libraries.Display_Echo() },
            { "disp_prompt", new Libraries.Display_Prompt() },
            { "disp_clear", new Libraries.Display_Clear() },
            { "disp_hide", new Libraries.Display_Hide() },
            { "disp_show", new Libraries.Display_Show() },
            { "dbug_vars", new Libraries.Debug_Vars() },
            // Uncomment these two lines to enable debug functions.
            { "dbug_parse", new Libraries.Debug_Parse() },
            { "math_abs", new Libraries.Math_Abs() },
            { "math_round", new Libraries.Math_Round() },
            { "math_rand", new Libraries.Math_Rand() },
            { "math_mod", new Libraries.Math_Modulo() },
            { "math_pow", new Libraries.Math_Power() },
            { "math_sqrt", new Libraries.Math_Sqrt() },
            { "file_read", new Libraries.File_Read() },
            { "file_save", new Libraries.File_Save() },
            { "file_copy", new Libraries.File_Copy() },
            { "file_delete", new Libraries.File_Delete() },
            { "string_has", new Libraries.String_Contains() },
            { "string_lower", new Libraries.String_Lower() },
            { "string_upper", new Libraries.String_Upper() },
            { "string_length", new Libraries.String_Length() },
            { "call", new Libraries.Call() },
            { "wait", new Libraries.Wait() },

        };
    }
    #region Other
    class Wait : IFunction
    {
        string IFunction.Description()
        {
            return "Waits the amount of milliseconds specified in the first argument before continuing running the program";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "wait");

            int amount = Convert.ToInt32(Math.Floor(Convert.ToDouble(ValueHelper.CastValue(Inputs[0], Core.ValueType.NUMBER).Data)));
            Thread.Sleep(amount);
            return Value.Default;
        }
    }


    class Call : IFunction
    {
        string IFunction.Description()
        {
            return "Calls the specified function in the first argument, and passes any other argument to the called function.";
        }
        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "call");

            if (Interpreter.Functions.TryGetValue(Inputs[0].Data, out IFunction function))
            {
                Inputs.RemoveAt(0);
                List<Value> args = new();
                Inputs.ForEach(args.Add);
                return function.Run(args);
            }
            return Value.Default;
        }
    }
    #endregion
    class Template : IFunction
    {
        string IFunction.Description()
        {
            return "";
        }
        Value IFunction.Run(List<Value> Inputs)
        {
            return Value.Default;
        }
    }

    #region Math
    class Math_Rand : IFunction
    {
        public string Description()
        {
            return "Returns a random integer between the value of the first, and second argument.";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2, "math_rand");

            int lower = (int)Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data));
            int upper = (int)Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[1], Core.ValueType.NUMBER).Data));

            Random random = new();
            return new Value(random.Next(lower, upper).ToString(), Core.ValueType.NUMBER);
        }
    }
    class Math_Round : IFunction
    {
        public string Description()
        {
            return "Returns a rounded value of the first argument ";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 1) throw new ArgumentCountException(1, "math_round");

            return new Value(Math.Round(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data)).ToString(), Core.ValueType.NUMBER);
        }
    }
    class Math_Modulo : IFunction
    {
        public string Description()
        {
            return "Returns the result of the modulo operator between the first and the second argument";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2, "math_mod");
            double left = Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data);
            double right = Convert.ToDouble(ValueHelper.CastValue(values[1], Core.ValueType.NUMBER).Data);
            double returnval = left % right;
            return new Value(returnval.ToString(), Core.ValueType.NUMBER);
        }
    }
    class Math_Power : IFunction
    {
        public string Description()
        {
            return "Returns the power of the first argument to the second argument";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2, "math_pow");
            double left = Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data);
            double right = Convert.ToDouble(ValueHelper.CastValue(values[1], Core.ValueType.NUMBER).Data);
            return new Value(Math.Pow(left, right).ToString(), Core.ValueType.NUMBER);
        }
    }
    class Math_Sqrt : IFunction
    {
        public string Description()
        {
            return "Returns the square root of the first argument";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 1) throw new ArgumentCountException(1, "math_sqrt");
            return new Value(Math.Sqrt(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data)).ToString(), Core.ValueType.NUMBER);
        }
    }
    class Math_Abs : IFunction
    {
        public string Description()
        {
            return "Returns the absolute value of the first argument";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 1) throw new ArgumentCountException(1, "math_abs");
            return new Value(Math.Abs(Convert.ToDouble(ValueHelper.CastValue(values[0], Core.ValueType.NUMBER).Data)).ToString(), Core.ValueType.NUMBER);

        }
    }
    #endregion
    #region File
    class File_Read : IFunction
    {
        public string Description()
        {
            return "Returns all text from the path given in the first argument";
        }
        public Value Run(List<Value> values)
        {
            string output = "";
            if (values.Count < 1) throw new ArgumentCountException(1, "file_read");
            try
            {
                output = File.ReadAllText(values[0].Data);
            }
            catch { }
            return new Value(output, Core.ValueType.STRING);
        }
    }
    class File_Save : IFunction
    {
        public string Description()
        {
            return "Saves all text in the second argument into the file path of the first argument";
        }
        public Value Run(List<Value> values)
        {
            if (values.Count < 2) throw new ArgumentCountException(2, "file_save");
            try
            {
                File.WriteAllText(values[0].Data, values[1].Data);
            }
            catch { }
            return Value.Default;
        }
    }
    class File_Copy : IFunction
    {
        string IFunction.Description()
        {
            return "Copies the file in the first argument into the file path of the second argument";
        }
        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 2) throw new ArgumentCountException(2, "file_copy");
            try
            {
                File.Copy(Inputs[0].Data, Inputs[1].Data);
            }
            catch { }
            return Value.Default;
        }
    }
    class File_Delete : IFunction
    {
        string IFunction.Description()
        {
            return "Deletes the file from the path given in the first argument";
        }
        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "file_delete");
            if (Inputs[0].Data == null) throw new ArgumentCountException(1, "file_delete");

            try
            {
                File.Delete(Inputs[0].Data);
            }
            catch { }

            return Value.Default;
        }
    }
    #endregion
    #region Display
    class Display_Hide : IFunction
    {
        string IFunction.Description()
        {
            return "Hides console window from view and input, but it does not close Cbb when hidden.";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            Program.ShowWindow(Program.GetConsoleWindow(), 0);
            return Value.Default;
        }
    }

    class Display_Show : IFunction
    {
        string IFunction.Description()
        {
            return "Shows console window.";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            Program.ShowWindow(Program.GetConsoleWindow(), 1);
            return Value.Default;
        }
    }

    class Display_Echo : IFunction
    {
        public string Description()
        {
            return "Writes all of the arguments to the console window";
        }
        public Value Run(List<Value> Inputs)
        {
            string write = "";

            foreach (Value v in Inputs) write += v.Data;
            Console.WriteLine(write);
            return Value.Default;
        }
    }
    class Display_Prompt : IFunction
    {
        public string Description()
        {
            return "Returns the next input line from the console window";
        }
        public Value Run(List<Value> Inputs)
        {
            return new Value(Console.ReadLine(), Core.ValueType.STRING);
        }
    }
    class Display_Clear : IFunction
    {
        public string Description()
        {
            return "Clears the console window";
        }
        public Value Run(List<Value> Inputs) { Console.Clear(); return Value.Default; }
    }
    #endregion
    #region Debug

    class Debug_Parse : IFunction
    {
        string IFunction.Description()
        {
            return "Parses the first argument, then lists the parser result in the console window";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "dbug_parse");
            var a = StatementParser.Parse(Lexer.Tokenize(Inputs[0].Data), false);
            string output = "";
            foreach (var n in a) output += n;
            return new Value(output, Core.ValueType.STRING);
        }
    }

    class Debug_Vars : IFunction
    {
        public string Description()
        {
            return "Lists all global variables in the console window";
        }
        public Value Run(List<Value> Inputs)
        {
            foreach (string key in Interpreter.GlobalVars.Keys)
            {
                Interpreter.GlobalVars.TryGetValue(key, out Value val);
                Console.WriteLine(key + " OF TYPE: " + val.DataType + " = " + val.Data);
            }
            return Value.Default;
        }
    }
    #endregion
    #region REPL
    class REPL_Help : IFunction
    {
        string IFunction.Description()
        {
            return "Display help for the REPL environment";
        }

        Value IFunction.Run(List<Value> Inputs)
        {

            if (Inputs.Count == 1)
            {
                if (Inputs[0].Data != null)
                {
                    if (Inputs[0].Data.ToLower() == "functions")
                    {
                        Console.WriteLine("Available Functions:");
                        foreach (string key in Interpreter.Functions.Keys)
                        {
                            Interpreter.Functions.TryGetValue(key, out IFunction function);
                            Console.WriteLine(String.Format("{0} => {1}", key, function.Description()));
                        }
                    }
                    else if (Inputs[0].Data.ToLower() == "syntax")
                    {
                        Console.WriteLine("Syntax Help:\n" +
                            "To see help regarding syntax, please refer to the github repository for Cbb.");
                    }
                    else
                    {
                        Console.WriteLine("A programming language that you could use, but shouldn't.\n" +
                        "For help regarding available functions, type 'help<-(\"functions\");'\n" +
                        "For help regarding syntax, type 'help<-(\"syntax\");'\n");
                    }
                }
                else
                {
                    Console.WriteLine("A programming language that you could use, but shouldn't.\n" +
                    "For help regarding available functions, type 'help<-(\"functions\");'\n" +
                    "For help regarding syntax, type 'help<-(\"syntax\");'\n");
                }
            }
            else
            {
                Console.WriteLine("A programming language that you could use, but shouldn't.\n" +
                "For help regarding available functions, type 'help<-(\"functions\");'\n" +
                "For help regarding syntax, type 'help<-(\"syntax\");'\n");
            }
            return Value.Default;
        }
    }
    class REPL_Exit : IFunction
    {
        string IFunction.Description()
        {
            return "Immediately exits from the Cbb Program";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            Environment.Exit(0);
            return Value.Default; // Lol, returning in a function that calls exit.
        }
    }
    #endregion
    #region String
    class String_Contains : IFunction
    {
        string IFunction.Description()
        {
            return "Returns true if the first argument contains the second argument as a string, otherwise returns false";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 2) throw new ArgumentCountException(2, "string_has");
            bool output = Inputs[0].Data.Contains(Inputs[1].Data);
            return new Value(output.ToString(), Core.ValueType.BOOL);
        }
    }
    class String_Upper : IFunction
    {
        string IFunction.Description()
        {
            return "Takes first argument, then returns the string value in uppercase";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "string_upper");
            if (Inputs[0].Data == null) throw new ArgumentCountException(1, "string_upper");
            return new Value(Inputs[0].Data.ToUpper(), Core.ValueType.STRING);
        }
    }
    class String_Lower : IFunction
    {
        string IFunction.Description()
        {
            return "Takes first argument, then returns the string value in uppercase";
        }

        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "string_lower");
            if (Inputs[0].Data == null) throw new ArgumentCountException(1, "string_lower");
            return new Value(Inputs[0].Data.ToLower(), Core.ValueType.STRING);
        }
    }
    class String_Length : IFunction
    {
        string IFunction.Description()
        {
            return "Returns the string length of the first argument";
        }
        Value IFunction.Run(List<Value> Inputs)
        {
            if (Inputs.Count < 1) throw new ArgumentCountException(1, "string_length");
            if (Inputs[0].Data == null) throw new ArgumentCountException(1, "string_length");
            return new Value(Inputs[0].Data.Length.ToString(), Core.ValueType.NUMBER);
        }
    }
    #endregion
}
