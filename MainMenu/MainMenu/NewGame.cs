using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost)
        {
            InitializeComponent();
            this.playerNumber = playerNumber;
            this.pcPlayer = pcPlayer;
            this.cardNumber = cardNumber;
            this.difficulty = obtiznost;
            score = new int[playerNumber];
            sizeOfTable();
            iconsToPlace();
            updateScoreLabel();
            tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);
            playerCurrent = 1;


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

        private void sizeOfTable()
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
        private void updateScoreLabel()
        {
            toolStripStatusLabel1.Text = "Hráč " + playerCurrent + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];

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
                updateScoreLabel();


                first = null;
                second = null;


                WinnerCheck();
            }
            else
            {
                timer1.Start();

                playerCurrent = (playerCurrent % playerNumber) + 1;
                updateScoreLabel();
            }
            if (pcPlayer && playerCurrent == 2)
            {

                computerTurn();
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
            first.ForeColor = first.BackColor;
            second.ForeColor = second.BackColor;

            first = null;
            second = null;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconsToPlace()
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
        private async void computerTurn()
        {
            
            
        }









    }



}
    


