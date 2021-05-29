using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace C_Double_Flat.Core
{
    public class Lexer
    {
        private char[] _script; //The text file as a char array
        private Position _position; //Read head position
        private char _currentChar;

        public Lexer(string text, out List<Token> output)
        {
            _script = text.ToCharArray();
            _position = new Position(-1, 1, 1);
            _currentChar = default;
            Advance();
            output = makeTokens();
        }
        public Lexer(string text)
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
                    case '+':
                        tokenList.Add(new Token(TokenType.PLUS));
                        Advance();
                        break;
                    case '-':
                        tokenList.Add(new Token(TokenType.MINUS));
                        Advance();
                        break;
                    case '/':
                        tokenList.Add(new Token(TokenType.DIV));
                        Advance();
                        break;
                    case '*':
                        tokenList.Add(new Token(TokenType.MUL));
                        Advance();
                        break;
                    case '%':
                        tokenList.Add(new Token(TokenType.MOD));
                        Advance();
                        break;
                    case '^':
                        tokenList.Add(new Token(TokenType.POW));
                        Advance();
                        break;
                    case '(':
                        tokenList.Add(new Token(TokenType.LPAREN));
                        Advance();
                        break;
                    case ')':
                        tokenList.Add(new Token(TokenType.RPAREN));
                        Advance();
                        break;
                    case ',':
                        tokenList.Add(new Token(TokenType.COMMA));
                        Advance();
                        break;
                    case '{':
                        tokenList.Add(new Token(TokenType.LCURLY));
                        Advance();
                        break;
                    case '<':

                        // Because the < character can be followed by multiple characters giving tokens this is what we have to do.

                        if (_position._index + 1 != _script.Length)
                        {
                            switch (_script[_position._index + 1])
                            {
                                case '=':
                                    tokenList.Add(new Token(TokenType.LESS_EQ));
                                    Advance(2);
                                    break;
                                case '-':
                                    tokenList.Add(new Token(TokenType.INSRTOP));
                                    Advance(2);
                                    break;
                                default:
                                    tokenList.Add(new Token(TokenType.LESS));
                                    Advance();
                                    break;
                            }
                        }
                        else
                        {
                            tokenList.Add(new Token(TokenType.LESS));
                            Advance();
                        }
                        break;
                    case '>':
                        if (_position._index - +1 != _script.Length)
                        {
                            switch (_script[_position._index + 1])
                            {
                                case '=':
                                    tokenList.Add(new Token(TokenType.GRTR_EQ));
                                    Advance(2);
                                    break;
                                default:
                                    tokenList.Add(new Token(TokenType.GRTR));
                                    Advance();
                                    break;
                            }
                        }
                        else
                        {
                            tokenList.Add(new Token(TokenType.GRTR));
                            Advance();
                        }

                        break;
                    case '}':
                        tokenList.Add(new Token(TokenType.RCURLY));
                        Advance();
                        break;
                    case '=':
                        tokenList.Add(new Token(TokenType.EQAL));
                        Advance();
                        break;
                    case ':':
                        tokenList.Add(new Token(TokenType.ASSGNOP));
                        Advance();
                        break;
                    case ';':
                        tokenList.Add(new Token(TokenType.NXTLNOP));
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
                                    tokenList.Add(new Token(TokenType.ERROR, "Unknown Token: '" + _currentChar + "' Col: " + _position._col + " Line: " + _position._line));
                                    Advance();
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
            if (foundEnd) return new Token(TokenType.STRING, accumulator);
            else return new Token(TokenType.ERROR, "String Literal was not terminated." + " Col: " + _position._col + " Line: " + _position._line);
        }

        private Token keywordFinder()
        {
            string fromCurrent = new String(_script, _position._index, _script.Length - _position._index);
            fromCurrent = Regex.Split(fromCurrent, @"[\s\W]")[0];
            Advance(fromCurrent.Length);
            switch (fromCurrent)
            {
                case "if":
                    return new Token(TokenType.IF);
                case "else":
                    return new Token(TokenType.ELSE);
                case "loop":
                    return new Token(TokenType.LOOP);
                case "given":
                    return new Token(TokenType.GIVEN);
                case "return":
                    return new Token(TokenType.RETURN);
                case "true":
                    return new Token(TokenType.BOOL, "true");
                case "false":
                    return new Token(TokenType.BOOL, "false");
                default:
                    return new Token(TokenType.IDENTIFIER, fromCurrent);
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
                        return new Token(TokenType.ERROR, "More than one '.' found in decimal number." + " Col: " + _position._col + " Line: " + _position._line);
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
            return new Token((dotCount == 1) ? TokenType.DOUBLE : TokenType.INT, value);
        }
    }
}
