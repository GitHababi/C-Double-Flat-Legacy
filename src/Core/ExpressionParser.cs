using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Errors;
using System.Web.Script.Serialization;

namespace C_Double_Flat.Core
{
    public class ConditionParser
    {
        public static ConditionNode Parse(List<Token> tokens)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (tokens.Count == 0) throw new Exception
            ("Under no circumstances should this throw, YOUR INPUTS SHOULD BE MORE SANITIZED!!!");

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
            output.Left = ExpressionParser.ParseLR(tokens.ToArray().Take(index).ToList());
            output.Right = ExpressionParser.ParseLR(tokens.ToArray().Skip(index + 1).ToList());
            return output;
        }
    }

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
            if (index > tokens.Count) throw new ExpectedTokenException(tokens[index - 1].Position, type.ToString());
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
        private List<ExpNode> ParseArgs()
        {
            List<ExpNode> output = new List<ExpNode>();

            tokens = TokenHelper.getFromParenthesis(tokens);
            index += tokens.Count;
            List<List<Token>> tokentoken = TokenHelper.Split(tokens, TokenType.COMMA);
            foreach (List<Token> t in tokentoken)
            {
                output.Add(ExpressionParser.ParseLR(t));
            }
            // kill me
            

            return output;
        }
    }

    public class ExpressionParser
    {
        public static ExpNode ParseLR(List<Token> tokens)
        {
            return new ExpressionParser(tokens).Parse();
        }

        private List<Token> tokens;
        private int index = 0;
        private ExpNode output = ExpNode.None;

        private ExpressionParser(List<Token> tokens)
        {
            this.tokens = tokens;
            this.tokens.Add(TokenHelper.None);
        }

        private ExpNode Parse()
        {

            while (!Object.Equals(tokens[index], TokenHelper.None))
            {
                output = ParseOperation();
            }

            return output;
        }

        private ExpNode ParseOperation()
        {
            ExpNode term1;
            if (output == ExpNode.None)
            {
                term1 = ParseTerm();
                index++;
            }
            else
            {
                term1 = output;
            }
            ExpNode operation = ParseOperator();
            if (operation.Type == TokenType.NONE) return term1;
            index++;
            ExpNode term2 = ParseTerm();
            index++;
            operation.Right = term2;
            operation.Left = term1;
            return operation;
        }

        private ExpNode ParseTerm()
        {
            switch (tokens[index].Type)
            {
                case TokenType.IDENTIFIER:
                    if (tokens.Count > index + 1)
                    {
                        if (tokens[index + 1].Type == TokenType.INSRT)
                        {
                            int amount = 0;
                            FuncCallNode output = null;
                            FunctionCallParser.Parse(tokens.ToArray().Skip(index).ToList(), out amount, out output);

                            index += amount + 1;
                            return output;
                        }
                    }
                    return new ExpNode(tokens[index]);
                case TokenType.NUMBER:
                    return new ExpNode(tokens[index]);
                case TokenType.STRING:
                    return new ExpNode(tokens[index]);
                case TokenType.BOOL:
                    return new ExpNode(tokens[index]);
                case TokenType.LPAREN:
                    List<Token> n = TokenHelper.getFromParenthesis(tokens, index);
                    index += n.Count + 1;
                    return ExpressionParser.ParseLR(n);
                default:
                    throw new InvalidTokenException(tokens[index].Position);
            }
        }
        private ExpNode ParseOperator()
        {
            switch (tokens[index].Type)
            {
                case TokenType.ADD:
                    return new ExpNode(tokens[index]);
                case TokenType.SUB:
                    return new ExpNode(tokens[index]);
                case TokenType.DIV:
                    return new ExpNode(tokens[index]);
                case TokenType.MUL:
                    return new ExpNode(tokens[index]);
                case TokenType.NONE:
                    return new ExpNode(TokenHelper.None);

                default:
                    Console.WriteLine(tokens[index]);
                    throw new InvalidTokenException(tokens[index].Position);

            }
        }
    }
}