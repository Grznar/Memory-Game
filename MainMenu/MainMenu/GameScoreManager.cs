using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainMenu
{
    public class GameScoreManager
    {
        private List<ScoreData> scoreList;
        private string[] playerNames;
        private int[] scores;
        private int[] wins;
        private int[] losses;

        public GameScoreManager(string[] names)
        {
            
            this.playerNames = names;
            this.scores = new int[names.Length];
            this.wins = new int[names.Length];
            this.losses = new int[names.Length];
            scoreList = new List<ScoreData>();
            foreach (var name in names)
            {
                
                if (!scoreList.Any(s => s.PlayerName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                {
                    scoreList.Add(new ScoreData(name, 0, 0, 0, 0));
                }
            }
        }
        public void AddOrUpdateScore(string playerName, int wins, int losses, int pairsFound, int totalCards)
        {
            var playerScore = scoreList.FirstOrDefault(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase));
            if (playerScore != null)
            {
                
                playerScore.Wins += wins;
                playerScore.Losses += losses;
                playerScore.PairsFound += pairsFound;
                playerScore.TotalCards += totalCards;
            }
            else
            {
                
                scoreList.Add(new ScoreData(playerName, wins, losses, pairsFound, totalCards));
            }
            
            GameScoreSaveManager.SaveScoreData(scoreList);
        }

        
        public List<ScoreData> GetAllScoresToList()
        {
            return scoreList;
        }

        
        public void ClearAllScores()
        {
            scoreList.Clear();
            GameScoreSaveManager.ClearScoreData();
        }
        public void AddScore(int playerIndex, int points)
        {
            if (playerIndex < 0 || playerIndex >= scores.Length) return;
            scores[playerIndex] += points;
        }
        public void AddWin(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= wins.Length) return;
            wins[playerIndex]++;
        }
        public void AddLoss(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= losses.Length) return;
            losses[playerIndex]++;
        }
        public int GetWins(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= wins.Length)
                return 0;
            return wins[playerIndex];
        }
        public int GetLosses(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= losses.Length)
                return 0;
            return losses[playerIndex];
        }
        public string GetPlayerName(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= playerNames.Length)
                return "Neznámý hráč";
            return playerNames[playerIndex];
        }
        public int GetScore(int playerIndex)
        {
            if (playerIndex < 0 || playerIndex >= scores.Length)
                return 0;
            return scores[playerIndex];
        }
        public void SetPlayerName(int playerIndex, string newName)
        {
            if (playerIndex < 0 || playerIndex >= playerNames.Length) return;
            playerNames[playerIndex] = newName;
        }
        public string[] GetAllNames()
        {
            return (string[])playerNames.Clone();
        }
        public int[] GetAllScores()
        {
            return (int[])scores.Clone();
        }
        public int[] GetAllWins()
        {
            return (int[])wins.Clone();
        }

        public int[] GetAllLosses()
        {
            return (int[])losses.Clone();
        }
        public void SetWins(int[] newWins)
        {
            if (newWins == null) return;
            for (int i = 0; i < wins.Length && i < newWins.Length; i++)
            {
                wins[i] = newWins[i];
            }
        }
        public void SetLosses(int[] newLosses)
        {
            if (newLosses == null) return;
            for (int i = 0; i < losses.Length && i < newLosses.Length; i++)
            {
                losses[i] = newLosses[i];
            }
        }
        public void SetScores(int[] newScores)
        {
            if (newScores == null) return;
            for (int i = 0; i < scores.Length && i < newScores.Length; i++)
            {
                scores[i] = newScores[i];
            }
        }
        public void GetSortedScores(out string[] sortedNames, out int[] sortedScores)
        {
            
            sortedNames = (string[])playerNames.Clone();
            sortedScores = (int[])scores.Clone();

            
            
            Array.Sort(sortedScores, sortedNames);

            
            Array.Reverse(sortedScores);
            Array.Reverse(sortedNames);
        }
        public void EndScore()
        {
            
            GetSortedScores(out string[] sortedNames, out int[] sortedScores);

            string output = "";
            for (int i = 0; i < sortedNames.Length; i++)
            {
                output += "Hráč "+sortedNames[i]+" má skóre: "+sortedScores[i]+"\n";
            }

            
            MessageBox.Show(output, "Finální skóre");
        }


    }

}
