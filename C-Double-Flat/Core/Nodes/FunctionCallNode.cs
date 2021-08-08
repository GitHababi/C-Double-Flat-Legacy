using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public class FunctionCallNode : ExpressionNode
    {
        public List<ExpressionNode> Args;
        public new ExpressionNode Left;
        
        public FunctionCallNode(Token id) { this.Value = id.Value; this.Type = id.Type; }
        public FunctionCallNode(Token id, List<ExpressionNode> Args) { this.Value = id.Value; this.Type = TokenType.FUNC_CALL; this.Args = Args; }

        public override string ToString()
        {
            if (this.Type == TokenType.IDENTIFIER)
            {
                string output = "fi";
                if (Args != null)
                {
                    output += String.Format("[FUNCTION: {0}, ARGS: (", this.Value);
                    for (int x = 0; x < Args.Count - 1; x++) { output += String.Format("{0}, ", Args[x].ToString()); }
                    if (Args.Count > 0) output += String.Format("{0}", Args[Args.Count - 1].ToString());
                    output += ")]";
                }

                return output;
            }
            else
            {
                string output = "fa";
                if (Args != null)
                {
                    output += String.Format("[FUNCTION: {0}, ARGS: (", this.Left);
                    for (int x = 0; x < Args.Count - 1; x++) { output += String.Format("{0}, ", Args[x].ToString()); }
                    if (Args.Count > 0) output += String.Format("{0}", Args[Args.Count - 1].ToString());
                    output += ")]";
                }
                return output;
            }
        }
    }
}
