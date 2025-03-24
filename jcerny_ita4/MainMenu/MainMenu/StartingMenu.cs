using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MainMenu
{
    public partial class StartingMenu : Form
    {
        private int difficulty = 2;
        private int playerCount = 2;
        private int cardCount = 4;
        private bool pcPlayer = false;
        private bool isSound = true;

        public StartingMenu()
        {
            InitializeComponent();
        }

        public StartingMenu(int playerCount, int cardCount, bool pcPlayer, int difficulty, bool isSound)
        {
            InitializeComponent();
            this.playerCount = playerCount;
            this.difficulty = difficulty;
            this.cardCount = cardCount;
            this.pcPlayer = pcPlayer;
            this.isSound = isSound;
        }

        private void QuitApp(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void LoadGame(object sender, EventArgs e)
        {
            NewGame gameWindow = new NewGame(playerCount, cardCount, pcPlayer, difficulty, isSound, false);
            gameWindow.Show();
            this.Visible = false;
        }

        private void LoadSettings(object sender, EventArgs e)
        {
            GameSettings gameSettings = new GameSettings(playerCount, cardCount, pcPlayer, difficulty, isSound);
            gameSettings.Show();
            this.Visible = false;
        }

        private void LoadScore(object sender, EventArgs e)
        {
            Score score = new Score(playerCount, cardCount, pcPlayer, difficulty, isSound);
            score.Show();
            this.Visible = false;
        }

        private void LoadGameDetails(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Pexeso Saved Game|*.save";
                ofd.Title = "Načíst hru";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        GameSave loadedGame = GameSaveManager.LoadGame(ofd.FileName);
                        NewGame gameWindow = new NewGame(
                            loadedGame.PlayerNumber,
                            loadedGame.CardNumber,
                            loadedGame.PCPlayer,
                            loadedGame.Difficulty,
                            loadedGame.IsSound,
                            isLoading: true
                        );

                        gameWindow.RestoreFromGameSave(loadedGame);
                        gameWindow.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při načítání hry " + ex.Message);
                    }
                }
            }
        }
    }
}
