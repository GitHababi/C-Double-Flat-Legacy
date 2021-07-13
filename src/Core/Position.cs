namespace C_Double_Flat.Core
{
    public class PositionHelper
    {
        public static readonly Position None = new Position(0, 0, 0);
    }
    public struct Position
    {
        public int _index { get; private set; }
        public int _line { get; private set; }
        public int _col { get; private set; }
        public Position(int index, int line, int col)
        {
            _index = index;
            _line = line;
            _col = col - 1;
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
