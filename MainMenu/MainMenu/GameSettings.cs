using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MainMenu
{
    public partial class GameSettings : Form
    {
        private int playerCount = 2;
        private int cardCount = 4;
        private bool pcPlayer = false;
        private int difficulty = 2;
        private bool isSound = true;
        public GameSettings(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost, bool isSound)
        {
            InitializeComponent();

            this.playerCount = playerNumber;
            this.difficulty = obtiznost;
            this.cardCount = cardNumber;
            this.pcPlayer = pcPlayer;
            this.isSound = isSound;


            playerCountTextBox.Text = playerNumber.ToString();
            if(cardNumber==2)cardCountOne.Checked = true;
            else if (cardNumber == 4) cardCountTwo.Checked = true;
            else if (cardNumber == 6) cardCountThree.Checked = true;
            if (isSound) volumeBool.Checked = true;
            else volumeBool.Checked = false;
            if (pcPlayer) isPcPlayer.Checked = true;
            else isPcPlayer.Checked = false;
            if (difficulty == 1) difficultyOne.Checked = true;
            else if (difficulty == 2) difficultyTwo.Checked = true;
            else if (difficulty == 3) difficultyThree.Checked = true;



        }
        

        
        private void LoadMenu(object sender, EventArgs e)
        {
            int count = 0;
            if(int.TryParse(playerCountTextBox.Text, out count))
            {

                if (count >= 2 && count <= 6)
                {
                    playerCount = count;
                }
                else count = 0;
               
            }
            if (count == 0)
            {
                MessageBox.Show("Zadej platný počet hráčů!");
                
            }
            else
            {
                StartingMenu startingMenu = new StartingMenu(playerCount, cardCount, pcPlayer, difficulty, isSound);

                startingMenu.Show();
                this.Visible = false;
            }

        }
        

        

        private void PlayerCountChanged(object sender, EventArgs e)
        {
            //int count;
            //if(int.TryParse(playerCountTextBox.Text, out count))
            //{
            //    if (count >= 2 && count <= 6)
            //    {
            //        playerCount = count;
            //    }
            //    else
            //    {

            //        playerCount = 2;

            //    }
            //}
            //else if (string.IsNullOrEmpty(playerCountTextBox.Text))
            //{

            //    playerCount = 2;


            //}
            
            
            
            
        }

        private void CardCountChanged(object sender, EventArgs e)
        {
            if (cardCountOne.Checked)
            {
                cardCount = 2;
            }
            else if (cardCountTwo.Checked)
            {
                cardCount = 4;
            }
            else if (cardCountThree.Checked)
            {
                cardCount = 6;
            }
        }

        private void VolumeChecked(object sender, EventArgs e)
        {
            if (volumeBool.Checked) isSound = true;
            else isSound = false;
        }

        private void isPcPlayerChecked(object sender, EventArgs e)
        {
            if (isPcPlayer.Checked) pcPlayer = true;
            else pcPlayer = false;
        }

        private void difficultyChanged(object sender, EventArgs e)
        {
            if (difficultyOne.Checked)
            {
                difficulty = 1;
            }
            else if (difficultyTwo.Checked)
            {
                difficulty = 2;
            }
            else if (difficultyThree.Checked)
            {
                difficulty = 3;
            }
        }
    }
}
