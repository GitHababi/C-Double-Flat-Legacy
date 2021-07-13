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

        public ExpressionNode(Token data) { Type = data.Type; Value = data.Value; }
        public ExpressionNode() { }

        public override string ToString()
        {
            string output = "";
            string left = "";
            string right = "";
            if (Left != null) left = Left.ToString();
            if (Right != null) right = Right.ToString();
            if (left == "" && right == "") output = String.Format("[{0}: {1}]", this.Type, this.Value);
            else output = String.Format("({0}, {1}, {2})", left, this.Type, right);
            return output;
        }
        public static readonly ExpressionNode None = new ExpressionNode(TokenHelper.None);
    }


    public class FuncCallNode : ExpressionNode
    {
        public List<ExpressionNode> Args;
        public FuncCallNode(Token id) { this.Value = id.Value; this.Type = TokenType.FUNC_CALL; }
        public FuncCallNode(Token id, List<ExpressionNode> Args) { this.Value = id.Value; this.Type = TokenType.FUNC_CALL; this.Args = Args; }

        public override string ToString()
        {
            string output = "";
            if (Args != null)
            {
                output = String.Format("[FUNCTION: {0}, ARGS: (", this.Value);
                for (int x = 0; x < Args.Count - 1; x++) { output += String.Format("{0}, ", Args[x].ToString()); }
                if (Args.Count > 0) output += String.Format("{0}", Args[Args.Count - 1].ToString());
                output += ")]";
            }

            return output;
        }
    }
}
