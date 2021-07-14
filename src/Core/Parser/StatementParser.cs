using C_Double_Flat.Errors;
using System.Collections.Generic;
using System.Linq;

namespace C_Double_Flat.Core.Parser
{
    public class StatementParser
    {
        public static List<Statement> Parse(List<Token> tokens, bool isNested)
        {
            return new StatementParser(tokens, isNested).Private_Parse();
        }

        private List<Token> tokens;
        private int index;
        private bool isNested;

        private StatementParser(List<Token> tokens, bool isNested)
        {
            this.tokens = tokens;
            this.tokens.Add(TokenHelper.None);
            this.isNested = isNested;
            index = 0;
        }

        private Token currentToken { get { return tokens[index]; } set { } }

        private List<Statement> Private_Parse()
        {
            List<Statement> output1 = new List<Statement>();

            while (true)
            {
                if (currentToken.Type == TokenType.NONE) break;
                output1.Add(ParseStatement());
            }

            return output1;
        }

        private TokenType Peek(int amount = 1)
        {
            if (amount + index < tokens.Count) return tokens[index + amount].Type;
            else throw new IncompleteExpressionException(tokens[index].Position);
        }

        private void Expect(TokenType type)
        {
            if (currentToken.Type != type) throw new ExpectedTokenException(new Position(tokens[index - 1].Position._index, tokens[index - 1].Position._line, tokens[index - 1].Position._col +1), type.ToString());
        }

        private int NextStatementEnding()
        {
            int out_x = 0;
            for (int x = index; x < tokens.Count - 1; x++)
            {
                out_x = x;
                if (tokens[x].Type == TokenType.NXTLN) return x;
            }
            throw new ExpectedTokenException(tokens[out_x].Position, ";");
        }

        private Statement ParseStatement()
        {
            switch (currentToken.Type)
            {
                case TokenType.IDENTIFIER:
                    switch (Peek())
                    {
                        case TokenType.ASSGN:
                            if (Peek(2) == TokenType.LCURLY)
                            {
                                if (isNested) throw new FunctionDeclarationException(tokens[index].Position);
                                return ParseFunctionAssignment();
                            }
                            else return ParseAssignment();
                        default:
                            return ParseExpressionStatement();
                    }
                case TokenType.NXTLN:
                    index++;
                    return new NONE(); // No-op 
                case TokenType.RETURN:
                    return ParseReturn();
                case TokenType.IF:
                    return ParseIf();
                case TokenType.NUMBER:
                    return ParseExpressionStatement();
                case TokenType.STRING:
                    return ParseExpressionStatement();
                case TokenType.BOOL:
                    return ParseExpressionStatement();
                case TokenType.ELSE:
                    throw new InvalidElseException(currentToken.Position);
                case TokenType.LOOP:
                    return ParseLoop();
                case TokenType.RUN:
                    return ParseRun();
                case TokenType.SUB:
                    return ParseExpressionStatement();
                case TokenType.REPEAT:
                    return ParseRepeat();
                default:
                    index++;
                    return new NONE();
            }
        }

        private REPEAT ParseRepeat()
        {
            REPEAT output = new();

            index += 1;
            Expect(TokenType.LPAREN);

            List<Token> inParenthesis = TokenHelper.GetFromParenthesis(tokens.Skip(index).ToList());
            if (inParenthesis.Count == 0) throw new EmptyConditionException(tokens[index].Position);
            output.Amount = ExpressionParser.ParseLR(inParenthesis);

            index += inParenthesis.Count + 1;
            Expect(TokenType.ASSGN);

            index++;

            int end = TokenHelper.GetMatchingBracket(tokens, index); // Find the matching } bracket to the {

            output.Statements = StatementParser.Parse(tokens.Skip(index).Take(end + 1).ToList(), true);

            index += end + 1;
            return output;
        }

        private RUN ParseRun()
        {
            RUN output = new RUN();

            index += 1; //moving one over because we just skipped past a return token.

            int endpoint = NextStatementEnding();

            if (TokenHelper.Contains(tokens.Skip(index).Take(endpoint - index).ToList(), TokenHelper.conditions))
                throw new InvalidExpressionException(tokens[index].Position);
            else
                output.Path = ExpressionParser.ParseLR(tokens.Skip(index).Take(endpoint - index).ToList());

            index += endpoint - index;

            return output;
        }

