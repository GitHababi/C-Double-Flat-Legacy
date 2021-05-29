using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    
    public class Token 
    {
        public TokenType Type { get; private set; }
        public string Value { get; private set; } 

        public Token(TokenType _type) 
        { 
            Type = _type; 
            Value = default; 
        }
        
        public Token(TokenType _type, string _value)
        { 
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

        PLUS, // +
        MINUS, // -
        MUL, // *
        DIV, // /
        MOD, // %
        POW, // ^

        // Comparison Operators

        LESS, // <
        GRTR, // >
        GRTR_EQ, // >=
        LESS_EQ, // <=
        EQAL, // =
        NOT_EQAL, // !=

        // Data Types

        CHAR,
        STRING,
        INT,
        DOUBLE,
        BOOL,

        // Special Operators

        ASSGNOP, // The : Operator
        INSRTOP, // The <- Operator
        NXTLNOP, // The ; Operator

        // Flow Control

        IF,
        ELSE,
        LOOP,
        GIVEN,
        RETURN,
        
        // Special Tokens

        ERROR,
        IDENTIFIER
    }
}
