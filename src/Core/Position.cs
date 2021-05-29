using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Double_Flat.Core
{
    public struct Position
    {
        public int _index { get; private set; }
        public int _line { get; private set; }
        public int _col { get; private set; }
        public Position(int index, int line, int col)
        {
            _index = index;
            _line = line;
            _col = col;
        }

        public void Advance(char currentChar)
        {
            _index++;
            _col++;
            if (currentChar == '\n')
            {
                _line++;
                _col = 1;
            }
        }

        public Position Clone()
        {
            return this;
        }
    }
}
