using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsBigger
{
    internal class Game
    {
        private static Game instance;
        public static Game Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        public List<Player> players;
        public List<Round> rounds;
        public int bank;
        public Round firstRound;
        public Player firstPlayer;
        public Dictionary<Player, int> resultsPerRound;

        public Game()
        {
            players = new List<Player>();
            rounds = new List<Round>();
            resultsPerRound = new Dictionary<Player, int>();
        }

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }
        public void AddRound(Round round)
        {
            rounds.Add(round);
        }
        public void AddBank()
        {
            foreach(Player player in players)
            {
                bank += player.bet;
            }
        }
        public void UpdateResultsPerRound()
        {
            foreach (Player player in players)
            {
                if (resultsPerRound.ContainsKey(player))
                {
                    resultsPerRound[player] += player.roundPoints;
                }
                else
                {
                    resultsPerRound.Add(player, player.roundPoints);
                }
            }
        }
        public void ClearResultsPerRound()
        {
            resultsPerRound.Clear();
        }






        public void FirstRoundIdentify()
        {
            firstRound = rounds[0];
        }
        public void FirstPlayerIdentify()
        {
            firstPlayer = players[0];
        }
        public void RoundChange()
        {
            Round round = rounds[0];
            rounds.RemoveAt(0);
            rounds.Add(round);
        }
        public void PlayerChange()
        {
            Player player = players[0];
            players.RemoveAt(0);
            players.Add(player);

        }
        public List<Player> ExtraPlayerChange(List<Player> winnerForExtra)
        {
            Player player = winnerForExtra[0];
            winnerForExtra.RemoveAt(0);
            winnerForExtra.Add(player);
            return winnerForExtra;
        }
        public List<Player> DetermineWinnerForRound()
        {
            int max = 0;
            List<Player> winner = new List<Player>();

            foreach (Player player in players)
            {
                if (player.roundPoints > max)
                {
                    max = player.roundPoints;
                }
            }

            foreach (Player player in players)
            {
                if (player.roundPoints == max)
                {
                    winner.Add(player);
                }
            }

            foreach (Player player in players)
            {
                player.ClearRoundPoints();
            }

            return winner;
        }
        public List<Player> DetermineWinnerForGame()
        {
            int max = 0;
            List<Player> winner = new List<Player>();

            foreach (Player player in players)
            {
                if (player.generalPoints > max)
                {
                    max = player.generalPoints;
                }
            }

            foreach (Player player in players)
            {
                if (player.generalPoints == max)
                {
                    winner.Add(player);
                }
            }

            return winner;
        }
    }
}
