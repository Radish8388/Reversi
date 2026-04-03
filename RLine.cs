using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class RLine
    {
        public Square end = new Square();
        public List<Square> flipped = new List<Square>();
    }
}
