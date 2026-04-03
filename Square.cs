using System;
using System.Collections.Generic;
using System.Text;

namespace Reversi
{
    internal class Square
    {
        public int row = 0;
        public int col = 0;

        public Square(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public Square() { }  // restores the no-arg constructor
    }
}
