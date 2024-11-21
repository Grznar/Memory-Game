using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
namespace MainMenu
{
    public partial class NewGame : Form
        {
            Label first, second;
            private int[] score;
            private int playerNumber;
            private int playerCurrent = 1;
            private int cardNumber;
            private bool pcPlayer;
            private int difficulty;
            private bool playerRound;
            private string[] names;
            private bool isSound;

        private List<Label> flippedLabels = new List<Label>();
        private List<Image> cardImages= new List<Image>();
        private Image backImage;
        private List<int> cardImagesIds = new List<int>();
        public List<int> FlippedLabelsIndex { get; set; }
        private List<Label> hiddenLabels = new List<Label>();
        private int backImageId = -1;
            public NewGame(int playerNumber, int cardNumber, bool pcPlayer, int obtiznost, bool isSound, bool isLoading = false)
            {
                InitializeComponent();
                this.playerNumber = playerNumber;
                this.pcPlayer = pcPlayer;
                this.cardNumber = cardNumber;
                this.difficulty = obtiznost;
                this.isSound = isSound;
                FlippedLabelsIndex = new List<int>();
                score = new int[playerNumber];
                playerRound = true;
                LoadImages();
                SizeOfTable();
                IconsToPlace();
            
                tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);
                tableLayoutPanel1.Padding = new Padding(0, toolStrip1.Height, 0, 0);
            
                playerCurrent = 1;
                if (!isLoading) names = GetNames(playerNumber, pcPlayer);
                else
                {
                    names = new string[playerNumber];
                    for (int i = 0; i < playerNumber; i++)
                    {
                        names[i] = "Hráč " + (i + 1);
                    }
                }
                UpdateScoreLabel();

            }


            Random rnd = new Random();
            
