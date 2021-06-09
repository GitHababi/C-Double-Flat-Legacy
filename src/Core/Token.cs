using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C_Double_Flat.Errors;
namespace C_Double_Flat.Core
{
    public static class TokenHelper
    {
        public static readonly Token None = new Token(TokenType.NONE, new Position(0, 0, 1));

        public static List<List<Token>> Split(List<Token> tokens, TokenType splitter)
        {
            List<List<Token>> output = new List<List<Token>>();

            int count = NumberOf(tokens, splitter);
            if (count == 0) { output.Add(tokens); return output; }

            int index = 0;
            while (index < tokens.Count)
            {
                if (count > 0)
                {
                    index = IndexOf(tokens, splitter);
                    if (index != -1) count--;
                    output.Add(tokens.ToArray().Take(index).ToList());
                    tokens.RemoveRange(0, index + 1);
                }
                else break;
            }
            if (tokens.Count > 0) output.Add(tokens);
            return output;
        }

        public static int IndexOf(List<Token> tokens, TokenType token)
        {
            for (int x = 0; x < tokens.Count; x++)
            {
                if (tokens[x].Type == token) return x;
            }
            return -1;
        }

        public static int NumberOf(List<Token> tokens, TokenType token)
        {
            int output = 0;
            for (int x = 0; x < tokens.Count; x++)
            {
                if (tokens[x].Type == token) output++; 
            }
            return output;
        }

        public static bool Contains(List<Token> tokens, TokenType token)
        {
            for (int x = 0; x < tokens.Count; x++)
            {
                if (tokens[x].Type == token) return true;
            }
            return false;
        }

        public static List<Token> getFromBrackets(List<Token> tokens, int index = 0)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (tokens.Count == 0) throw new Exception
            ("Under no circumstances should this throw, YOUR INPUTS SHOULD BE MORE SANITIZED!!!");
            Token currentToken = tokens[index];
            while (currentToken.Type != TokenType.LCURLY)
            {
                if (index + 1 < tokens.Count) index++;
                else return tokens; // If at end and no LPAREN found, return itself as it has been fully resolved. 
                currentToken = tokens[index];
            }
            int gap = 0;
            int balance = 0;
            while (gap + index < tokens.Count)
            {
                if (currentToken.Type == TokenType.LCURLY) balance++;
                if (currentToken.Type == TokenType.RCURLY)
                {
                    balance--;
                    if (balance == 0) break;
                }
                if (gap + index + 1 < tokens.Count) gap++;
                else throw new TerminatingParenthesisException(tokens[index].Position);
                // If at end and no RPAREN found, throw error as that is not supposed to happen.
                currentToken = tokens[gap + index];
            }
            if (index + 1 == index + gap) return new List<Token>();
            return tokens.ToArray().Skip(index + 1).Take(gap - 1).ToList();
        }
        public static int getMatchingBracket(List<Token> tokens, int index = 0)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (tokens.Count == 0) throw new Exception
            ("Under no circumstances should this throw, YOUR INPUTS SHOULD BE MORE SANITIZED!!!");
            Token currentToken = tokens[index];
            while (currentToken.Type != TokenType.LCURLY)
            {
                if (index + 1 < tokens.Count) index++;
                else return 0; // If at end and no LPAREN found, return itself as it has been fully resolved. 
                currentToken = tokens[index];
            }
            int gap = 0;
            int balance = 0;
            while (gap + index < tokens.Count)
            {
                if (currentToken.Type == TokenType.LCURLY) balance++;
                if (currentToken.Type == TokenType.RCURLY)
                {
                    balance--;
                    if (balance == 0) break;
                }
                if (gap + index + 1 < tokens.Count) gap++;
                else throw new TerminatingParenthesisException(tokens[index].Position);
                // If at end and no RPAREN found, throw error as that is not supposed to happen.
                currentToken = tokens[gap + index];
            }
            return gap;
        }

        public static List<Token> getFromParenthesis(List<Token> tokens, int index = 0)
        {
            if (tokens == null) throw new ArgumentNullException("tokens");
            if (tokens.Count == 0) return new List<Token>();
            Token currentToken = tokens[index];
            while (currentToken.Type != TokenType.LPAREN)
            {
                if (index + 1 < tokens.Count) index++;
                else return tokens; // If at end and no LPAREN found, return itself as it has been fully resolved. 
                currentToken = tokens[index];
            }
            int gap = 0;
            int balance = 0;
            while (gap + index < tokens.Count)
            {
                if (currentToken.Type == TokenType.LPAREN) balance++;
                if (currentToken.Type == TokenType.RPAREN)
                {
                    balance--;
                    if (balance == 0) break;
                }
                if (gap + index + 1 < tokens.Count) gap++;
                else throw new TerminatingParenthesisException(tokens[index].Position);
                // If at end and no RPAREN found, throw error as that is not supposed to happen.
                currentToken = tokens[gap + index];
            }
            if (index + 1 == index + gap) return new List<Token>();
            return tokens.ToArray().Skip(index + 1).Take(gap - 1).ToList();
        }
    }
    public struct Token
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set; }
        public Position Position { get; private set; }

        public Token(TokenType _type, Position _position)
        {
            Position = _position;
            Type = _type;
            Value = "";
        }

        public Token(TokenType _type, string _value, Position _position)
        {
            Position = _position;
            Type = _type;
            Value = _value;
        }

        public override string ToString()
        {
            string output = (Value == null) ?
                "Type: " + Type.ToString() :
                "Type: " + Type.ToString() + " Value: " + Value.ToString();
            //If there is no value for the token, add nothing.
            return output;
        }
    }
    public struct None { /* Empty struct LOL, Don't Delete this. I need it */}
    public enum TokenType
    {
        LPAREN,
        RPAREN,
        LCURLY,
        RCURLY,
        COMMA,

        // Operators

        ADD, // +
        SUB, // -
        MUL, // *
        DIV, // /

        // Comparison Operators

        LESS, // <
        GRTR, // >
        GRTR_EQ, // >=
        LESS_EQ, // <=
        EQAL, // =
        NOT_EQAL, // !=

        // Data Types

        STRING,
        NUMBER,
        BOOL,

        // Special Operators

        ASSGN, // The : Operator
        INSRT, // The <- Operator
        NXTLN, // The ; Operator

        // Flow Control

        IF,
        ELSE,
        LOOP,
        RETURN,

        // Special Tokens

        IDENTIFIER,
        NONE, // Specifically in use for the parser, to be added to the end of the tokenlist to parse.
        FUNC_CALL
    }
}
