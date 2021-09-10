using C_Double_Flat.Errors;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace C_Double_Flat.Core
{
    public class Lexer
    {
        private readonly char[] _script; //The text file as a char array
        private Position _position; //Read head position
        private char _currentChar;

        /// <summary>
        /// Tokenize a string input into tokens.
        /// </summary>
        /// <param name="text">The input</param>
        /// <returns>tokens</returns>
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
            return MakeTokens();
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

        private List<Token> MakeTokens()
        {
            List<Token> tokenList = new(); // The final list of tokens to be passed into Parser / Interpreter

            while (_currentChar != default)
            {
                switch (_currentChar)
                {
                    case '#':
                        CommentCompleter();
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
                        tokenList.Add(StringLiteralFinder());
                        break;
                    default:
                        if (char.IsDigit(_currentChar)) tokenList.Add(MakeNumberTokens());
                        else
                        {
                            if (char.IsLetter(_currentChar) || _currentChar == '_' || _currentChar == '-')
                            {
                                tokenList.Add(KeywordFinder());
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
        private void CommentCompleter()
        {
            while (_currentChar != default && _currentChar != '\n')
            {
                Advance();
            }
        }
        private Token StringLiteralFinder()
        {
            Advance();
            string accumulator = "";
            bool foundEnd = false;
            for (int x = _position._index; x < _script.Length; x++)
            {
                if (_currentChar == '^')
                {
                    Advance();
                    accumulator += _currentChar switch
                    {
                        'n' => '\n',
                        't' => '\t',
                        '"' => '"',
                        '^' => '^',
                        _ => (string)"^" + _currentChar.ToString(),
                    };
                    Advance();
                }
                else
                {
                    if (_currentChar == '"') { Advance(); foundEnd = true; break; }
                    accumulator += _currentChar;
                    Advance();
                }
            }
            if (foundEnd) return new Token(TokenType.STRING, accumulator, _position);
            else throw new TerminatingStringException(_position);
        }

        private Token KeywordFinder()
        {
            string fromCurrent = new(_script, _position._index, _script.Length - _position._index);
            fromCurrent = Regex.Split(fromCurrent, @"[\s\W]")[0];
            Advance(fromCurrent.Length);
            return fromCurrent switch
            {
                "if" => new Token(TokenType.IF, _position),
                "else" => new Token(TokenType.ELSE, _position),
                "loop" => new Token(TokenType.LOOP, _position),
                "return" => new Token(TokenType.RETURN, _position),
                "run" => new Token(TokenType.RUN, _position),
                "true" => new Token(TokenType.BOOL, "true", _position),
                "false" => new Token(TokenType.BOOL, "false", _position),
                "repeat" => new Token(TokenType.REPEAT, _position),
                "nameof" => new Token(TokenType.NAMEOF, _position),
                "asname" => new Token(TokenType.ASNAME, _position),
                _ => new Token(TokenType.IDENTIFIER, fromCurrent, _position),
            };
        }

        private Token MakeNumberTokens()
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
