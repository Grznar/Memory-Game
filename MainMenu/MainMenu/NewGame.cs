using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainMenu.NewGame;
namespace MainMenu
{
    public partial class NewGame : Form
    {

        private GameBoard gameBoard;



        Label first, second;
         
        private int playerCount;
        public int[] score;
        private int currentPlayer = 1;
        private int cardCount;
        private bool pcPlayer;
        private int difficulty;
        private bool playerRound;
        private string[] names;
        private bool isSound;
        private bool isLoading = false;
        private bool locked = false;
        private int backImageId = -1;



        private List<int> flippedLabels = new List<int>();
        
        public List<int> cardImagesIds = new List<int>();
        public List<int> FlippedLabelsIndex { get; set; }
        
        private Dictionary<int, bool> alreadyFlipped;
        List<int> rightFlipped = new List<int>();
        Random rnd = new Random();

        public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost, bool isSound, bool isLoading = false)
        {
            InitializeComponent();

            this.playerCount = playerNumber;
            this.pcPlayer = pcPlayer;
            this.cardCount = cardNumber;
            this.difficulty = obtiznost;
            this.isSound = isSound;
            
            score = new int[playerNumber];
            playerRound = true;

            gameBoard = new GameBoard(cardCount, tableLayoutPanel1);
            gameBoard.CardClicked += label_Click;
            gameBoard.InitializeBoard(isLoading,statusStrip1,toolStrip1);
            
            
            
            AlreadyFlipped();
            
            

            currentPlayer = 1;
            if (!isLoading) names = GetNames(playerNumber, pcPlayer);
            else
            {
                names = new string[playerNumber];
                for (int i = 0; i < playerNumber; i++)
                {
                    names[i] = "Hráč " + (i + 1);
                }
            }
            ShowScore();

        }



        private void AlreadyFlipped()
        {
            alreadyFlipped = new Dictionary<int, bool>();
            for(int i=0;i<tableLayoutPanel1.Controls.Count;i++)
            {
                alreadyFlipped.Add(i, false);
            }
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

                    nameInputForm.Controls.Add(label);
                    nameInputForm.Controls.Add(textbox);
                    nameInputForm.Controls.Add(btn);


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
            string playerName = names[currentPlayer - 1];
            toolStripStatusLabel1.Text = "Hráč " + playerName + " je na tahu. Počet jeho bodů: " + score[currentPlayer - 1];

        }
        private void EndScore()
        {
            string[] namesClone = names;
            Array.Sort(score, namesClone);
            Array.Reverse(namesClone);
            Array.Reverse(score);
            string output = null;
            for (int i = 0; i < playerCount; i++)
            {
                output += "Hráč " + namesClone[i] + " , měl skoré: " + score[i] + "\n";
            }

            MessageBox.Show(output, "Konečné skóre");
            Score scoreboard = new Score();
            scoreboard.LoadScoreData();
            scoreboard.ShowDialog();
        }

        private async void label_Click(object sender, EventArgs e)
        {
            locked = false;
            if (locked || !playerRound || first != null && second != null) return;
            {

            }

            Label clickedLabel = sender as Label;
            if (clickedLabel == null)
            {

                return;
            }
            if((int)clickedLabel.Tag==backImageId)
            {
                return;
            }

            if (first == null)
            {
                first = clickedLabel;
                gameBoard.FlipCardFront(first);
                Console.WriteLine("Pridana hodnota do flipped " + (int)first.Tag);
                if (alreadyFlipped.TryGetValue(tableLayoutPanel1.Controls.IndexOf(first),out bool hodnotaF))
                {
                    if (hodnotaF == false)
                    {
                        alreadyFlipped[tableLayoutPanel1.Controls.IndexOf(first)] = true;
                        flippedLabels.Add((int)first.Tag);
                        
                    }
                
                }
                
                gameBoard.HiddenLabels.Remove((int)first.Tag);
                
                return;
            }

            if (second == null)
            {
                second = clickedLabel;
                gameBoard.FlipCardFront(second);
                if (alreadyFlipped.TryGetValue(tableLayoutPanel1.Controls.IndexOf(second), out bool hodnotaS))
                {
                    if (hodnotaS == false)
                    {
                        alreadyFlipped[tableLayoutPanel1.Controls.IndexOf(second)] = true;
                        flippedLabels.Add((int)second.Tag);

                        Console.WriteLine("Pridana hodnota do flipped " + (int)second.Tag);
                    }
                }
                gameBoard.HiddenLabels.Remove((int)second.Tag);
            }

            if ((int)first.Tag == (int)second.Tag)
            {
                score[currentPlayer - 1]++;
                ShowScore();
                flippedLabels.Remove((int)first.Tag);
                flippedLabels.Remove((int)second.Tag);
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(first));
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(second));
                gameBoard.MatchedPairs[tableLayoutPanel1.Controls.IndexOf(first)] = (int)(first.Tag);
                gameBoard.MatchedPairs[tableLayoutPanel1.Controls.IndexOf(second)] = (int)(second.Tag);
                first.Tag = backImageId;
                second.Tag = backImageId;
                first = null;
                second = null;


