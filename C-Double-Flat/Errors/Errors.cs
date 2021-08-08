using C_Double_Flat.Core;
using System;

namespace C_Double_Flat.Errors
{
    public class TokenException : Exception
    {
        public TokenException(Position pos, char token)
        : base(String.Format("Unknown token: '{0}' at Line: {1} Column: {2}", token, pos._line, pos._col)) { }
        public TokenException() { }
    }
    public class NumberDotException : Exception
    {
        public NumberDotException(Position pos) :
            base(String.Format("More than one '.' found in number expression at Line: {0} Column: {1}", pos._line, pos._col))
        { }

        public NumberDotException() { }
    }
    public class IncompleteExpressionException : Exception
    {
        public IncompleteExpressionException(Position pos) :
            base(String.Format("Incomplete statement at Line: {0} Column: {1}", pos._line, pos._col))
        { }

        public IncompleteExpressionException() { }
    }
    public class TerminatingStringException : Exception
    {
        public TerminatingStringException() { }
        public TerminatingStringException(Position pos) : base(String.Format("Non-terminating string at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class TerminatingParenthesisException : Exception
    {
        public TerminatingParenthesisException() { }
        public TerminatingParenthesisException(Position pos) : base(String.Format("Non-terminating parenthesis at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class FunctionDeclarationException : Exception
    {
        public FunctionDeclarationException() { }
        public FunctionDeclarationException(Position pos) : base(String.Format("Illegal nested function declaration at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() { }
        public InvalidTokenException(Position pos) : base(String.Format("Invalid token type at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class ExpectedTokenException : Exception
    {
        public ExpectedTokenException() { }
        public ExpectedTokenException(Position pos, string type) : base(String.Format("Expected token type '{2}' at Line: {0} Column: {1}", pos._line, pos._col, type)) { }
    }
    public class EmptyConditionException : Exception
    {
        public EmptyConditionException() { }
        public EmptyConditionException(Position pos) : base(String.Format("Empty condition block at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class InvalidElseException : Exception
    {
        public InvalidElseException() { }
        public InvalidElseException(Position pos) : base(String.Format("Else cannot start a statement at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class ArgumentCountException : Exception
    {
        public ArgumentCountException(int number, string name) : base(String.Format("Function '{1}' requires at least {0} arguments.", number, name)) { }
    }
    public class InvalidExpressionException : Exception
    {
        public InvalidExpressionException() { }
        public InvalidExpressionException(Position pos) : base(String.Format("Expected expression, not condition at Line: {0} Column: {1}", pos._line, pos._col)) { }
    }
    public class InvalidAsNameException : Exception
    {
        public InvalidAsNameException() { }
        public InvalidAsNameException(Position pos) : base(String.Format("An 'asname' expression resolved to a reserved keyword at Line: {0} Column {1}",pos._line,pos._col)) { }
    }
}
