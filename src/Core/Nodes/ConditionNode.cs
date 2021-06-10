﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public class ConditionNode
    {
        public TokenType Type;
        public ExpressionNode Left;
        public ExpressionNode Right;
        public ConditionNode(Token data) { Type = data.Type; }
        public ConditionNode(TokenType Type) { this.Type = Type; }
        public ConditionNode() {}
        
        public override string ToString()
        {
            string output = "";
            string left = "";
            string right = "";
            if (Left != null) left = Left.ToString();
            if (Right != null) right = Right.ToString();
            if (left == "" && right == "") output = String.Format("[{0}]", this.Type);
            else output = String.Format("({0}, {1}, {2})", left, this.Type, right);
            return output;
        }
    }
}