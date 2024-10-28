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
    public partial class GameSettings : Form
    {
        public GameSettings()
        {
            InitializeComponent();
            radioButton2.Checked = true;
            checkBox1.Checked = false;
            radioButton5.Checked = true;
        }
        private int playerNumber = 2;
        private int cardNumber = 4;
        private bool pcPlayer = false;
        private int difficulty = 2;
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void LoadMenu(object sender, EventArgs e)
        {
            
            StartingMenu startingMenu = new StartingMenu(playerNumber,cardNumber,pcPlayer,difficulty);
            startingMenu.Show();
            this.Visible = false;

        }
        private void Volume(object sender, EventArgs e)
        {

            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                pcPlayer = true;
            }
            else pcPlayer = false;
        }

        private void GameSettings_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && (e.KeyChar < '2' || e.KeyChar > '6'))
            {
                e.Handled = true; 
            }
            if (textBox1.Text.Length >= 1 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; 
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || !int.TryParse(textBox1.Text,out playerNumber))
            {
                
                playerNumber = 2;
                
            }
            else playerNumber = int.Parse(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
             

        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        private void radioButtonChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                cardNumber = 2;
            }
            else if (radioButton2.Checked)
            {
                cardNumber = 4;
            }
            else if (radioButton3.Checked)
            {
                cardNumber = 6;
            }
           

        }

        private void radioChanged2(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                difficulty = 1;
            }
            else if (radioButton5.Checked)
            {
                difficulty = 2;
            }
            else if (radioButton6.Checked)
            {
                difficulty = 3;
            }
        }
    }
}
