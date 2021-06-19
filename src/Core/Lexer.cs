using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using C_Double_Flat.Errors;
namespace C_Double_Flat.Core
{
    public class Lexer
    {
        private char[] _script; //The text file as a char array
        private Position _position; //Read head position
        private char _currentChar;

        public static List<Token> Tokenize(string text)
        {
            return new Lexer(text).GetTokens();
        }

        private Lexer(string text)
        {
            _script = text.ToCharArray();
            _position = new Position(-1, 1, 1);
            _currentChar = default;
            Advance();
        }
        public List<Token> GetTokens()
        {
            return makeTokens();
        }

        private void Advance()
        {
            _position.Advance(_currentChar);
            _currentChar = (_position._index < _script.Length) ? _script[_position._index] : default;
        }
        private void Advance(int amount)
        {
            for (int x = 0; x < amount; x++) _position.Advance(_currentChar);
            _currentChar = (_position._index < _script.Length) ? _script[_position._index] : default;
        }

        private List<Token> makeTokens()
        {
            List<Token> tokenList = new List<Token>(); // The final list of tokens to be passed into Parser / Interpreter

            while (_currentChar != default)
            {
                switch (_currentChar)
                {
                    case '#':
                        commentCompleter();
                        Advance();
                        break;
                    case '+':
                        tokenList.Add(new Token(TokenType.ADD, _position));
                        Advance();
                        break;
                    case '-':
                        tokenList.Add(new Token(TokenType.SUB, _position));
                        Advance();
                        break;
                    case '/':
                        tokenList.Add(new Token(TokenType.DIV, _position));
                        Advance();
                        break;
                    case '*':
                        tokenList.Add(new Token(TokenType.MUL, _position));
                        Advance();
                        break;
                    case '(':
                        tokenList.Add(new Token(TokenType.LPAREN, _position));
                        Advance();
                        break;
                    case ')':
                        tokenList.Add(new Token(TokenType.RPAREN, _position));
                        Advance();
                        break;
                    case ',':
                        tokenList.Add(new Token(TokenType.COMMA, _position));
                        Advance();
                        break;
                    case '{':
                        tokenList.Add(new Token(TokenType.LCURLY, _position));
                        Advance();
                        break;
                    case '<':

                        // Because the < character can be followed by multiple characters giving tokens this is what we have to do.

                        if (_position._index + 1 < _script.Length)
                        {
                            switch (_script[_position._index + 1])
                            {
                                case '=':
                                    tokenList.Add(new Token(TokenType.LESS_EQ, _position));
                                    Advance(2);
                                    break;
                                case '-':
                                    tokenList.Add(new Token(TokenType.INSRT, _position));
                                    Advance(2);
                                    break;
                                default:
                                    tokenList.Add(new Token(TokenType.LESS, _position));
                                    Advance();
                                    break;
                            }
                        }
                        else
                        {
                            tokenList.Add(new Token(TokenType.LESS, _position));
                            Advance();
                        }
                        break;
                    case '>':
                        if (_position._index + 1 < _script.Length)
                        {
                            switch (_script[_position._index + 1])
                            {
                                case '=':
                                    tokenList.Add(new Token(TokenType.GRTR_EQ, _position));
                                    Advance(2);
                                    break;
                                default:
                                    tokenList.Add(new Token(TokenType.GRTR, _position));
                                    Advance();
                                    break;
                            }
                        }
                        else
                        {
                            tokenList.Add(new Token(TokenType.GRTR, _position));
                            Advance();
                        }

                        break;
                    case '!':
                        if (_position._index + 1 < _script.Length)
                        {
                            if (_script[_position._index + 1] == '=') { tokenList.Add(new Token(TokenType.NOT_EQAL, _position)); Advance(2); }
                            else throw new TokenException(_position, _currentChar);
                        }
                        else
                        {
                            throw new TokenException(_position, _currentChar);
                        }
                        break;
                    case '}':
                        tokenList.Add(new Token(TokenType.RCURLY, _position));
                        Advance();
                        break;
                    case '=':
                        tokenList.Add(new Token(TokenType.EQAL, _position));
                        Advance();
                        break;
                    case ':':
                        tokenList.Add(new Token(TokenType.ASSGN, _position));
                        Advance();
                        break;
                    case ';':
                        tokenList.Add(new Token(TokenType.NXTLN, _position));
                        Advance();
                        break;
                    case '"':
                        tokenList.Add(stringLiteralFinder());
                        break;
                    default:
                        if (char.IsDigit(_currentChar)) tokenList.Add(makeNumberTokens());
                        else
                        {
                            if (char.IsLetter(_currentChar) || _currentChar == '_' || _currentChar == '-')
                            {
                                tokenList.Add(keywordFinder());
                            }
                            else
                            {
                                if (!char.IsWhiteSpace(_currentChar) && _currentChar != default)
                                {
                                    throw new TokenException(_position, _currentChar);
                                }
                                else
                                {
                                    Advance();
                                }
                            }
                        }
                        break;
                }
            }
            return tokenList;
        }
        private void commentCompleter()
        {
            while (_currentChar != default && _currentChar != '\n')
            {
                Advance();
            }
        }
        private Token stringLiteralFinder()
        {
            Advance();
            string accumulator = "";
            bool foundEnd = false;
            for (int x = _position._index; x < _script.Length; x++)
            {
                if (_currentChar == '"') { Advance(); foundEnd = true; break; }
                accumulator += _currentChar;
                Advance();
            }
            if (foundEnd) return new Token(TokenType.STRING, accumulator, _position);
            else throw new TerminatingStringException(_position);
        }

        private Token keywordFinder()
        {
            string fromCurrent = new String(_script, _position._index, _script.Length - _position._index);
            fromCurrent = Regex.Split(fromCurrent, @"[\s\W]")[0];
            Advance(fromCurrent.Length);
            switch (fromCurrent)
            {
                case "if":
                    return new Token(TokenType.IF, _position);
                case "else":
                    return new Token(TokenType.ELSE, _position);
                case "loop":
                    return new Token(TokenType.LOOP, _position);
                case "return":
                    return new Token(TokenType.RETURN, _position);
                case "run":
                    return new Token(TokenType.RUN, _position);
                case "true":
                    return new Token(TokenType.BOOL, "true", _position);
                case "false":
                    return new Token(TokenType.BOOL, "false", _position);
                default:
                    return new Token(TokenType.IDENTIFIER, fromCurrent, _position);
            }
        }

        private Token makeNumberTokens()
        {
            string value = "";
            int dotCount = 0;
            while (_currentChar != default && (_currentChar == '.' || char.IsDigit(_currentChar)))
            {
                if (_currentChar == '.')
                {
                    if (dotCount > 0)
                    {
                        throw new NumberDotException(_position);
                    }
                    value += '.';
                    dotCount += 1;
                    Advance();
                }
                else
                {
                    value += _currentChar;
                    Advance();
                }

            }
            return new Token(TokenType.NUMBER, value, _position);
        }
    }
}
