﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost)
        {
            InitializeComponent();
            this.playerNumber = playerNumber;
            this.pcPlayer = pcPlayer;
            this.cardNumber = cardNumber;
            this.difficulty = obtiznost;
            score = new int[playerNumber];
            playerRound = true;
            SizeOfTable();
            IconsToPlace();
            
            tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);
            tableLayoutPanel1.Padding = new Padding(0, toolStrip1.Height, 0,0);
            playerCurrent = 1;
            names = GetNames(playerNumber, pcPlayer);
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
        private string[] GetNames(int playerNumber,bool pcPlayer)
        {
            string[] names = new string[playerNumber];
            for(int i=0;i<playerNumber;i++)
            {
                if(!string.IsNullOrEmpty(names[i]))
                {
                    names[i] = this.names[i];
                    continue;
                }
                if (pcPlayer && i == 1)
                {
                    names[i] = "PC";
                    continue;
                }
            using(Form nameInputForm = new Form())
            {
                    nameInputForm.Text = "Zadejte jméno pro hráče " + (i + 1);
                    Label label = new Label();
                    label.Text = "Hráč " + (i + 1);
                    label.AutoSize = true;
                    label.Location =new Point (10, 10);
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

                    if(nameInputForm.ShowDialog() == DialogResult.OK)
                    {
                        names[i] = textbox.Text;
                    }

            }
            }
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
            if (names[playerCurrent-1]==string.Empty)
            {
                toolStripStatusLabel1.Text = "Hráč " + (playerCurrent-1) + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];
            }
            toolStripStatusLabel1.Text = "Hráč " + names[playerCurrent-1] + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];

        }
        private void DisplayScores()
        {
            int[] playerNumeros = new int[playerNumber];
            for (int i = 0; i < playerNumber; i++)
            {
                playerNumeros[i] = i;
            }


            Array.Sort(score, playerNumeros);
            Array.Reverse(score);


            string scoreText = "Pořadí hráčů:\n";

            for (int i = 0; i < playerNumber; i++)
            {
                scoreText += "Hráč " + (playerNumeros[i] + 1) + ": " + score[i] + " bodů\n";
            }

            MessageBox.Show(scoreText, "Konečné skóre");
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
                return;
            }

            second = clickedLabel;
            second.ForeColor = Color.White;


            if (first.Text == second.Text)
            {
                score[playerCurrent - 1]++;
                UpdateScoreLabel();


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
                        return 10;

                    }
                case 2:
                    {
                        return 35;

                    }
                case 3:
                    {
                        return 75;

                    }
                default: return 10;
            }
        }
        [Serializable]public class GameState
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

            public GameState(List<bool>cardVisibility,int playerNumber, int[] scores,int playerCurrent,int difficulty,bool pcPlayer,int cardNumber,List<string>cardIcons,List<string> names)
            {
                PlayerNumber = playerNumber;
                Scores=scores;
                PlayerCurrent=playerCurrent;
                Difficulty = difficulty;
                PcPlayer = pcPlayer;
                CardIcons = cardIcons;
                CardNumber = cardNumber;
                CardVisibility = cardVisibility;
                Names = Names;

            }
           

        }
        private List<string> GetCardIcons()
        {
            List<string> icons = new List<string>();
            foreach(Control control in tableLayoutPanel1.Controls)
            {
                if(control is Label label)
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
            names: this.names.ToList()


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
            this.names = loadedGameState.Names.ToArray();
            
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
        int firstIndex = random.Next(hiddenLab.Count);
        Label firstLabel = hiddenLab[firstIndex];


        firstLabel.ForeColor = Color.White;




        await Task.Delay(1000);
        bool isMatch = false;


        if (random.Next(100) < GetRight())
        {

            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label && label.ForeColor != Color.White && label.Text == firstLabel.Text)
                {

                    isMatch = true;
                    break;
                }
            }
        }


        Label secondLabel;
        if (isMatch)
        {
            secondLabel = hiddenLab.First(label => label.Text == firstLabel.Text && label.ForeColor != Color.White);
        }
        else
        {
            hiddenLab.Remove(firstLabel);
            secondLabel = hiddenLab[random.Next(hiddenLab.Count)];
        }

        await Task.Delay(1000);
        secondLabel.ForeColor = Color.White;


        if (firstLabel.Text == secondLabel.Text)
        {
            score[playerCurrent - 1]++;
            UpdateScoreLabel();
            WinnerCheck();
            ComputerTurn();
        }
        else
        {

            await Task.Delay(1000);
            {
                if (firstLabel != null)
                {
                    firstLabel.ForeColor = firstLabel.BackColor;
                }

                if (secondLabel != null)
                {
                    secondLabel.ForeColor = secondLabel.BackColor;
                }


                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
            };
        }
        playerRound = true;
    }
}
        }
    












    


