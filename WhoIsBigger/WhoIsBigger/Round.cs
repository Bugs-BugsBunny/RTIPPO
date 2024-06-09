using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsBigger
{
    internal class Round
    {
        public int number;
        public List<Move> moves;

        public Round(int Number)
        {
            number = Number;
            moves = new List<Move>();
        }

        public void ClearMoves()
        {
            moves.Clear();
        }
    }
}
