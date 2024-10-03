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
    public partial class StartingMenu : Form
    {
        public StartingMenu()
        {
            InitializeComponent();
        }

        private void QuitApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            NewGame gameWindow = new NewGame();
            gameWindow.Show();
            this.Visible = false;
        }

        private void LoadSettings(object sender, EventArgs e)
        {
            GameSettings gameSettings = new GameSettings();
            gameSettings.Show();
            this.Visible = false;
        }

        private void LoadScore(object sender, EventArgs e)
        {
            Score score = new Score();
            score.Show();
            this.Visible = false;
        }
    }
}
