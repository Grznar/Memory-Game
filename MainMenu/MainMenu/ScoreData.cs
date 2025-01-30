using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainMenu
{
    [Serializable]
    public class ScoreData
    {
        public string PlayerName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int PairsFound { get; set; }
        public int TotalCards { get; set; }

        public ScoreData(string playerName, int wins, int losses, int pairsFound, int totalCards)
        {
            PlayerName = playerName;
            Wins = wins;
            Losses = losses;
            PairsFound = pairsFound;
            TotalCards = totalCards;
        }
    }
}
