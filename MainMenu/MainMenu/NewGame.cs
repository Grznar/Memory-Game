    using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainMenu.GameLogic;
using static MainMenu.NewGame;

namespace MainMenu
{
    public partial class NewGame : Form
    {

        private GameBoard gameBoard;
        private GameScoreManager scoreManager;
        private GameLogic gameLogic;

        private int playerCount;
        public int[] score;
        
        private int cardCount;
        private bool pcPlayer;
        private int difficulty;
        
        private string[] names;
        private bool isSound;
        private bool isLoading = false;
        private int backImageId = -1;

        public List<int> cardImagesIds = new List<int>();
        public List<int> FlippedLabelsIndex { get; set; }
        

        public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost, bool isSound, bool isLoading = false)
        {
            InitializeComponent();

            this.playerCount = playerNumber;
            this.pcPlayer = pcPlayer;
            this.cardCount = cardNumber;
            this.difficulty = obtiznost;
            this.isSound = isSound;
            Console.WriteLine(isSound);
            score = new int[playerNumber];
            

            gameBoard = new GameBoard(cardCount, tableLayoutPanel1);
            
            gameBoard.InitializeBoard(isLoading,statusStrip1,toolStrip1);
            

            if (!isLoading) names = GetNames(playerNumber, pcPlayer);
            else
            {
                names = new string[playerNumber];
                for (int i = 0; i < playerNumber; i++)
                {
                    names[i] = "Hráč " + (i + 1);
                }
            }
            scoreManager = new GameScoreManager(names);
            gameLogic = new GameLogic(gameBoard, scoreManager,
            playerNumber, cardCount, pcPlayer, obtiznost, isSound, isLoading);

            gameBoard.CardClicked += async (sender, e) =>
            {
                Label clickedLabel = sender as Label;
                if (clickedLabel != null)
                {
                    await gameLogic.OnCardClicked(clickedLabel);
                    ShowScore();
                }
            };
            gameLogic.ScoreUpdated += ShowScore;
            gameLogic.GameEnded += EndGame;
           


            ShowScore();

        }

        

        private string[] GetNames(int playerNumber, bool pcPlayer)
        {
            if (this.names != null) return this.names;
            string[] names = new string[playerNumber];
            for (int i = 0; i < playerNumber; i++)
            {

                if (pcPlayer && i == 1)
                {
                    names[i] = "PC";
                    continue;
                }
                using (Form nameInputForm = new Form())
                {
                    nameInputForm.Text = "Zadejte jméno pro hráče " + (i + 1);
                    Label label = new Label();
                    label.Text = "Hráč " + (i + 1);
                    label.AutoSize = true;
                    label.Location = new Point(10, 10);
                    TextBox textbox = new TextBox();
                    textbox.Width = 200;
                    textbox.Location = new Point(10, 30);
                    Button btn = new Button();
                    btn.Text = "Ok";
                    btn.DialogResult = DialogResult.OK;
                    btn.Location = new Point(10, 60);
                    nameInputForm.MaximizeBox = false;
                    nameInputForm.MinimizeBox = false;
                    nameInputForm.Controls.Add(label);
                    nameInputForm.Controls.Add(textbox);
                    nameInputForm.Controls.Add(btn);
                    nameInputForm.StartPosition = FormStartPosition.CenterScreen;
                    nameInputForm.ShowIcon = false;

                    if (nameInputForm.ShowDialog() == DialogResult.OK)
                    {
                        if (textbox.Text != string.Empty)   
                        {
                            names[i] = textbox.Text;
                        }
                        else names[i] = ((i + 1).ToString());

                    }

                }
            }
            this.names = names;
            return names;
        }
       
        private void ShowScore()
        {
            int index = gameLogic.CurrentPlayer - 1;
            string name = scoreManager.GetPlayerName(index);
            int points = scoreManager.GetScore(index);
            toolStripStatusLabel1.Text = "Hráč " + name + " je na tahu. Počet bodů: " + points ;

        }
       

        

        
        private void timer1_Tick(object sender, EventArgs e)
        {

            timer1.Stop();
            gameLogic.OnTimer1Tick();
        }

        

        

