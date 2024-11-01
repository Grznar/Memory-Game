using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainMenu.NewGame;

namespace MainMenu
{
    public partial class Score : Form
    {
        private DataTable scoreTable;
        public Score()
        {
            InitializeComponent();
            ScoreScore();
            LoadScoreData();
        }
        public void ScoreScore()
        {
            dataGridView1.Columns.Add("PlayerName", "Jméno hráče");
            dataGridView1.Columns.Add("Wins", "Výhry");
            dataGridView1.Columns.Add("Losses", "Prohry");
            dataGridView1.Columns.Add("PairsFound", "Nasbírané páry"); 
            dataGridView1.Columns.Add("TotalCards", "Celkový počet karet"); 
        }
        public void LoadScoreData()
        {
            string file = "gameResult.json";

            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);
                List<GameData> gameResults = JsonConvert.DeserializeObject<List<GameData>>(json);

                var aggregatedResults = new Dictionary<string, GameData>();

                
                foreach (var result in gameResults)
                {
                    if (!aggregatedResults.ContainsKey(result.PlayerName))
                    {
                        
                        aggregatedResults[result.PlayerName] = new GameData(result.PlayerName, 0, 0);
                    }

                   
                    aggregatedResults[result.PlayerName].Wins += result.Wins;
                    aggregatedResults[result.PlayerName].Loses += result.Loses;
                    aggregatedResults[result.PlayerName].PairsFound += result.PairsFound;
                    aggregatedResults[result.PlayerName].TotalCards += result.TotalCards;
                }

                
                dataGridView1.Rows.Clear();

                
                foreach (var playerData in aggregatedResults.Values)
                {
                    dataGridView1.Rows.Add(playerData.PlayerName, playerData.Wins, playerData.Loses, playerData.PairsFound, playerData.TotalCards);
                }
            }
        }
        [Serializable]
        public class GameData
        {
            public string PlayerName { get; set; }
            public int Wins { get; set; }
            public int Loses { get; set; }
            public int PairsFound { get; set; }
            public int TotalCards { get; set; }

            public GameData(string playerName, int pairsFound, int totalCards)
            {
                PlayerName = playerName;
                PairsFound = pairsFound;
                TotalCards = totalCards;
                Wins = 0;
                Loses = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            string file = "gameResult.json";
            if (File.Exists(file))
            {
                File.Delete(file); 
            }
        }
    }
}