                WinnerCheck();
            }
            else
            {
                await Task.Delay(1000);
                timer1.Start();

                currentPlayer = (currentPlayer % playerCount) + 1;
                ShowScore();
            }

            //Console.WriteLine("Hidden Labels : ");
            //foreach (int i in hiddenLabels) Console.Write(" ," + i);
            //Console.WriteLine();
            //Console.WriteLine("Flipped labels : ");
            //foreach (int i in flippedLabels) Console.Write(" ," + i)
            
        }

        private void WinnerCheck()
        {
            Console.WriteLine("Pocet hidden a flipped :");
            Console.WriteLine(gameBoard.HiddenLabels.Count());
            Console.WriteLine(flippedLabels.Count());
            int totalCards = cardCount * cardCount;
            if (rightFlipped.Count == totalCards)
            {
                EndScore();
            }


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            timer1.Stop();
            if (first != null && (int)first.Tag != backImageId)
            {

                gameBoard.FlipCardBack(first);

            }
            if (second != null && (int)second.Tag != backImageId)
            {

                gameBoard.FlipCardBack(second);

            }


            first = null;
            second = null;
            locked = true;
            if (pcPlayer && currentPlayer == 2) ComputerTurn();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private int GetRight()
        {

            switch (difficulty)
            {
                case 1:
                    {
                        return 30;

                    }
                case 2:
                    {
                        return 60;

                    }
                case 3:
                    {
                        return 100;

                    }
                default: return 60;
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            StartingMenu startingMenu = new StartingMenu();
            startingMenu.Show();
            this.Visible = false;
        }

        private async void ComputerTurn()
        {
            playerRound = false;


            await Task.Delay(1000);


            


            bool chance = GetRight() >= rnd.Next(100);
            int indexFirstLabel = -1;
            int indexSecondLabel = -1;
            Console.WriteLine("Šance: "+chance);
            if (chance)
            {
                bool nasel = false;
                
                for (int i = 0; i < flippedLabels.Count && !nasel; i++)
                {
                    for (int j = i + 1; j < flippedLabels.Count; j++)
                    {
                        if (flippedLabels[i] == flippedLabels[j])
                        {
                            indexFirstLabel = flippedLabels[i]; indexSecondLabel = flippedLabels[j];
                            nasel = true;
                            Console.WriteLine("Našel stejný par ve flipped");



                            break;
                        }
                    }
                }
            }
            Console.Write("Hidden indexy jsou");
            foreach (int i in gameBoard.HiddenLabels) Console.Write(+i + " ");
            bool pridatFirst = false;
            if (indexFirstLabel == -1)
            {
                if (gameBoard.HiddenLabels.Count > 0)
                {
                    Console.WriteLine("Nahodny prvni obrazek");
                    int indexFirstRandom = rnd.Next(gameBoard.HiddenLabels.Count);
                    indexFirstLabel = gameBoard.HiddenLabels[indexFirstRandom];
                    gameBoard.HiddenLabels.RemoveAt(indexFirstRandom);
                    pridatFirst = true;
                }
                else
                {
                    int indexFirstRandom = rnd.Next(flippedLabels.Count);
                    indexFirstLabel = flippedLabels[indexFirstRandom];


                    

                    
                }

                bool foundSecond = false;
                if (chance && indexSecondLabel==-1)
                {

                    for (int index = 0; index < flippedLabels.Count - 1 && !foundSecond; index++)
                    {
                        if (flippedLabels[index] == indexFirstLabel)
                        {
                            Console.WriteLine("Nasel second in flipped po prvnim random");
                            indexSecondLabel = flippedLabels[index];
                            foundSecond = true;

                        }
                    }
                }
                


                if (indexSecondLabel == -1)
                {

                    if (gameBoard.HiddenLabels.Count >= 1)
                    {
                        int indexS = rnd.Next(gameBoard.HiddenLabels.Count);
                        
                        indexSecondLabel = gameBoard.HiddenLabels[indexS];
                        gameBoard.HiddenLabels.RemoveAt(indexS);
                        flippedLabels.Add(indexSecondLabel);
                        Console.WriteLine("Druhy se bere z hidden");
                    }
                    else
                    {

                        int indexS = rnd.Next(flippedLabels.Count);
                        indexSecondLabel = flippedLabels[indexS];
                        Console.WriteLine("Druhy se bere z flipped");
                       
                    }


                   
                   
                }

                if (pridatFirst == true) flippedLabels.Add(indexFirstLabel);
            }
            
            


            await Task.Delay(1000);
            Console.WriteLine("Tag prvniho je " + indexFirstLabel);
            Console.WriteLine("Tag druheho je " + indexSecondLabel);
            Label firstLabel = null;

            Label secondLabel = null;
            List<Label> labels = tableLayoutPanel1.Controls.OfType<Label>().ToList();

            firstLabel = labels.FirstOrDefault(label => label.Tag is int tag && tag == indexFirstLabel);
            secondLabel = labels.Last(label => label.Tag is int tag && tag == indexSecondLabel);

            if (firstLabel == null || secondLabel == null)
            {
                Console.WriteLine("Error: Could not find one or both labels");
                return;
            }
            gameBoard.FlipCardFront(firstLabel);
            await Task.Delay(1000);

            gameBoard.FlipCardFront(secondLabel);
            await Task.Delay(1000);
           
            if (indexFirstLabel == indexSecondLabel)
            {
                flippedLabels.RemoveAll(l => l == indexFirstLabel);
                flippedLabels.RemoveAll(l => l == indexSecondLabel);
                score[currentPlayer - 1]++;
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(firstLabel));
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(secondLabel));
                gameBoard.MatchedPairs[tableLayoutPanel1.Controls.IndexOf(firstLabel)] = (int)(firstLabel.Tag);
                gameBoard.MatchedPairs[tableLayoutPanel1.Controls.IndexOf(firstLabel)] = (int)(firstLabel.Tag);
                firstLabel.Tag = backImageId;
                secondLabel.Tag = backImageId;
                ShowScore();
                WinnerCheck();
                ComputerTurn();
            }
            else
            {
                timer2.Start();
                timer2.Stop();



                gameBoard.FlipCardBack(firstLabel);
                gameBoard.FlipCardBack(secondLabel);

                currentPlayer = (currentPlayer % playerCount) + 1;
                ShowScore();



                firstLabel = null;
                secondLabel = null;
                playerRound = true;
            }





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

                        
                        int indexFirstFlipped = tableLayoutPanel1.Controls.IndexOf(first);
                        int indexSecondFlipped = tableLayoutPanel1.Controls.IndexOf(second);

                        
                        GameSave gs = new GameSave
                        {
                            PlayerNumber = this.playerCount,
                            CardNumber = this.cardCount,
                            PCPlayer = this.pcPlayer,
                            Difficulty = this.difficulty,
                            IsSound = this.isSound,
                            Score = this.score,
                            CardImagesIds = this.cardImagesIds,
                            RightFlipped = this.rightFlipped,
                            CardPositions = cardPositions,
                            IndexFirstFlipped = indexFirstFlipped,
                            IndexSecondFlipped = indexSecondFlipped,
                            MatchedPairs = new Dictionary<int, int>(gameBoard.MatchedPairs)
                        };

                       
                        GameSaveManager.SaveGame(gs, sfd.FileName);

                        MessageBox.Show("Hra byla úspěšně uložena.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při ukládání hry: " + ex.Message);
                    }
                }
            }
        }



        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadGame();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void LoadGame()
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
                        this.score = loaded.Score;
                        this.cardImagesIds = loaded.CardImagesIds;
                        this.rightFlipped = loaded.RightFlipped;
                        

                        gameBoard.InitializeBoard(isLoading: true);

                        
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
                            first = tableLayoutPanel1.Controls[loaded.IndexFirstFlipped] as Label;
                            if (first != null && (int)first.Tag != backImageId)
                            {
                                gameBoard.FlipCardFront(first);
                            }
                        }
                        else
                        {
                            first = null;
                        }

                        
                        if (loaded.IndexSecondFlipped >= 0 && loaded.IndexSecondFlipped < tableLayoutPanel1.Controls.Count)
                        {
                            second = tableLayoutPanel1.Controls[loaded.IndexSecondFlipped] as Label;
                            if (second != null && (int)second.Tag != backImageId)
                            {
                                gameBoard.FlipCardFront(second);
                            }
                        }
                        else
                        {
                            second = null;
                        }

                        ShowScore();
                        MessageBox.Show("Hra byla úspěšně načtena.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při načítání hry: " + ex.Message);
                    }
                }
            }
        }
    }


    }



