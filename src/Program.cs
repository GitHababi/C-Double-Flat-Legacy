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
        public static string ProgramLocation = Directory.GetCurrentDirectory();

        public static void Main(string[] args)
        {

            Interpreter.Functions.Append(Libraries.Standard.Library);
            bool runWithArgs = false;

            if (args.Length > 0)
            {
                List<Token> Tokens = new List<Token>();
                if (File.Exists(args[0]))
                {
                    try
                    {

                        Tokens = Lexer.Tokenize(File.ReadAllText(args[0]));

                        runWithArgs = true;
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
            
            if (!runWithArgs)
            {
                Console.WriteLine("C Double Flat - REPL v1.1");
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

                    string output = Interpreter.Interpret(StatementParser.Parse(Tokens, false), ProgramLocation).Data + " <<<" ;
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
