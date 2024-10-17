using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
        public NewGame(int playerNumber)
        {
            InitializeComponent();
            this.playerNumber = playerNumber;
            score=new int[playerNumber];
            iconsToPlace();
            updateScoreLabel();
            tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);

        }
        Random rnd = new Random();
        List<string> icons = new List<string>()
        {
        "a","a","b","b","c","c","d","d","e","e","f","f","g","g","h","h"
        };


        private void updateScoreLabel()
        {
            toolStripStatusLabel1.Text = "Hráč " + playerCurrent + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];
            
        }
        private void DisplayScores()
        {
            int[] playerIndices = new int[playerNumber];
            for (int i = 0; i < playerNumber; i++)
            {
                playerIndices[i] = i;
            }

            
            Array.Sort(score, playerIndices);
            Array.Reverse(score);  

            
            string scoreText = "Pořadí hráčů:\n";
            for (int i = 0; i < playerNumber; i++)
            {
                scoreText += "Hráč " + (playerIndices[i] + 1) + ": " + score[i] + " bodů\n";
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
            second=null;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void iconsToPlace()
        {
            Label label;
            int rndnumber;

            for (int i = 0; i < tableLayoutPanel1.Controls.Count;i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label)
                {
                    label = (Label)tableLayoutPanel1.Controls[i];
                }
                else continue;

                rndnumber = rnd.Next(0, icons.Count);
                label.Text = icons[rndnumber];
                icons.RemoveAt(rndnumber);
                label.ForeColor = label.BackColor;
            }
            

        }
    }
}