        private LOOP ParseLoop()
        {
            LOOP output = new();

            index++;
            Expect(TokenType.LPAREN);

            List<Token> inParenthesis = TokenHelper.GetFromParenthesis(tokens.Skip(index).ToList());
            if (inParenthesis.Count == 0) throw new EmptyConditionException(tokens[index].Position);
            output.Condition = ConditionParser.Parse(inParenthesis);

            index += inParenthesis.Count + 2;
            Expect(TokenType.ASSGN);

            index++;

            int end = TokenHelper.GetMatchingBracket(tokens, index); // Find the matching } bracket to the {

            output.Statements = StatementParser.Parse(tokens.Skip(index).Take(end + 1).ToList(), true);

            index += end + 1;

            return output;
        }

        private IF ParseIf()
        {
            IF output = new IF();
            index++;
            Expect(TokenType.LPAREN);
            List<Token> inParenthesis = TokenHelper.GetFromParenthesis(tokens.Skip(index).ToList());

            if (inParenthesis.Count == 0) throw new EmptyConditionException(tokens[index].Position);

            output.Condition = ConditionParser.Parse(inParenthesis);

            index += inParenthesis.Count + 2;

            Expect(TokenType.ASSGN);

            index++;

            int end = TokenHelper.GetMatchingBracket(tokens, index); // Find the matching } bracket to the {

            output.If = StatementParser.Parse(tokens.Skip(index).Take(end + 1).ToList(), true);

            index += end + 1;

            if (currentToken.Type == TokenType.ELSE)
            {
                index++;

                Expect(TokenType.ASSGN);

                index++;

                int else_end = TokenHelper.GetMatchingBracket(tokens, index); // Find the matching } bracket to the {

                output.Else = StatementParser.Parse(tokens.Skip(index).Take(else_end + 1).ToList(), true);

                index += else_end + 1;
            }
            else
            {
                output.Else = new List<Statement>();
            }

            return output;
        }

        private RETURN ParseReturn()
        {
            RETURN output = new RETURN();

            index += 1; //moving one over because we just skipped past a return token.

            int endpoint = NextStatementEnding();

            if (TokenHelper.Contains(tokens.Skip(index).Take(endpoint - index).ToList(), TokenHelper.conditions))
                output.Value = ConditionParser.Parse(tokens.Skip(index).Take(endpoint - index).ToList());
            else
                output.Value = ExpressionParser.ParseLR(tokens.Skip(index).Take(endpoint - index).ToList());

            index += endpoint - index;

            return output;
        }

        private ASSIGN ParseAssignment()
        {

            ASSIGN output = new ASSIGN();

            output.Identifier = currentToken;

            index += 2; // Adding to index because we just skipped past an assignment token and identifier

            int endpoint = NextStatementEnding();

            if (TokenHelper.Contains(tokens.Skip(index).Take(endpoint - index).ToList(), TokenHelper.conditions))
                output.Value = ConditionParser.Parse(tokens.Skip(index).Take(endpoint - index).ToList());
            else
                output.Value = ExpressionParser.ParseLR(tokens.Skip(index).Take(endpoint - index).ToList());

            index = endpoint + 1;

            return output;
        }

        private EXPRESSION ParseExpressionStatement()
        {
            EXPRESSION output = new EXPRESSION();

            int endpoint = NextStatementEnding();

            output.Value = ExpressionParser.ParseLR(tokens.Skip(index).Take(endpoint - index).ToList());

            index = endpoint;

            return output;
        }

        private FUNCTION ParseFunctionAssignment()
        {
            FUNCTION output = new FUNCTION();

            output.Identifier = currentToken; // Assign name to the function Node

            index += 2; // Because earlier we already determined that there was an identifer and an assgn token, we can skip 2

            Expect(TokenType.LCURLY);

            int end = TokenHelper.GetMatchingBracket(tokens, index); // Find the matching } bracket to the {

            output.Statements = StatementParser.Parse(tokens.Skip(index).Take(end + 1).ToList(), true); // get the statements

            index += end + 1; // increment past the tokens that we parsed into instructions.

            Expect(TokenType.INSRT); // there must be a <- operator

            index++;

            int terminal = NextStatementEnding(); // to make sure we dont accidentally take past the ; 

            Expect(TokenType.LPAREN);

            List<Token> inputs = TokenHelper.GetFromParenthesis(tokens.Skip(index).Take(terminal - index).ToList(), 0);

            List<Token> args = new List<Token>();

            foreach (Token input in inputs)
            {
                if (input.Type != TokenType.IDENTIFIER && input.Type != TokenType.COMMA)
                {
                    throw new ExpectedTokenException(input.Position, "[Identifier, ',']");
                }
                if (input.Type == TokenType.IDENTIFIER)
                {
                    args.Add(input);
                }
            }

            index += inputs.Count + 1;
            output.Arguments = args;

            return output;
        }
    }

}