        private void btnMenu_Click(object sender, EventArgs e)
        {
            
            StartingMenu startingMenu = new StartingMenu();
            startingMenu.Show();
            this.Visible = false;
            this.Dispose();
        }

       
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveGame();
        }


        private void SaveGame()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Pexeso Saved Game|*.save";
                sfd.Title = "Uložit hru";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        
                        List<int> cardPositions = new List<int>();
                        for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
                        {
                            Label lbl = tableLayoutPanel1.Controls[i] as Label;
                            if (lbl != null && lbl.Tag is int tag)
                            {
                                cardPositions.Add(tag);
                            }
                        }

                        int indexFirstFlipped = tableLayoutPanel1.Controls.IndexOf(gameLogic.first);
                        int indexSecondFlipped = tableLayoutPanel1.Controls.IndexOf(gameLogic.second);

                        GameSave gs = new GameSave
                        {
                            PlayerNumber = this.playerCount,
                            CardNumber = this.cardCount,
                            PCPlayer = this.pcPlayer,
                            Difficulty = this.difficulty,
                            IsSound = this.isSound,
                            Score = scoreManager.GetAllScores(),
                            Names = scoreManager.GetAllNames(),
                            CardImagesIds = this.cardImagesIds,
                            CardPositions = cardPositions,
                            IndexFirstFlipped = indexFirstFlipped,
                            IndexSecondFlipped = indexSecondFlipped,
                            MatchedPairs = new Dictionary<int, int>(gameBoard.MatchedPairs),
                            CurrentPlayerOnTurn = gameLogic.CurrentPlayer,
                            HiddenLabels = gameBoard.HiddenLabels,
                            FlippedLabels = gameLogic.flippedLabels,
                            GameStateSave = gameLogic.gameState
                        };

                        
                        GameSaveManager.SaveGame(gs, sfd.FileName);

                        MessageBox.Show("Hra byla úspěšně uložena.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při ukládání hry: " + ex.Message);
                        return;
                    }
                }
            }
        }




        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        public void LoadGame()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Pexeso Saved Game|*.save";
                ofd.Title = "Načíst hru";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        
                        GameSave loaded = GameSaveManager.LoadGame(ofd.FileName);

                        this.playerCount = loaded.PlayerNumber;
                        this.cardCount = loaded.CardNumber;
                        this.pcPlayer = loaded.PCPlayer;
                        this.difficulty = loaded.Difficulty;
                        this.isSound = loaded.IsSound;
                        this.names = loaded.Names;


                        if (scoreManager == null)
                        {
                            scoreManager = new GameScoreManager(loaded.Names);
                        }
                        else
                        {
                            for (int i = 0; i < loaded.Names.Length; i++)
                            {
                                scoreManager.SetPlayerName(i, loaded.Names[i]);
                            }
                            scoreManager.SetScores(loaded.Score);
                        }


                        gameBoard.InitializeBoard(isLoading: true, statusStrip1, toolStrip1);

                        
                        List<int> cardPositions = loaded.CardPositions;
                        for (int i = 0; i < cardPositions.Count; i++)
                        {
                            Label lbl = tableLayoutPanel1.Controls[i] as Label;
                            if (lbl != null)
                            {
                                lbl.Tag = cardPositions[i];
                                lbl.Image = gameBoard.GetBackImage();
                                lbl.Enabled = true;
                            }
                        }

                        
                        gameBoard.MatchedPairs = new Dictionary<int, int>(loaded.MatchedPairs);
                        foreach (var kvp in gameBoard.MatchedPairs)
                        {
                            int index = kvp.Key;
                            int cardId = kvp.Value;

                            if (index >= 0 && index < tableLayoutPanel1.Controls.Count)
                            {
                                Label lbl = tableLayoutPanel1.Controls[index] as Label;
                                if (lbl != null)
                                {
                                    lbl.Tag = cardId;
                                    gameBoard.FlipCardFront(lbl);
                                    lbl.Tag = backImageId;
                                }
                            }
                        }

                       
                        if (loaded.IndexFirstFlipped >= 0 && loaded.IndexFirstFlipped < tableLayoutPanel1.Controls.Count)
                        {
                            gameLogic.first = tableLayoutPanel1.Controls[loaded.IndexFirstFlipped] as Label;
                            if (gameLogic.first != null && (int)gameLogic.first.Tag != backImageId)
                            {
                                gameLogic.gameState = GameState.OneCardFlipped;
                                gameBoard.FlipCardFront(gameLogic.first);
                            }
                        }
                        else
                        {
                            gameLogic.first = null;
                        }

                        if (loaded.IndexSecondFlipped >= 0 && loaded.IndexSecondFlipped < tableLayoutPanel1.Controls.Count)
                        {
                            gameLogic.second = tableLayoutPanel1.Controls[loaded.IndexSecondFlipped] as Label;
                            if (gameLogic.second != null && (int)gameLogic.second.Tag != backImageId)
                            {
                                gameLogic.gameState = GameState.Processing;
                                gameBoard.FlipCardFront(gameLogic.second);
                            }
                        }
                        else
                        {
                            gameLogic.second = null;
                        }

                        
                        gameLogic.currentPlayer = loaded.CurrentPlayerOnTurn;
                        gameLogic.flippedLabels = loaded.FlippedLabels;
                        gameBoard.HiddenLabels = loaded.HiddenLabels;
                        gameLogic.gameState = loaded.GameStateSave;

                        ShowScore();

                        MessageBox.Show("Hra byla úspěšně načtena.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při načítání hry: " + ex.Message);
                        return;
                    }
                }
            }
        }

        public void RestoreFromGameSave(GameSave loaded)
        {
            this.playerCount = loaded.PlayerNumber;
            this.cardCount = loaded.CardNumber;
            this.pcPlayer = loaded.PCPlayer;
            this.difficulty = loaded.Difficulty;
            this.isSound = loaded.IsSound;
            this.names = loaded.Names;


            if (scoreManager == null)
            {
                scoreManager = new GameScoreManager(loaded.Names);
            }
            else
            {
                for (int i = 0; i < loaded.Names.Length; i++)
                {
                    scoreManager.SetPlayerName(i, loaded.Names[i]);
                }
                scoreManager.SetScores(loaded.Score);
            }


            gameBoard.InitializeBoard(isLoading: true, statusStrip1, toolStrip1);


            List<int> cardPositions = loaded.CardPositions;
            for (int i = 0; i < cardPositions.Count; i++)
            {
                Label lbl = tableLayoutPanel1.Controls[i] as Label;
                if (lbl != null)
                {
                    lbl.Tag = cardPositions[i];
                    lbl.Image = gameBoard.GetBackImage();
                    lbl.Enabled = true;
                }
            }


            gameBoard.MatchedPairs = new Dictionary<int, int>(loaded.MatchedPairs);
            foreach (var kvp in gameBoard.MatchedPairs)
            {
                int index = kvp.Key;
                int cardId = kvp.Value;

                if (index >= 0 && index < tableLayoutPanel1.Controls.Count)
                {
                    Label lbl = tableLayoutPanel1.Controls[index] as Label;
                    if (lbl != null)
                    {
                        lbl.Tag = cardId;
                        gameBoard.FlipCardFront(lbl);
                        lbl.Tag = backImageId;
                    }
                }
            }


            if (loaded.IndexFirstFlipped >= 0 && loaded.IndexFirstFlipped < tableLayoutPanel1.Controls.Count)
            {
                gameLogic.first = tableLayoutPanel1.Controls[loaded.IndexFirstFlipped] as Label;
                if (gameLogic.first != null && (int)gameLogic.first.Tag != backImageId)
                {
                    gameLogic.gameState = GameState.OneCardFlipped;
                    gameBoard.FlipCardFront(gameLogic.first);
                }
            }
            else
            {
                gameLogic.first = null;
            }

            if (loaded.IndexSecondFlipped >= 0 && loaded.IndexSecondFlipped < tableLayoutPanel1.Controls.Count)
            {
                gameLogic.second = tableLayoutPanel1.Controls[loaded.IndexSecondFlipped] as Label;
                if (gameLogic.second != null && (int)gameLogic.second.Tag != backImageId)
                {
                    gameLogic.gameState = GameState.Processing;
                    gameBoard.FlipCardFront(gameLogic.second);
                }
            }
            else
            {
                gameLogic.second = null;
            }


            gameLogic.currentPlayer = loaded.CurrentPlayerOnTurn;
            gameLogic.flippedLabels = loaded.FlippedLabels;
            gameBoard.HiddenLabels = loaded.HiddenLabels;
            gameLogic.gameState = loaded.GameStateSave;

            ShowScore();
        }
        private void EndGame()
        {

            int maxScore = int.MinValue;
            int minScore = int.MaxValue;
            List<int> winners = new List<int>();
            List<int> losers = new List<int>();

            // Najděte maximální a minimální skóre
            for (int i = 0; i < playerCount; i++)
            {
                int playerScore = scoreManager.GetScore(i); // Předpokládá se, že GetScore(i) vrací PairsFound

                if (playerScore > maxScore)
                {
                    maxScore = playerScore;
                    winners.Clear();
                    winners.Add(i);
                }
                else if (playerScore == maxScore)
                {
                    winners.Add(i);
                }

                if (playerScore < minScore)
                {
                    minScore = playerScore;
                    losers.Clear();
                    losers.Add(i);
                }
                else if (playerScore == minScore)
                {
                    losers.Add(i);
                }
            }

            // Načtěte existující skóre
            List<ScoreData> allScores = GameScoreSaveManager.LoadScoreData();

            // Aktualizujte skóre pro všechny hráče
            for (int i = 0; i < playerCount; i++)
            {
                string playerName = scoreManager.GetPlayerName(i);
                int pairsFoundThisGame = scoreManager.GetScore(i); // Předpokládá se, že tento počet je aktuální PairsFound

                // Najděte existující záznam pro hráče nebo vytvořte nový
                var scoreData = allScores.FirstOrDefault(s => s.PlayerName.Equals(playerName, StringComparison.OrdinalIgnoreCase));
                if (scoreData != null)
                {
                    // Aktualizace PairsFound a TotalCards
                    scoreData.PairsFound += pairsFoundThisGame;
                    scoreData.TotalCards += cardCount * cardCount;

                    // Přičtení výhry, pokud je hráč vítězem
                    if (winners.Contains(i))
                    {
                        scoreData.Wins += 1;
                    }

                    // Přičtení prohry, pokud je hráč poraženým
                    if (losers.Contains(i))
                    {
                        scoreData.Losses += 1;
                    }
                }
                else
                {
                    // Vytvoření nového záznamu pro hráče
                    ScoreData newScore = new ScoreData(playerName, 0, 0, pairsFoundThisGame, cardCount * cardCount);

                    // Přičtení výhry, pokud je hráč vítězem
                    if (winners.Contains(i))
                    {
                        newScore.Wins = 1;
                    }

                    // Přičtení prohry, pokud je hráč poraženým
                    if (losers.Contains(i))
                    {
                        newScore.Losses = 1;
                    }

                    allScores.Add(newScore);
                }
            }

            // Uložte aktualizované skóre
            GameScoreSaveManager.SaveScoreData(allScores);

            // Zobrazte zprávu o ukončení hry
            MessageBox.Show("Hra byla ukončena. Skóre bylo uloženo.", "Konec Hry", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Otevřete formulář Score a zavřete aktuální formulář
            Score scoreForm = new Score();
            scoreForm.Show();
            this.Close();
        }
    }


    }



