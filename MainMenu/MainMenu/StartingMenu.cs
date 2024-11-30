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
            private int playerNumber = 2;
            private int cardNumber = 4;
            private bool pcPlayer = false;
            private bool isSound = true;
            private void QuitApp(object sender, EventArgs e)
            {
                Application.Exit();
            }
            public StartingMenu(int playerNumber,int cardNumber,bool pcPlayer,int difficulty,bool isSound)
            {
                InitializeComponent();
            
                {
                    this.playerNumber = playerNumber;
                    this.difficulty = difficulty;
                    this.cardNumber = cardNumber;
                    this.pcPlayer = pcPlayer;
                    this.isSound = isSound;
                
                
                
                }
            
            }
            private void LoadGame(object sender, EventArgs e)
            {
                NewGame gameWindow = new NewGame(playerNumber,cardNumber,pcPlayer,difficulty,isSound,false);
                gameWindow.Show();
                this.Visible = false;
            }

        
            private void LoadSettings(object sender, EventArgs e)
            {
                GameSettings gameSettings = new GameSettings(playerNumber, cardNumber, pcPlayer, difficulty, isSound);
                gameSettings.Show();
                this.Visible = false;
            }

            private void LoadScore(object sender, EventArgs e)
            {
            
                Score score = new Score();
                score.Show();
                this.Visible = false;
            }

            private void LoadGame()
            {
                
                
            }
            private void button4_Click(object sender, EventArgs e)
            {
            LoadGameDetails();
            }
        private void LoadGameDetails()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pexeso Saved Game Files|*.save";
                openFileDialog.Title = "Načíst hru";

                // Pokud uživatel vybere soubor
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Načteme a deserializujeme uložený soubor
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            GameSave gameSave = (GameSave)formatter.Deserialize(fs);

                            // Předáme uložený stav do formuláře pro novou hru
                            StartGameFromSavedState(gameSave);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při načítání hry: " + ex.Message);
                    }
                }
            }
        }

        private void StartGameFromSavedState(GameSave gameSave)
        {
            
        }
    }
    }
    
