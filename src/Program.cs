using C_Double_Flat.Core;
using C_Double_Flat.Core.Parser;
using C_Double_Flat.Core.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace C_Double_Flat
{
    class Program
    {
        public static string ProgramLocation = Directory.GetCurrentDirectory();
        [DllImport("Kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int cmdShow);




        public static void Main(string[] args)
        {
            ShowWindow(GetConsoleWindow(), 1);
            Interpreter.Functions.Append(Libraries.Standard.Library);
            bool runWithArgs = false;

            if (args.Length > 0)
            {

                if (File.Exists(args[0]))
                {
                    try
                    {

                        runWithArgs = true;
                        List<Token> Tokens = Lexer.Tokenize(File.ReadAllText(args[0]));
                        // For some reason, combining these two lines break the program, so don't do it.
                        string output = Interpreter.Interpret(StatementParser.Parse(Tokens, false), Path.GetDirectoryName(args[0])).Data + " <<<";
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

            REPL_Loop(runWithArgs);

        }

        private static void REPL_Loop(bool runWithArgs)
        {
            if (!runWithArgs)
            {
                Console.WriteLine("C Double Flat - REPL v1.4.1");
                Console.WriteLine("Created by Heerod Sahraei");
                Console.WriteLine("Copyright (C) Hababisoft Corporation. All rights reserved.");
                Console.WriteLine("Type: 'help<-();' for help.\n");
            }

            while (true)
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

                    string output = Interpreter.Interpret(StatementParser.Parse(Tokens, false), ProgramLocation).Data + " <<<";
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(output);
                    Console.ResetColor();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
    }
    public static class Extensions
    {
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            List<KeyValuePair<K, V>> pairs = second.ToList();
            pairs.ForEach(pair => first.Add(pair.Key, pair.Value));
        }
    }
}
