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
        public partial class StartingMenu : Form
        {
            public StartingMenu()
            {
                InitializeComponent();
            }
            private int difficulty = 2;
            private int playerCount = 2;
            private int cardCount = 4;
            private bool pcPlayer = false;
            private bool isSound = true;
            private void QuitApp(object sender, EventArgs e)
            {
                Application.Exit();
            }
            public StartingMenu(int playerCount,int cardCount,bool pcPlayer,int difficulty,bool isSound)
            {
                InitializeComponent();
            
                {
                    this.playerCount = playerCount;
                    this.difficulty = difficulty;
                    this.cardCount = cardCount;
                    this.pcPlayer = pcPlayer;
                    this.isSound = isSound;
                
                
                
                }
            
            }
            private void LoadGame(object sender, EventArgs e)
            {
                NewGame gameWindow = new NewGame(playerCount,cardCount,pcPlayer,difficulty,isSound,false);
                gameWindow.Show();
                this.Visible = false;
            }

        
            private void LoadSettings(object sender, EventArgs e)
            {
            GameSettings gameSettings = null;
            
                 gameSettings = new GameSettings(playerCount, cardCount, pcPlayer, difficulty, isSound);
            
            
                gameSettings.Show();
                this.Visible = false;
            
            
            }

            private void LoadScore(object sender, EventArgs e)
            {
            
                Score score = new Score();
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
                        MessageBox.Show($"Chyba při načítání hry: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


    }


}
 
    
