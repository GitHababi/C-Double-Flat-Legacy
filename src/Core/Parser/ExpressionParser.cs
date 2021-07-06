using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Errors;

namespace C_Double_Flat.Core.Parser
{
    
    public class ExpressionParser
    {
        public static ExpressionNode ParseLR(List<Token> tokens)
        {
            return new ExpressionParser(tokens).Parse();
        }

        private List<Token> tokens;
        private int index = 0;
        private ExpressionNode output = ExpressionNode.None;

        private ExpressionParser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tokens.Add(TokenHelper.None);
        }

        private ExpressionNode Parse()
        {

            while (!Object.Equals(tokens[index], TokenHelper.None))
            {
                output = ParseOperation();
            }

            return output;
        }

        private ExpressionNode ParseOperation()
        {
            ExpressionNode term1;
            if (output == ExpressionNode.None)
            {
                term1 = ParseTerm();
                index++;
            }
            else
            {
                term1 = output;
            }
            ExpressionNode operation = ParseOperator();
            if (operation.Type == TokenType.NONE) return term1;
            index++;
            ExpressionNode term2 = ParseTerm();
            index++;
            operation.Right = term2;
            operation.Left = term1;
            return operation;
        }

        private ExpressionNode ParseTerm()
        {
            switch (tokens[index].Type)
            {
                case TokenType.IDENTIFIER:
                    if (tokens.Count > index + 1)
                    {
                        if (tokens[index + 1].Type == TokenType.INSRT)
                        {
                            FunctionCallParser.Parse(tokens.ToArray().Skip(index).ToList(), out int amount, out FuncCallNode output);

                            index += amount + 1;
                            return output;
                        }
                    }
                    return new ExpressionNode(tokens[index]);
                case TokenType.NUMBER:
                    return new ExpressionNode(tokens[index]);
                case TokenType.STRING:
                    return new ExpressionNode(tokens[index]);
                case TokenType.BOOL:
                    return new ExpressionNode(tokens[index]);
                case TokenType.LPAREN:
                    List<Token> n = TokenHelper.getFromParenthesis(tokens, index);
                    index += n.Count + 1;
                    return ExpressionParser.ParseLR(n);
                case TokenType.SUB:
                    index++;
                    output.Left = ParseTerm();
                    output.Type = TokenType.MUL;
                    output.Right = new ExpressionNode(new Token(TokenType.NUMBER,"-1",tokens[index].Position));
                    return output;
                default:
                    throw new ExpectedTokenException(tokens[index].Position, "[Number, String, Boolean, Expression]");
            }
        }
        private ExpressionNode ParseOperator()
        {
            switch (tokens[index].Type)
            {
                case TokenType.ADD:
                    return new ExpressionNode(tokens[index]);
                case TokenType.SUB:
                    return new ExpressionNode(tokens[index]);
                case TokenType.DIV:
                    return new ExpressionNode(tokens[index]);
                case TokenType.MUL:
                    return new ExpressionNode(tokens[index]);
                case TokenType.NONE:
                    return new ExpressionNode(TokenHelper.None);

                default:
                    throw new ExpectedTokenException(tokens[index].Position, "[+, -, /, *]");

            }
        }
    }
}