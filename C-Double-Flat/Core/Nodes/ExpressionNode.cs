using System;
using System.Collections.Generic;

namespace C_Double_Flat.Core
{
    public class ExpressionNode
    {
        public TokenType Type;
        public string Value;
        public ExpressionNode Left;
        public ExpressionNode Right;
        public Position Position;

        public ExpressionNode(Token data) { Type = data.Type; Value = data.Value; Position= data.Position;  }
        public ExpressionNode() { }

        public override string ToString()
        {
            string output = "e";
            string left = "";
            string right = "";
            if (Left != null) left = Left.ToString();
            if (Right != null) right = Right.ToString();
            if (left == "" && right == "") output += String.Format("[{0}: {1}]", this.Type, this.Value);
            else output += String.Format("({0}, {1}, {2})", left, this.Type, right);
            return output;
        }
        public static readonly ExpressionNode None = new (TokenHelper.None);
    }
}
