    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Text;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml;
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
           public List<int> FlippedLabelsIndex { get; set; }
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
                MessageBox.Show("Složka images neexsituje!!");
                return;
            }

            
            string[] imageFiles = Directory.GetFiles(imagesPath, "*.png"); 

            for (int i = 0; i < imageFiles.Length; i++)
            {
               
                if (Path.GetFileName(imageFiles[i]).Equals("BackImage.png", StringComparison.OrdinalIgnoreCase))
                {
                    backImage = Image.FromFile(imageFiles[i]);
                    continue;
                }

                
                cardImages.Add(Image.FromFile(imageFiles[i]));
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
                            names[i] = textbox.Text;
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

            }
            private void UpdateScoreLabel()
            {
                if (names[playerCurrent - 1] == string.Empty)
                {
                    toolStripStatusLabel1.Text = "Hráč " + (playerCurrent - 1) + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];
                }
                toolStripStatusLabel1.Text = "Hráč " + names[playerCurrent - 1] + " je na tahu. " + " Počet jeho bodů: " + score[playerCurrent - 1];

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
                if (!playerRound) return;
                if (first != null && second != null)
                {
                    return;
                }
                Label clickedLabel = sender as Label;
                if (clickedLabel == null || clickedLabel.Image != backImage)
                {
                    return;
                }
                
                if (first == null)
                {
                    first = clickedLabel;
                    first.Image = clickedLabel.Tag as Image;
                    flippedLabels.Add(first);
                    return;
                }

                second = clickedLabel;
             second.Image = clickedLabel.Tag as Image;
            flippedLabels.Add(second);


                if (first.Image == second.Image)
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
            Label label;


            for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                label = tableLayoutPanel1.Controls[i] as Label;
                if (label != null && label.Image == backImage)
                {

                    return;
                }
            }

            SaveGameDetails();
            DisplayScores();


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (first != null && first.Image != backImage)
            {
                first.Image = backImage;
            }
            if (second != null && second.Image != backImage) second.Image = backImage;


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
                    Tag=cardImages[randIcons[i]],
                    ImageAlign = ContentAlignment.MiddleCenter,
                    AutoSize = false,
                    Size=new Size(100,100),
                    Dock = DockStyle.Fill
                };

                
                

                
                
                label.Click += label_Click;
                tableLayoutPanel1.Controls.Add(label);
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
                    public List<string> CardIcons { get; set; }
                    public List<bool> CardVisibility { get; set; }
                    public List<string> Names { get; set; }
                    public bool IsSound { get; set; }
                    public List<int> FlippedLabelsIndex { get; set; }
                    public GameState(List<bool> cardVisibility, int playerNumber, int[] scores, int playerCurrent, int difficulty, bool pcPlayer, int cardNumber, List<string> cardIcons, List<string> names, bool isSound, List<int> flippedLabelsIndex)
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
                private List<string> GetCardIcons()
                {
            List<string> icons = new List<string>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label && label.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        label.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        string base64String = Convert.ToBase64String(ms.ToArray());
                        icons.Add(base64String);
                    }
                }
                else
                {
                    icons.Add(null); 
                }
            }
            return icons;
        }

                public void SaveGame(string filePath)
                {
                    List<int> flippedLabelsIndex= new List<int>();
                    foreach(Label label in flippedLabels)
                    {
                    int index = tableLayoutPanel1.Controls.IndexOf(label);
                    flippedLabelsIndex.Add(index);    
                    }
                    GameState gamestate = new GameState(
                    playerNumber: this.playerNumber, 
                    scores: this.score,
                    cardNumber: this.cardNumber,
                    pcPlayer: this.pcPlayer,
                    difficulty: this.difficulty,
                    playerCurrent: this.playerCurrent,
                    cardIcons: GetCardIcons(),
                    cardVisibility: GetCardVisibility(),
                    names: this.names.ToList(),
                    isSound : this.isSound,
                    flippedLabelsIndex: flippedLabelsIndex
            



                        );
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
                    if (label.Image == backImage) visibility.Add(false);
                    else if (label.Image==label.Tag) visibility.Add(true);
                        }
                    }
                    return visibility;
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
            List<Label> hiddenLab = new List<Label>();
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Label label && label.Image == backImage)
                {
                    hiddenLab.Add(label);
                }
            }

            if (hiddenLab.Count == 0)
            {
                playerCurrent = (playerCurrent % playerNumber) + 1;
                UpdateScoreLabel();
                return;
            }

            Random random = new Random();
            Label firstLabel = null;
            Label secondLabel = null;
            bool found = false;
            bool pokracovat = GetRight() >= random.Next(100);
            if (pokracovat)
            {
                
                for (int i = 0; i < flippedLabels.Count; i++)
                {
                    for (int j = i + 1; j < flippedLabels.Count; j++)
                    {
                        if (flippedLabels[i].Tag == flippedLabels[j].Tag && flippedLabels[i] != flippedLabels[j])
                        {
                            firstLabel = flippedLabels[i];
                            secondLabel = flippedLabels[j];
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
            }

            if (found && firstLabel != null && secondLabel != null)
            {

                firstLabel.Image = firstLabel.Tag as Image;
                await Task.Delay(1000);
                secondLabel.Image = secondLabel.Tag as Image;
                await Task.Delay(1000);
                flippedLabels.Remove(firstLabel);
                flippedLabels.Remove(secondLabel);
                score[playerCurrent - 1]++;
                UpdateScoreLabel();
                
            }
            else if(pokracovat && !found)
            {
                int firstIndex = random.Next(hiddenLab.Count);
                firstLabel = hiddenLab[firstIndex];
                hiddenLab.RemoveAt(firstIndex);
                firstLabel.Image = firstLabel.Tag as Image;
                await Task.Delay(1000);
                bool found2 = false;
                foreach(Label label in flippedLabels)
                {
                    if(label.Tag==firstLabel.Tag&&label!=firstLabel)
                    {
                        secondLabel = label;
                        await Task.Delay(1000);
                        secondLabel.Image= secondLabel.Tag as Image;
                        await Task.Delay(1000);
                        flippedLabels.Remove(secondLabel);
                        score[playerCurrent - 1]++;
                        UpdateScoreLabel();
                        
                        found2 = true;
                        break;
                    }
                }
                if(!found2)
                {
                    if (hiddenLab.Count > 0)
                    {
                        int secondIndex = random.Next(hiddenLab.Count);
                        secondLabel = hiddenLab[secondIndex];
                        await Task.Delay(1000);
                        secondLabel.Image = secondLabel.Tag as Image;
                        hiddenLab.Remove(secondLabel);
                        await Task.Delay(1000);
                    }
                    else
                    {

                        playerCurrent = (playerCurrent % playerNumber) + 1;
                        UpdateScoreLabel();
                        return;
                    }

                    flippedLabels.Add(firstLabel);
                    flippedLabels.Add(secondLabel);
                }
            }
            else
            {
                
                int firstIndex = random.Next(hiddenLab.Count);
                firstLabel = hiddenLab[firstIndex];
                hiddenLab.RemoveAt(firstIndex);
                firstLabel.Image = firstLabel.Tag as Image;
                await Task.Delay(1000); 

                if (hiddenLab.Count > 0) 
                {
                    int secondIndex = random.Next(hiddenLab.Count);
                    await Task.Delay(1000);
                    secondLabel = hiddenLab[secondIndex]; 
                    secondLabel.Image = secondLabel.Tag as Image;
                    hiddenLab.Remove(secondLabel);
                    await Task.Delay(1000);
                }
                else
                {
                    
                    playerCurrent = (playerCurrent % playerNumber) + 1;
                    UpdateScoreLabel();
                    return;
                }

                flippedLabels.Add(firstLabel);
                flippedLabels.Add(secondLabel);
            }

            
            if (firstLabel != null && secondLabel != null && firstLabel.Tag == secondLabel.Tag)
            {
                
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
