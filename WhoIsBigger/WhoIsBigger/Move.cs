using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsBigger
{
    internal class Move
    {
        public List<Throw> throws { get; set; }
        public Player player { get; set; }

        public Move(Player Player)
        {
            player = Player;
            throws = new List<Throw>();
        }
    }
}
