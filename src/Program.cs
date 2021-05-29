using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Core;
using System.IO;

namespace C_Double_Flat
{
    class Program
    {
        public static void Main(string[] args)
        {
            
            
            while (true)
            {
                var Tokens = new List<Token>();
                string input = Console.ReadLine();

                if (File.Exists(input)) 
                {
                    new Lexer(File.ReadAllText(input), out Tokens);
                }
                else
                {
                    new Lexer(input, out Tokens);
                }

                for (int x = 0; x < Tokens.Count; x++)
                {
                    Token token = Tokens[x];
                    if (token.Type != TokenType.ERROR)
                    {
                        Console.WriteLine(token.ToString());
                    }
                    else
                    {
                        Console.WriteLine("ERROR: " + token.Value);
                    }
                }
            }
        }
    }
}
