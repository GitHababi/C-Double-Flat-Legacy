using System;

namespace C_Double_Flat.Core
{
    public class ConditionNode : ExpressionNode
    {

        public ConditionNode(Token data) { Type = data.Type; }
        public ConditionNode(TokenType Type) { this.Type = Type; }
        public ConditionNode() { }

        public override string ToString()
        {
            string output = "c";
            string left = "";
            string right = "";
            if (Left != null) left = Left.ToString();
            if (Right != null) right = Right.ToString();
            if (left == "" && right == "") output += String.Format("[{0}]", this.Type);
            else output += String.Format("({0}, {1}, {2})", left, this.Type, right);
            return output;
        }
    }
}
