using C_Double_Flat.Errors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace C_Double_Flat.Core.Parser
{

    public partial class ExpressionParser
    {
        /// <summary>
        /// If you are developing a library or smthn, you will not need to use this method, use the StatementParser instead.
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>

        public static ExpressionNode Parse(List<Token> tokens)
        {
            return new ExpressionParser(tokens).ParsePrivate();
        }

        private List<Token> tokens;
        private int index = 0;
        private ExpressionNode output = ExpressionNode.None;

        private ExpressionParser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tokens.Add(TokenHelper.None);
        }

        private ExpressionNode ParsePrivate()
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
            ExpressionNode output1 = new();
            switch (tokens[index].Type)
            {
                case TokenType.IDENTIFIER:
                    if (tokens.Count > index + 1)
                    {
                        if (tokens[index + 1].Type == TokenType.INSRT) //Function Call Node Creation
                        {
                            FunctionCallNode funccall = new (tokens[index]);

                            index += 2;

                            funccall.Args = ParseArgs();

                            return funccall;
                        }
                    }
                    return new ExpressionNode(tokens[index]);
                case TokenType.ASNAME:

                    Position current = tokens[index].Position;

                    output1 = ParseAsName();



                    if (tokens.Count > index + 1)
                    {
                        if (tokens[index + 1].Type == TokenType.INSRT)
                        {
                            FunctionCallNode funccall = new(new Token(TokenType.ASNAME, current));

                            index += 2;

                            funccall.Left = output1;

                            funccall.Args = ParseArgs();

                            return funccall;
                        } else { return output1; }
                    }
                    else { return output1; }
                    
                case TokenType.NAMEOF:
                    if (tokens.Count > index + 1)
                    {
                        if (tokens[index + 1].Type == TokenType.IDENTIFIER)
                        {
                            index++;
                            Token myoutput = new(TokenType.STRING, tokens[index].Value, tokens[index].Position);
                            return new ExpressionNode(myoutput);
                        }
                        else if (tokens[index + 1].Type == TokenType.ASNAME)
                        {
                            return ParseAsName().Left;
                        }
                        throw new ExpectedTokenException(tokens[index].Position, "[Identifier]");
                    }
                    throw new ExpectedTokenException(tokens[index].Position, "[Identifier]");
                case TokenType.NUMBER:
                    return new ExpressionNode(tokens[index]);
                case TokenType.STRING:
                    return new ExpressionNode(tokens[index]);
                case TokenType.BOOL:
                    return new ExpressionNode(tokens[index]);
                case TokenType.LPAREN:
                    List<Token> n = TokenHelper.GetFromParenthesis(tokens, index);
                    index += n.Count + 1;
                    return ExpressionParser.Parse(n);
                case TokenType.SUB:
                    index++;
                    output1.Left = ParseTerm();
                    output1.Type = TokenType.MUL;
                    output1.Right = new ExpressionNode(new Token(TokenType.NUMBER, "-1", tokens[index].Position));
                    return output1;
                default:
                    throw new ExpectedTokenException(tokens[index].Position, "[Number, String, Boolean, Expression]");
            }
        }

        private ExpressionNode ParseAsName()
        {
            ExpressionNode asnamenode = new(tokens[index]);

            index++; // Move past 'asname' token

            if (tokens[index].Type != TokenType.LPAREN) throw new ExpectedTokenException(tokens[index].Position, "(");
            var sub = TokenHelper.GetFromParenthesis(tokens.Skip(index).ToList());

            index += sub.Count + 1; //increment past parenthesis

            asnamenode.Left = Parse(sub);

            return asnamenode;
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