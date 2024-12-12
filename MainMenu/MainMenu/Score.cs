using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainMenu.NewGame;

namespace MainMenu
{
    public partial class Score : Form
    {
        private DataTable scoreTable;
        private List<GameData> gameResults;
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

        private void DisplayData(List<GameData> data)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.PlayerName, item.Wins, item.Loses, item.PairsFound, item.TotalCards);
            }
        }
        private void InitializeComboBox()
        {
            comboBox1.Items.Add("Jméno hráče");
            comboBox1.Items.Add("Výhry");
            comboBox1.Items.Add("Prohry");
            comboBox1.Items.Add("Nasbírané páry");
            comboBox1.Items.Add("Celkový počet karet");
            comboBox1.SelectedIndex = 0; 
        }
        public void LoadScoreData()
        {
            string file = "gameResult.dat"; 

            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    gameResults = (List<GameData>)formatter.Deserialize(fs);


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
            else
            {
                
                MessageBox.Show("Game result file not found.");
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

        private void buttonFilter(object sender, EventArgs e)
        {
            string filterText = textBox1.Text.ToLower();
            List<GameData> filteredResults = new List<GameData>();

            string selectedFilter = comboBox1.SelectedItem.ToString();

            foreach (var game in gameResults)
            {
                bool match = false;

                switch (selectedFilter)
                {
                    case "Jméno hráče":
                        match = game.PlayerName.ToLower().Contains(filterText);
                        break;
                    case "Výhry":
                        match = game.Wins.ToString().Contains(filterText);
                        break;
                    case "Prohry":
                        match = game.Loses.ToString().Contains(filterText);
                        break;
                    case "Nasbírané páry":
                        match = game.PairsFound.ToString().Contains(filterText);
                        break;
                    case "Celkový počet karet":
                        match = game.TotalCards.ToString().Contains(filterText);
                        break;
                }

                if (match)
                {
                    filteredResults.Add(game);
                }
            }

            DisplayData(filteredResults);
        
    }

        private void Score_Load(object sender, EventArgs e)
        {

        }
    }
}

