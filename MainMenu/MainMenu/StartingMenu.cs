﻿using System;
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
        public StartingMenu(int playerNumber,int cardNumber,bool pcPlayer,int obtiznost,bool isSound)
        {
            InitializeComponent();
            
            {
                this.playerNumber = playerNumber;
                this.difficulty = obtiznost;
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

        private void loadGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Herní soubory (*.hra)|*.hra", 
                Title = "Načíst uloženou hru" 
            };

            
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;

                    
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        
                        BinaryFormatter formatter = new BinaryFormatter();
                        GameState loadedGameState = (GameState)formatter.Deserialize(fs);

                        
                        NewGame gameWindow = new NewGame(
                            loadedGameState.PlayerNumber,
                            loadedGameState.CardNumber,
                            loadedGameState.PcPlayer,
                            loadedGameState.Difficulty,
                            loadedGameState.IsSound,
                            true 
                        );

                        
                        gameWindow.LoadGameDetails(loadedGameState);

                        
                        gameWindow.Show();
                        this.Hide();
                    }
                }
                catch (Exception)
                {
                   
                    MessageBox.Show("Chyba při načítání uložené hry");
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            loadGame();
        }
    }
}
