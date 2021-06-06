using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace C_Double_Flat.Core
{
    public class StatementParser
    {
        public static List<Statement> Parse(List<Token> tokens)
        {
            return new StatementParser(tokens).Private_Parse();
        }

        private List<Token> tokens;
        private List<Statement> output = new List<Statement>();

        private StatementParser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private List<Statement> Private_Parse()
        {

            List<Statement> output1 = output;
            return output1;
        }
    }
    
}
