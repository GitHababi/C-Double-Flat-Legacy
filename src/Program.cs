using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using C_Double_Flat.Core.Parser;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace C_Double_Flat
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                List<Token> Tokens = new List<Token>();
                if (File.Exists(args[0]))
                {
                    try
                    {

                        Tokens = Lexer.Tokenize(File.ReadAllText(args[0]));

                        Console.WriteLine("Lexer Result:");

                        foreach (Token token in Tokens) Console.WriteLine(token);

                        Console.WriteLine("\nParser Result:");

                        List<Statement> statements = StatementParser.Parse(Tokens, false);

                        foreach (Statement statement in statements) Console.WriteLine(statement);

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
                    Console.WriteLine("Lexer Result:");

                    foreach (Token token in Tokens) Console.WriteLine(token);


                    Console.WriteLine("\nParser Result:");

                    List<Statement> statements = StatementParser.Parse(Tokens, false);

                    foreach (Statement statement in statements) Console.WriteLine(statement);
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }
    }
}