        private void LoadImages()
        {
            
            string imagesPath = Path.Combine(Application.StartupPath, "Images");

            
            if (!Directory.Exists(imagesPath))
            {
                MessageBox.Show("Složka images neexisituje!!");
                return;
            }

            
            string[] imageFiles = Directory.GetFiles(imagesPath, "*.png"); 

            for (int i = 0; i < imageFiles.Length; i++)
            {

                if (Path.GetFileName(imageFiles[i]).Equals("BackImage.png", StringComparison.OrdinalIgnoreCase))
                {
                    backImage = Image.FromFile(imageFiles[i]);
                    backImageId = -1;
                    continue;
                }
                
                {
                    cardImages.Add(Image.FromFile(imageFiles[i]));
                    cardImagesIds.Add(i);
                }
            }

            if (cardImages.Count < 18)
            {
                MessageBox.Show("Není dost obrázků!");
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
                        if(textbox.Text!=string.Empty)
                        {
                            names[i] = textbox.Text;
                        }
                        else names[i]= ((i+1).ToString());
                            
                        }

                    }
                }
                this.names = names;
                return names;
            }
            private void SizeOfTable()
            {
            
                int rows = cardNumber;
                int columns = cardNumber;
                tableLayoutPanel1.RowCount = rows;
                tableLayoutPanel1.ColumnCount = columns;
                tableLayoutPanel1.ColumnStyles.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.Dock = DockStyle.Fill;


                for (int i = 0; i < columns; i++)
                {
                    tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
                }


                for (int i = 0; i < rows; i++)
                {
                    tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
                }

            tableLayoutPanel1.Controls.Clear();
        }
            private void UpdateScoreLabel()
            {
            string playerName = names[playerCurrent - 1];
            

            toolStripStatusLabel1.Text = "Hráč " + playerName + " je na tahu. Počet jeho bodů: " + score[playerCurrent - 1];

        }
            private void DisplayScores()
            {
                string[] namesClone = names;
                Array.Sort(score, namesClone);
                Array.Reverse(namesClone);
                Array.Reverse(score);
                string output = null;
                for (int i = 0; i < playerNumber; i++)
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
                if (!playerRound || first != null && second != null) return;
            
                Label clickedLabel = sender as Label;
                if (clickedLabel == null || (int)clickedLabel.Tag == backImageId)
                {
                
                return;
                }
           

            if (first == null)
                {
                    first = clickedLabel;
                    first.Image = cardImages[(int)first.Tag];
                    flippedLabels.Add(first);
                    hiddenLabels.Remove(first);
                    return;
                }

                second = clickedLabel;
                second.Image = cardImages[(int)second.Tag];
                flippedLabels.Add(second);
                hiddenLabels.Remove(second);


                if ((int)first.Tag == (int)second.Tag)
                {
                    score[playerCurrent - 1]++;
                    UpdateScoreLabel();
                    flippedLabels.Remove(first);
                    flippedLabels.Remove(second);
                
                    first = null;
                    second = null;


                    WinnerCheck();
                }
                else
                {
                await Task.Delay(1000);
                    timer1.Start();

                    playerCurrent = (playerCurrent % playerNumber) + 1;
                    UpdateScoreLabel();
                }


            }

            private void WinnerCheck()
            {
            


            if (flippedLabels.Count() == cardNumber * cardNumber)
            {
                SaveGameDetails();
                DisplayScores();
            }


             }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (first != null && (int)first.Tag != backImageId)
            {
                
                first.Image = backImage;
                
            }
            if (second != null && (int)second.Tag != backImageId)
            {
                
                second.Image = backImage;  
                
            }


            first = null;
            second = null;
            if (pcPlayer && playerCurrent == 2) ComputerTurn();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void IconsToPlace()
        {
            Label label;
            
            List<int> icons = new List<int>();
            Random rnd = new Random();
            for(int i=0;i<(cardNumber*cardNumber)/2;i++)
            {
                
                
                icons.Add(i);
                icons.Add(i);
            }

            List<int> randIcons = new List<int>();

            
            while (icons.Count > 0)
            {
                int randIndex = rnd.Next(icons.Count);  
                randIcons.Add(icons[randIndex]);  
                icons.RemoveAt(randIndex);  
            }
            tableLayoutPanel1.Controls.Clear();

            
            for (int i = 0; i < cardNumber * cardNumber; i++)
            {
                
                label = new Label
                {
                    Image = backImage,
                    Tag = randIcons[i],
                    ImageAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Size = new Size(100, 100),
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.FixedSingle,
                    Enabled = true,
                    
                };


                

                
                if (label != null)
                {
                    label.Click += label_Click;
                    
                }
                



               
                tableLayoutPanel1.Controls.Add(label);
                hiddenLabels.Add(label);
            }


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
                [Serializable]
                public class GameState
                {
                    public int PlayerNumber { get; set; }
                    public int[] Scores { get; set; }
                    public int PlayerCurrent { get; set; }
                    public int Difficulty { get; set; } 
                    public bool PcPlayer { get; set; }
                    public int CardNumber { get; set; }
                    public List<int> CardIcons { get; set; }
                    public List<bool> CardVisibility { get; set; }
                    public bool PlayerRound { get; set; }
                    public List<string> Names { get; set; }
                    public bool IsSound { get; set; }
                    public List<int> FlippedLabelsIndex { get; set; }
                    public List<int> HiddenLabelsIndex { get; set; }
                    public bool CurrentGameState { get; set; }
            public GameState(List<bool> cardVisibility, int playerNumber, int[] scores, int playerCurrent, int difficulty, bool pcPlayer, int cardNumber, List<int> cardIcons, List<string> names, bool isSound, List<int> flippedLabelsIndex)
                    {
                        PlayerNumber = playerNumber;
                        Scores = scores;
                        PlayerCurrent = playerCurrent;
                        Difficulty = difficulty;
                        PcPlayer = pcPlayer;
                        CardIcons = cardIcons;
                        CardNumber = cardNumber;
                        CardVisibility = cardVisibility;
                        Names = names;
                        IsSound = isSound;
                        FlippedLabelsIndex = flippedLabelsIndex;
                    }


                }
                private List<int> GetCardIcons()
                {
            List<int> icons = new List<int>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label && label.Image != null)
                {
                    icons.Add((int)label.Tag);
                }
                else
                {
                    icons.Add(-1); 
                }
            }
            return icons;
        }

                public void SaveGame(string filePath)
                {
                    
                    GameState gamestate = new GameState
                    {
                        PlayerNumber = this.playerNumber,                // Počet hráčů
                        Scores = this.score,                             // Skóre hráčů
                        PlayerCurrent = this.playerCurrent,              // Který hráč je na řadě
                        Names = this.names.ToList(),                    // Jména hráčů
                        CardNumber = this.cardNumber,                    // Počet karet
                        CardIcons = GetCardIcons(),                      // Ikony karet
                        CardVisibility = GetCardVisibility(),           // Viditelnost karet
                        PlayerRound = this.playerRound,                  // Je na řadě hráč
                        Difficulty = this.difficulty,                    // Obtížnost
                        IsSound = this.isSound,                          // Stav zvuku
                        PcPlayer = this.pcPlayer,                        // Je hráč proti počítači
                        FlippedLabelsIndex = GetFlippedLabelsIndex(),    // Indexy otočených karet
                        HiddenLabelsIndex = GetHiddenLabelsIndex(),      // Indexy skrytých karet
                        CurrentGameState = this.currentGameState,       // Stav hry (aktivní/ukončená)
                    }
                    
            



                       
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(fs, gamestate);
                    }

                }

                private void buttonSave_Click(object sender, EventArgs e)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog
                    {
                        Filter = "Herní soubory (*hra)|*.hra",
                        Title = "Uložit hru"
                    };
                    if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        SaveGame(saveFileDialog.FileName);
                    }
                }
        public void LoadGameDetails(GameState loadedGameState)
        {
           

                    this.playerNumber = loadedGameState.PlayerNumber;
                    this.cardNumber = loadedGameState.CardNumber;
                    this.pcPlayer = loadedGameState.PcPlayer;
                    this.difficulty = loadedGameState.Difficulty;
                    this.score = loadedGameState.Scores;
                    this.playerCurrent = loadedGameState.PlayerCurrent;
                    this.isSound = loadedGameState.IsSound;
                    this.FlippedLabelsIndex = loadedGameState.FlippedLabelsIndex;

                    
                    if (loadedGameState.Names != null)
                        this.names = loadedGameState.Names.ToArray();
                    else
                    {
                        this.names = new string[playerNumber];
                        for (int i = 0; i < playerNumber; i++)
                        {
                            this.names[i] = "Hráč " + (i + 1);
                        }
                    }

                    
                    SetCardIcons(loadedGameState.CardIcons);
                    UpdateScoreLabel();

                    
                    SetCardVisibility(loadedGameState.CardVisibility);

                    
                    flippedLabels.Clear();
                    foreach (int i in loadedGameState.FlippedLabelsIndex)
                    {
                        Label label = tableLayoutPanel1.Controls[i] as Label;
                        if (label != null)
                            this.flippedLabels.Add(label);
                    }
            
        }
        private void SetCardVisibility(List<bool> cardVisibility)
                {
                    for (int i = 0; i < cardVisibility.Count; i++)
                    {
                        if (tableLayoutPanel1.Controls[i] is Label label)
                        {
                    if (cardVisibility[i])
                    {
                        label.Image = label.Tag as Image;
                    }
                    else label.Image = backImage;
                        }
                    }
                }
        private void SetCardIcons(List<string> cardIcons)
        {
            for (int i = 0; i < cardIcons.Count; i++)
            {
                if (tableLayoutPanel1.Controls[i] is Label label)
                {
                    if (cardIcons[i] != null)
                    {
                        byte[] imageBytes = Convert.FromBase64String(cardIcons[i]);
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            Image cardImage = Image.FromStream(ms);
                            label.Tag = cardImage; 
                            label.Image = cardImage;
                        } 
                    } else
                        {
                            label.Image = null;
                        }
                    }
                }
            }
        
                private List<bool> GetCardVisibility()
                {
                    List<bool> visibility = new List<bool>();
                    foreach (Control control in tableLayoutPanel1.Controls)
                    {
                        if (control is Label label)
                        {
                        if ((int)label.Tag == backImageId) visibility.Add(false);
                    else visibility.Add(true);
                        }
                    }
                    return visibility;
                }
        private List<int> GetFlippedLabelsIndex()
        {
            List<int> flippedLabelsIndex = new List<int>();
            foreach (Label label in flippedLabels)
            {
                int index = tableLayoutPanel1.Controls.IndexOf(label);
                flippedLabelsIndex.Add(index); 
            }
            return flippedLabelsIndex;
        }

        private List<int> GetHiddenLabelsIndex()
        {
            List<int> hiddenLabelsIndex = new List<int>();
            foreach (Label label in hiddenLabels)
            {
                int index = tableLayoutPanel1.Controls.IndexOf(label);
                hiddenLabelsIndex.Add(index); 
            }
            return hiddenLabelsIndex;
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
           
            if (timer1.Enabled)
            {
                timer1.Stop();
            }

            await Task.Delay(1000);
            

            if (hiddenLabels.Count == 0)
            {
                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
                playerRound = true;
                return;
            }

            
            bool chance = GetRight() >= rnd.Next(100);
            Label firstLabel = null;
            Label secondLabel = null;
            
            Dictionary<int, List<Label>> tagDictionary = new Dictionary<int, List<Label>>();
            
            if (chance)
                {
                    Console.WriteLine("Šance" + chance);

                    
                    foreach (Label label in flippedLabels)
                    {
                        int tag = (int)label.Tag;
                        if (!tagDictionary.ContainsKey(tag))
                            tagDictionary[tag] = new List<Label>();
                        tagDictionary[tag].Add(label);
                    }

                

                foreach (var tag in tagDictionary)
                    {
                        if (tag.Value.Count > 1)
                        {
                            firstLabel = tag.Value[0];
                            secondLabel = tag.Value[1];
                        
                            break;
                        }
                    }


                }
            if (firstLabel==null)
            {
                int indexF = rnd.Next(hiddenLabels.Count);
                firstLabel = hiddenLabels[indexF];
                hiddenLabels.Remove(firstLabel);
                flippedLabels.Add(firstLabel);
                if (chance)
                {
                    foreach (Label label in flippedLabels)
                    {
                        if((int)label.Tag==(int)firstLabel.Tag && firstLabel!=label)
                        {
                            secondLabel = label;
                           
                        }
                    }
                }
                
                if(secondLabel==null)
                {
                    int indexS = rnd.Next(hiddenLabels.Count);
                    secondLabel = hiddenLabels[indexS];
                    hiddenLabels.Remove(secondLabel);
                    flippedLabels.Add(secondLabel);
                    Console.WriteLine("Random");
                }
                
            }
            Console.WriteLine("First label tag: " + firstLabel.Tag);
            Console.WriteLine("Second label tag: " + secondLabel.Tag);
            await Task.Delay(1000);
            firstLabel.Image = cardImages[(int)firstLabel.Tag];
            await Task.Delay(1000);
            secondLabel.Image = cardImages[(int)secondLabel.Tag];
            await Task.Delay(1000);

            if ((int)firstLabel.Tag== (int)secondLabel.Tag)    
            {
                score[playerCurrent - 1]++;
                UpdateScoreLabel();
                flippedLabels.Remove(firstLabel);
                flippedLabels.Remove(secondLabel);
                hiddenLabels.Remove(firstLabel);
                hiddenLabels.Remove(secondLabel);
                WinnerCheck();
                ComputerTurn();
            }
            else
            {
                await Task.Delay(1000);
                
                

                    firstLabel.Image = backImage;
                    secondLabel.Image = backImage;
                
                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();



                firstLabel = null;
                secondLabel= null;
                playerRound = true;
            }
            

                


        }



        

        [Serializable]
        public class GameData
        {
            public string PlayerName { get; set; }
            public int Wins { get; set; }
            public int Loses { get; set; }
            public int PairsFound { get; set; }
            public int TotalCards { get; set; }

            public GameData(string playerName, int pairsFound, int totalCards)
            {
                PlayerName = playerName;
                PairsFound = pairsFound;
                TotalCards = totalCards;

            }
        }
        private void SaveGameDetails()
        {
            string file = "gameResult.dat";
            List<GameData> gameResults;

            
            if (File.Exists(file))
            {
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    gameResults = (List<GameData>)formatter.Deserialize(fs);
                }
            }
            else
            {
                gameResults = new List<GameData>();
            }

            int maxScore = score.Max();
            int minScore = score.Min();

            
            for (int i = 0; i < playerNumber; i++)
            {
                GameData existingPlayer = null;

                
                foreach (GameData player in gameResults)
                {
                    if (player.PlayerName == names[i])
                    {
                        existingPlayer = player;
                        break;
                    }
                }

                if (existingPlayer != null)
                {
                    
                    existingPlayer.PairsFound += score[i];
                    existingPlayer.TotalCards += cardNumber * cardNumber;

                    if (score[i] == maxScore)
                    {
                        existingPlayer.Wins += 1;
                    }
                    else if (score[i] == minScore)
                    {
                        existingPlayer.Loses += 1;
                    }
                }
                else
                {
                    
                    GameData gameData = new GameData(
                        playerName: names[i],
                        pairsFound: score[i],
                        totalCards: cardNumber * cardNumber
                    );

                    if (score[i] == maxScore)
                    {
                        gameData.Wins = 1;
                        gameData.Loses = 0;
                    }
                    else if (score[i] == minScore)
                    {
                        gameData.Wins = 0;
                        gameData.Loses = 1;
                    }
                    else
                    {
                        gameData.Wins = 0;
                        gameData.Loses = 0;
                    }

                    gameResults.Add(gameData);
                }
            }

            
            using (FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, gameResults);
            }
        }

    }
    }
