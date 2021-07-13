using C_Double_Flat.Errors;
using System.Collections.Generic;

namespace C_Double_Flat.Core.Parser
{
    public class FunctionCallParser
    {
        public static FuncCallNode Parse(List<Token> tokens)
        {
            return new FunctionCallParser(tokens).Parse_Internal();

        }
        public static void Parse(List<Token> tokens, out int idx, out FuncCallNode result)
        {
            FunctionCallParser function = new FunctionCallParser(tokens);
            result = function.Parse_Internal();
            idx = function.index;
        }

        private List<Token> tokens;
        private int index = 0;
        private FuncCallNode output;

        private FunctionCallParser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        private void Expect(TokenType type)
        {
            if (index >= tokens.Count) throw new ExpectedTokenException(tokens[index - 1].Position, type.ToString());
            if (tokens[index].Type != type) throw new ExpectedTokenException(tokens[index].Position, type.ToString());
        }

        private FuncCallNode Parse_Internal()
        {
            Expect(TokenType.IDENTIFIER);
            output = new FuncCallNode(tokens[index]);
            index++;
            Expect(TokenType.INSRT);
            index++;
            Expect(TokenType.LPAREN);
            output.Args = ParseArgs();

            return output;
        }
        private List<ExpressionNode> ParseArgs()
        {
            List<ExpressionNode> output = new List<ExpressionNode>();

            tokens = TokenHelper.getFromParenthesis(tokens);
            index += tokens.Count;
            List<List<Token>> TokenArgs = new List<List<Token>>();

            if (TokenHelper.Contains(tokens, TokenType.LPAREN))
            {
                int local_index = 0;
                int parenthesis_count = 0;
                List<Token> stack = new List<Token>();
                while (local_index < tokens.Count)
                {
                    if (tokens[local_index].Type == TokenType.LPAREN) { stack.Add(tokens[local_index]); parenthesis_count++; }
                    else if (tokens[local_index].Type == TokenType.RPAREN) { stack.Add(tokens[local_index]); parenthesis_count--; }
                    else if (tokens[local_index].Type == TokenType.COMMA)
                    {
                        if (parenthesis_count > 0)
                        {
                            stack.Add(tokens[local_index]);
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
                        stack.Add(tokens[local_index]);
                    }
                    local_index++;
                }
                if (stack.Count != 0) TokenArgs.Add(stack);
            }
            else
            {
                TokenArgs = TokenHelper.Split(tokens, TokenType.COMMA);
            }

            foreach (List<Token> t in TokenArgs)
            {
                output.Add(ExpressionParser.ParseLR(t));
            }

            return output;
        }
    }
}
