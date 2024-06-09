using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsBigger
{
    internal class Player
    {
        public string name { get; set; }
        public int bet { get; set; }
        public int roundPoints { get; set; }
        public int generalPoints { get; set; }


        public Player(string Name, int Bet) 
        {
            name = Name;
            bet = Bet;
        }

        public void AddRoundPoints(int points)
        {
            roundPoints += points;
        }
        
        public void ClearRoundPoints()
        {
            roundPoints = 0;
        }

        public void AddGeneralPoints()
        {
            generalPoints++;
        }
    }
}
