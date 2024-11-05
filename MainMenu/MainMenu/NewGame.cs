using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
namespace MainMenu
{
    public partial class NewGame : Form
    {
        Label first, second;
        private int[] score;
        private int playerNumber;
        private int playerCurrent = 1;
        private int cardNumber;
        private bool pcPlayer;
        private int difficulty;
        private bool playerRound;
        private string[] names;
        private bool isSound;
        private List<Label> flippedLabels = new List<Label>();
        public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost, bool isSound, bool isLoading = false)
        {
            InitializeComponent();
            this.playerNumber = playerNumber;
            this.pcPlayer = pcPlayer;
            this.cardNumber = cardNumber;
            this.difficulty = obtiznost;
            this.isSound = isSound;
            score = new int[playerNumber];
            playerRound = true;
            SizeOfTable();
            IconsToPlace();
            
            tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);
            tableLayoutPanel1.Padding = new Padding(0, toolStrip1.Height, 0, 0);
            playerCurrent = 1;
            if (!isLoading) names = GetNames(playerNumber, pcPlayer);
            else
            {
                names = new string[playerNumber];
                for (int i = 0; i < playerNumber; i++)
                {
                    names[i] = "Hráč " + (i + 1);
                }
            }
            UpdateScoreLabel();

        }


        Random rnd = new Random();
        List<string> icons6x6 = new List<string>()
            {
            "a","a","b","b","c","c","d","d","e","e","f","f","g","g","h","h","i","i","j","j","k","k","l","l","m","m","n","n","o","o","p","p","q","q","r","r"
            };
        List<string> icons2x2 = new List<string>()
            {
            "a","a","b","b"
            };
        List<string> icons4x4 = new List<string>()
            {
            "a","a","b","b","c","c","d","d","e","e","f","f","g","g","h","h"
            };
        
        private string[] GetNames(int playerNumber, bool pcPlayer)
        {
            if (this.names != null) return this.names;
            string[] names = new string[playerNumber];
            for (int i = 0; i < playerNumber; i++)
            {

                if (pcPlayer && i == 1)
                {
                    names[i] = "PC";
                    continue;
                }
                using (Form nameInputForm = new Form())
                {
                    nameInputForm.Text = "Zadejte jméno pro hráče " + (i + 1);
                    Label label = new Label();
                    label.Text = "Hráč " + (i + 1);
                    label.AutoSize = true;
                    label.Location = new Point(10, 10);
                    TextBox textbox = new TextBox();
                    textbox.Width = 200;
                    textbox.Location = new Point(10, 30);
                    Button btn = new Button();
                    btn.Text = "Ok";
                    btn.DialogResult = DialogResult.OK;
                    btn.Location = new Point(10, 60);

                    nameInputForm.Controls.Add(label);
                    nameInputForm.Controls.Add(textbox);
                    nameInputForm.Controls.Add(btn);


                    if (nameInputForm.ShowDialog() == DialogResult.OK)
                    {
                        names[i] = textbox.Text;
                    }

                }
            }
            this.names = names;
            return names;
        }
        private void SizeOfTable()
        {
            
            int rows = cardNumber;
            int columns = cardNumber;
            tableLayoutPanel1.RowCount = rows;
            tableLayoutPanel1.ColumnCount = columns;
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Dock = DockStyle.Fill;


            for (int i = 0; i < columns; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            }


            for (int i = 0; i < rows; i++)
            {
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            }

        }
        private void UpdateScoreLabel()
        {
            if (names[playerCurrent - 1] == string.Empty)
            {
                toolStripStatusLabel1.Text = "Hráč " + (playerCurrent - 1) + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];
            }
            toolStripStatusLabel1.Text = "Hráč " + names[playerCurrent - 1] + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];

        }
        private void DisplayScores()
        {
            string[] namesClone = names;
            Array.Sort(score, namesClone);
            Array.Reverse(namesClone);
            Array.Reverse(score);
            string output = null;
            for (int i = 0; i < playerNumber; i++)
            {
                output += "Hráč " + namesClone[i] + " , měl skoré: " + score[i] + "\n";
            }

            MessageBox.Show(output, "Konečné skóre");
            Score scoreboard = new Score();
            scoreboard.LoadScoreData();
            scoreboard.ShowDialog();
        }

        private void label_Click(object sender, EventArgs e)
        {
            if (!playerRound) return;
            if (first != null && second != null)
            {
                return;
            }
            Label clickedLabel = sender as Label;
            if (clickedLabel == null)
            {
                return;
            }
            if (clickedLabel.ForeColor == Color.White)
            {
                return;
            }
            if (first == null)
            {
                first = clickedLabel;
                first.ForeColor = Color.White;
                flippedLabels.Add(first);
                return;
            }

            second = clickedLabel;
            second.ForeColor = Color.White;
            flippedLabels.Add(second);


            if (first.Text == second.Text)
            {
                score[playerCurrent - 1]++;
                UpdateScoreLabel();
                flippedLabels.Remove(first);
                flippedLabels.Remove(second);

                first = null;
                second = null;


                WinnerCheck();
            }
            else
            {
                timer1.Start();

                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
            }


        }

        private void WinnerCheck()
        {
            Label label;


            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                label = tableLayoutPanel1.Controls[i] as Label;
                if (label != null && label.ForeColor == label.BackColor)
                {

                    return;
                }
            }

            SaveGameDetails();
            DisplayScores();


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (first != null)
            {
                first.ForeColor = first.BackColor;
            }
            if (second != null) second.ForeColor = second.BackColor;


            first = null;
            second = null;
            if (pcPlayer && playerCurrent == 2) ComputerTurn();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void IconsToPlace()
        {
            Label label;
            int rndnumber;
            List<string> icons = null;
            switch (cardNumber)
            {
                case 2:
                    {
                        icons = icons2x2;
                        break;
                    }
                case 4:
                    {
                        icons = icons4x4;
                        break;
                    }
                case 6:
                    {
                        icons = icons6x6;
                        break;
                    }
            }
            tableLayoutPanel1.Controls.Clear();
            for (int i = 0; i < cardNumber * cardNumber; i++)
            {
                label = new Label();
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.ImageAlign = ContentAlignment.MiddleCenter;
                label.BackColor = Color.LightSkyBlue;
                label.ForeColor = label.BackColor;
                label.Font = new Font("Webdings", 48);
                label.AutoSize = true;
                label.Dock = DockStyle.Fill;
                rndnumber = rnd.Next(0, icons.Count);
                label.Text = icons[rndnumber];
                icons.RemoveAt(rndnumber);

                label.Click += label_Click;
                tableLayoutPanel1.Controls.Add(label);

            }


        }

        private int GetRight()
        {

            switch (difficulty)
            {
                case 1:
                    {
                        return 30;

                    }
                case 2:
                    {
                        return 60;

                    }
                case 3:
                    {
                        return 100;

                    }
                default: return 60;
            }
        }
        [Serializable]
        public class GameState
        {
            public int PlayerNumber { get; set; }
            public int[] Scores { get; set; }
            public int PlayerCurrent { get; set; }
            public int Difficulty { get; set; }
            public bool PcPlayer { get; set; }
            public int CardNumber { get; set; }
            public List<string> CardIcons { get; set; }
            public List<bool> CardVisibility { get; set; }
            public List<string> Names { get; set; }
            public bool IsSound { get; set; }
            public GameState(List<bool> cardVisibility, int playerNumber, int[] scores, int playerCurrent, int difficulty, bool pcPlayer, int cardNumber, List<string> cardIcons, List<string> names, bool isSound)
            {
                PlayerNumber = playerNumber;
                Scores = scores;
                PlayerCurrent = playerCurrent;
                Difficulty = difficulty;
                PcPlayer = pcPlayer;
                CardIcons = cardIcons;
                CardNumber = cardNumber;
                CardVisibility = cardVisibility;
                Names = names;
                IsSound = isSound;
             }


        }
        private List<string> GetCardIcons()
        {
            List<string> icons = new List<string>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label)
                {
                    icons.Add(label.Text);
                }
            }
            return icons;
        }

        public void SaveGame(string filePath)
        {
            GameState gamestate = new GameState(
            playerNumber: this.playerNumber, // : je jako this.neco = 
            scores: this.score,
            cardNumber: this.cardNumber,
            pcPlayer: this.pcPlayer,
            difficulty: this.difficulty,
            playerCurrent: this.playerCurrent,
            cardIcons: GetCardIcons(),
            cardVisibility: GetCardVisibility(),
            names: this.names.ToList(),
            isSound : this.isSound


                );
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, gamestate);
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Herní soubory (*hra)|*.hra",
                Title = "Uložit hru"
            };
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                SaveGame(saveFileDialog.FileName);
            }
        }
        public void Load(GameState loadedGameState)
        {
            this.playerNumber = loadedGameState.PlayerNumber;
            this.cardNumber = loadedGameState.CardNumber;
            this.pcPlayer = loadedGameState.PcPlayer;
            this.difficulty = loadedGameState.Difficulty;
            this.score = loadedGameState.Scores;
            this.playerCurrent = loadedGameState.PlayerCurrent;
            this.isSound = loadedGameState.IsSound;
            if (loadedGameState.Names != null) this.names = loadedGameState.Names.ToArray();
            else
            {
                this.names = new string[playerNumber];
                for (int i = 0; i < playerNumber; i++)
                {
                    this.names[i] = "Hráč " + (i + 1);
                }
            }
            SetCardIcons(loadedGameState.CardIcons);
            UpdateScoreLabel();
            SetCardVisibility(loadedGameState.CardVisibility);


        }
        private void SetCardVisibility(List<bool> cardVisibility)
        {
            for (int i = 0; i < cardVisibility.Count; i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label label)
                {
                    label.ForeColor = cardVisibility[i] ? Color.White : label.BackColor;
                }
            }
        }
        private void SetCardIcons(List<string> cardIcons)
        {
            for (int i = 0; i < cardIcons.Count; i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label label)
                {
                    label.Text = cardIcons[i];
                    label.ForeColor = label.BackColor;
                }
            }
        }
        private List<bool> GetCardVisibility()
        {
            List<bool> visibility = new List<bool>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label)
                {
                    visibility.Add(label.ForeColor == Color.White);
                }
            }
            return visibility;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            StartingMenu startingMenu = new StartingMenu();
            startingMenu.Show();
            this.Visible = false;
        }

        private async void ComputerTurn()
        {
            playerRound = false;

            if (timer1.Enabled)
            {
                timer1.Stop();
            }

            await Task.Delay(1000);
            List<Label> hiddenLab = new List<Label>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label && label.ForeColor != Color.White)
                {
                    hiddenLab.Add(label);
                }
            }

            if (hiddenLab.Count == 0)
            {
                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
                return;
            }

            Random random = new Random();
            Label firstLabel = null;
            Label secondLabel = null;
            bool found = false;
            bool pokracovat = GetRight() >= random.Next(100);
            if (pokracovat)
            {
                
                for (int i = 0; i < flippedLabels.Count; i++)
                {
                    for (int j = i + 1; j < flippedLabels.Count; j++)
                    {
                        if (flippedLabels[i].Text == flippedLabels[j].Text && flippedLabels[i] != flippedLabels[j])
                        {
                            firstLabel = flippedLabels[i];
                            secondLabel = flippedLabels[j];
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
            }

            if (found && firstLabel != null && secondLabel != null)
            {
                
                firstLabel.ForeColor = Color.White;
                await Task.Delay(1000);
                secondLabel.ForeColor = Color.White;
                await Task.Delay(1000);
                flippedLabels.Remove(firstLabel);
                flippedLabels.Remove(secondLabel);
                score[playerCurrent - 1]++;
                UpdateScoreLabel();
                
            }
            else if(pokracovat && !found)
            {
                int firstIndex = random.Next(hiddenLab.Count);
                firstLabel = hiddenLab[firstIndex];
                hiddenLab.RemoveAt(firstIndex);
                firstLabel.ForeColor = Color.White;
                await Task.Delay(1000);
                bool found2 = false;
                foreach(Label label in flippedLabels)
                {
                    if(label.Text==firstLabel.Text&&label!=firstLabel)
                    {
                        secondLabel = label;
                        await Task.Delay(1000);
                        secondLabel.ForeColor= Color.White;
                        await Task.Delay(1000);
                        flippedLabels.Remove(secondLabel);
                        score[playerCurrent - 1]++;
                        UpdateScoreLabel();
                        
                        found2 = true;
                        break;
                    }
                }
                if(!found2)
                {
                    if (hiddenLab.Count > 0)
                    {
                        int secondIndex = random.Next(hiddenLab.Count);
                        secondLabel = hiddenLab[secondIndex];
                        await Task.Delay(1000);
                        secondLabel.ForeColor = Color.White;
                        hiddenLab.Remove(secondLabel);
                        await Task.Delay(1000);
                    }
                    else
                    {

                        playerCurrent = (playerCurrent % playerNumber) + 1;
                        UpdateScoreLabel();
                        return;
                    }

                    flippedLabels.Add(firstLabel);
                    flippedLabels.Add(secondLabel);
                }
            }
            else
            {
                
                int firstIndex = random.Next(hiddenLab.Count);
                firstLabel = hiddenLab[firstIndex];
                hiddenLab.RemoveAt(firstIndex);
                firstLabel.ForeColor = Color.White;
                await Task.Delay(1000); 

                if (hiddenLab.Count > 0) 
                {
                    int secondIndex = random.Next(hiddenLab.Count);
                    await Task.Delay(1000);
                    secondLabel = hiddenLab[secondIndex]; 
                    secondLabel.ForeColor = Color.White;
                    hiddenLab.Remove(secondLabel);
                    await Task.Delay(1000);
                }
                else
                {
                    
                    playerCurrent = (playerCurrent % playerNumber) + 1;
                    UpdateScoreLabel();
                    return;
                }

                flippedLabels.Add(firstLabel);
                flippedLabels.Add(secondLabel);
            }

            
            if (firstLabel != null && secondLabel != null && firstLabel.Text == secondLabel.Text)
            {
                
                WinnerCheck();
                ComputerTurn(); 
            }
            else
            {
                
                await Task.Delay(1000);
                firstLabel.ForeColor = firstLabel.BackColor; 
                secondLabel.ForeColor = secondLabel.BackColor; 

                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
                playerRound = true;
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

            }
        }
        private void SaveGameDetails()
        {
            string file = "gameResult.json";
            List<GameData> gameResults;

            if (File.Exists(file))
            {
                string json = File.ReadAllText(file);
                gameResults = JsonConvert.DeserializeObject<List<GameData>>(json);
            }
            else
            {
                gameResults = new List<GameData>();
            }

            int maxScore = score.Max();
            int minScore = score.Min();

            for (int i = 0; i < playerNumber; i++)
            {
                var existingPlayer = gameResults.FirstOrDefault(r => r.PlayerName == names[i]);
                if (existingPlayer != null)
                {
                    existingPlayer.PairsFound += score[i];
                    existingPlayer.TotalCards += cardNumber * cardNumber;

                    if (score[i] == maxScore)
                    {
                        existingPlayer.Wins += 1;
                    }
                    else if (score[i] == minScore)
                    {
                        existingPlayer.Loses += 1;
                    }
                }
                else
                {
                    var gameData = new GameData(
                        playerName: names[i],
                        pairsFound: score[i],
                        totalCards: cardNumber * cardNumber
                    );

                    if (score[i] == maxScore)
                    {
                        gameData.Wins = 1;
                        gameData.Loses = 0;
                    }
                    else if (score[i] == minScore)
                    {
                        gameData.Wins = 0;
                        gameData.Loses = 1;
                    }
                    else
                    {
                        gameData.Wins = 0;
                        gameData.Loses = 0;
                    }

                    gameResults.Add(gameData);
                }
            }

            string updatedJson = JsonConvert.SerializeObject(gameResults, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(file, updatedJson);
        }

    }
    }
