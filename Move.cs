using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class Move
    {
        public Square square = new Square();
        public List<RLine> lines = new List<RLine>();
        public int score = 0;
    }
}
