using C_Double_Flat.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C_Double_Flat.Core.Parser
{
    public class ConditionParser
    {
        public static ConditionNode Parse(List<Token> tokens)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (tokens.Count == 0) throw new ArgumentNullException("tokens");

            ConditionNode output;

            int index = 0;

            while (true)
            {
                if (
                    tokens[index].Type == TokenType.EQAL ||
                    tokens[index].Type == TokenType.NOT_EQAL ||
                    tokens[index].Type == TokenType.LESS ||
                    tokens[index].Type == TokenType.GRTR ||
                    tokens[index].Type == TokenType.LESS_EQ ||
                    tokens[index].Type == TokenType.GRTR_EQ
                    )
                {
                    output = new ConditionNode(tokens[index]);
                    break;
                }
                if (index + 1 < tokens.Count) index++;
                else throw new InvalidTokenException(tokens[index].Position);
            }
            output.Left = ExpressionParser.Parse(tokens.ToArray().Take(index).ToList());
            output.Right = ExpressionParser.Parse(tokens.ToArray().Skip(index + 1).ToList());
            return output;
        }
    }
}
