using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Core.Parser;
using C_Double_Flat.Core.Runtime;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace C_Double_Flat
{
    class Program
    {
        public static void Main(string[] args)
        {

            Interpreter.Functions.Add("disp_echo", new Libraries.Display_Echo());
            Interpreter.Functions.Add("disp_prompt", new Libraries.Display_Prompt());
            Interpreter.Functions.Add("disp_clear", new Libraries.Display_Clear());
            Interpreter.Functions.Add("dbug_funcs", new Libraries.Debug_Functions());
            Interpreter.Functions.Add("dbug_vars", new Libraries.Debug_Vars());
            Interpreter.Functions.Add("math_round", new Libraries.Math_Round());
            Interpreter.Functions.Add("math_rand", new Libraries.Math_Rand());
            Interpreter.Functions.Add("file_read", new Libraries.File_Read());
            Interpreter.Functions.Add("file_save", new Libraries.File_Save());


            if (args.Length > 0)
            {
                List<Token> Tokens = new List<Token>();
                if (File.Exists(args[0]))
                {
                    try
                    {

                        Tokens = Lexer.Tokenize(File.ReadAllText(args[0]));

                        string output = Interpreter.Interpret(StatementParser.Parse(Tokens, false)).Data + " <<<";
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(output);
                        Console.ResetColor();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

            }
            
            while ("" == "")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(">>>");
                Console.ResetColor();
                string input = Console.ReadLine();
                try
                {
                    var Tokens = new List<Token>();
                    if (File.Exists(input))
                    {
                        Tokens = Lexer.Tokenize(File.ReadAllText(input));
                    }
                    else
                    {
                        Tokens = Lexer.Tokenize(input);
                    }

                    string output = Interpreter.Interpret(StatementParser.Parse(Tokens, false)).Data + " <<<";
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(output);
                    Console.ResetColor();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
    }
}
