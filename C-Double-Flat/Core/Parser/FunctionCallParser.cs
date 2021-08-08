using C_Double_Flat.Errors;
using System.Collections.Generic;
using System;
using System.Linq;

namespace C_Double_Flat.Core.Parser
{
    public partial class ExpressionParser
    {

        // Originally this function was in a different class called the FunctionCallParser, but for the sake of simplicity I moved into the same class.
        // So that is why this file only has one function and is seperate from the others.

        private List<ExpressionNode> ParseArgs()
        {
            List<ExpressionNode> output = new();
            if (tokens[index].Type != TokenType.LPAREN) throw new ExpectedTokenException(tokens[index].Position, "'('");
            var argstokens = TokenHelper.GetFromParenthesis(tokens.Skip(index).ToList());
            index += argstokens.Count + 1;
            List<List<Token>> TokenArgs = new();

            if (TokenHelper.Contains(argstokens, TokenType.LPAREN))
            {
                int local_index = 0;
                int parenthesis_count = 0;
                List<Token> stack = new();
                while (local_index < argstokens.Count)
                {
                    if (argstokens[local_index].Type == TokenType.LPAREN) { stack.Add(argstokens[local_index]); parenthesis_count++; }
                    else if (argstokens[local_index].Type == TokenType.RPAREN) { stack.Add(argstokens[local_index]); parenthesis_count--; }
                    else if (argstokens[local_index].Type == TokenType.COMMA)
                    {
                        if (parenthesis_count > 0)
                        {
                            stack.Add(argstokens[local_index]);
                        }
                        else
                        {
                            TokenArgs.Add(stack);
                            stack = new List<Token>();
                            local_index++;
                            continue;
                        }
                    }
                    else
                    {
                        stack.Add(argstokens[local_index]);
                    }
                    local_index++;
                }
                if (stack.Count != 0) TokenArgs.Add(stack);
            }
            else
            {
                TokenArgs = TokenHelper.Split(argstokens, TokenType.COMMA);
            }

            foreach (List<Token> t in TokenArgs)
            {
                output.Add(ExpressionParser.Parse(t));
            }

            return output;
        }
    }
}
