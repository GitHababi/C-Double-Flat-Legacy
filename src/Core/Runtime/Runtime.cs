using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core.Runtime
{
    public class Environment
    {
        Dictionary<string, Value> VariableList;
        
        private int index = 0;
        private List<Statement> Statements;
        private Statement current { get { return Statements[index]; } }


        public static Value Run(List<Statement> statements)
        {
            return new Environment(statements).InternalRun();
        } 

        private Environment(List<Statement> statements) { this.Statements = statements; }
    
        private Value InternalRun()
        {
            while (index < Statements.Count)
            {
                if (current.Equals(typeof(ASSIGN))) DoVarAssignment((ASSIGN)current);
                else if (current.Equals(typeof(RETURN))) { }
            }

            return Value.Default;
        }

        private void DoVarAssignment(ASSIGN input)
        {
            VariableList.Add(input.Identifier.Value, null);
        }
    }
}
