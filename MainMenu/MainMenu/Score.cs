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
        private List<ScoreData> gameResults;

        public Score()
        {
            InitializeComponent();
            InitializeDataGridView();
            InitializeComboBox();
            LoadScoreData();
            DisplayData(gameResults);
        }

        
        private void InitializeDataGridView()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PlayerName",
                HeaderText = "Jméno hráče",
                DataPropertyName = "PlayerName",
                Width = 150
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Wins",
                HeaderText = "Výhry",
                DataPropertyName = "Wins",
                Width = 100
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Losses",
                HeaderText = "Prohry",
                DataPropertyName = "Losses",
                Width = 100
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PairsFound",
                HeaderText = "Nasbírané páry",
                DataPropertyName = "PairsFound",
                Width = 100
            });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TotalCards",
                HeaderText = "Celkový počet karet",
                DataPropertyName = "TotalCards",
                Width = 100
            });
            

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Padding = new Padding(0);
        }

        
        private void InitializeComboBox()
        {
            comboBoxFilter.Items.Clear();
            comboBoxFilter.Items.Add("Jméno hráče");
            comboBoxFilter.Items.Add("Výhry");
            comboBoxFilter.Items.Add("Prohry");
            comboBoxFilter.Items.Add("Nasbírané páry");
            comboBoxFilter.Items.Add("Celkový počet karet");
            comboBoxFilter.SelectedIndex = 0;
        }

        
        private void LoadScoreData()
        {
            try
            {
                gameResults = GameScoreSaveManager.LoadScoreData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba při načítání skóre: " + ex.Message);
                gameResults = new List<ScoreData>();
            }
        }

        
        private void DisplayData(List<ScoreData> data)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in data)
            {
                dataGridView1.Rows.Add(item.PlayerName, item.Wins, item.Losses, item.PairsFound, item.TotalCards);
            }
        }

        
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            string filterText = textBoxFilter.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(filterText))
            {
                DisplayData(gameResults);
                return;
            }

            string selectedFilter = comboBoxFilter.SelectedItem.ToString();
            List<ScoreData> filteredResults = new List<ScoreData>();

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
                        match = game.Losses.ToString().Contains(filterText);
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

        
        private void buttonClear_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Opravdu chcete vymazat veškeré skóre?", "Potvrzení", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    GameScoreSaveManager.ClearScoreData();
                    gameResults.Clear();
                    DisplayData(gameResults);
                    MessageBox.Show("Skóre bylo úspěšně vymazáno.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při mazání skóre: " + ex.Message);
                }
            }
        }

        
        private void buttonClose_Click(object sender, EventArgs e)
        {
            
            StartingMenu menu = new StartingMenu();
            menu.Show();
            this.Close();
        }

        
        private void Score_Load(object sender, EventArgs e)
        {
            LoadScoreData();
            DisplayData(gameResults);
        }
    }
}


