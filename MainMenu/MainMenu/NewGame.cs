﻿using System;
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
        private Image backImage;

        private List<int> flippedLabels = new List<int>();
        Dictionary<int, Label> labelsForComputer = new Dictionary<int, Label>();
        private List<Image> cardImages = new List<Image>();
        public List<int> cardImagesIds = new List<int>();
        public List<int> FlippedLabelsIndex { get; set; }
        private List<int> hiddenLabels = new List<int>();
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


            LoadImages();
            SizeOfTable();
            IconsToPlace();
            AlreadyFlipped();
            
            tableLayoutPanel1.Padding = new Padding(0, 0, 0, statusStrip1.Height);
            tableLayoutPanel1.Padding = new Padding(0, toolStrip1.Height, 0, 0);

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
        private void SizeOfTable()
        {

            int rows = cardCount;
            int columns = cardCount;
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
            if (locked || !playerRound||first != null && second != null) return;
            {
                
            }
            
            Label clickedLabel = sender as Label;
            if (clickedLabel == null || (int)clickedLabel.Tag == backImageId)
            {
                
                return;
            }


            if (first == null)
            {
                first = clickedLabel;
                first.Image = cardImages[(int)first.Tag];
                
                if(alreadyFlipped.TryGetValue(tableLayoutPanel1.Controls.IndexOf(first),out bool hodnotaF))
                {
                    if (hodnotaF == false)
                    {
                        alreadyFlipped[tableLayoutPanel1.Controls.IndexOf(first)] = true;
                        flippedLabels.Add((int)first.Tag);
                        //Console.WriteLine("Pridana hodnota do flipped " + (int)first.Tag);
                    }
                
                }
                
                hiddenLabels.Remove((int)first.Tag);
                
                return;
            }

            if (second == null)
            {
                second = clickedLabel;
                second.Image = cardImages[(int)second.Tag];
                if (alreadyFlipped.TryGetValue(tableLayoutPanel1.Controls.IndexOf(second), out bool hodnotaS))
                {
                    if (hodnotaS == false)
                    {
                        alreadyFlipped[tableLayoutPanel1.Controls.IndexOf(second)] = true;
                        flippedLabels.Add((int)second.Tag);

                        //Console.WriteLine("Pridana hodnota do flipped " + (int)second.Tag);
                    }
                }
                hiddenLabels.Remove((int)second.Tag);
            }

            if ((int)first.Tag == (int)second.Tag)
            {
                score[currentPlayer - 1]++;
                ShowScore();
                flippedLabels.Remove((int)first.Tag);
                flippedLabels.Remove((int)second.Tag);
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(first));
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(second));
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
            Console.WriteLine(hiddenLabels.Count());
            Console.WriteLine(flippedLabels.Count());

            if (hiddenLabels.Count+flippedLabels.Count==0)
            {
                
                EndScore();
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
            locked = true;
            if (pcPlayer && currentPlayer == 2) ComputerTurn();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void IconsToPlace()
        {
            Label label;
            List<int> icons = new List<int>();
            Random rnd = new Random();

            
            if (isLoading)
            {
                List<int> randIcons = new List<int>(cardImagesIds); 
                tableLayoutPanel1.Controls.Clear();

                
                for (int i = 0; i < cardCount * cardCount; i++)
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

                    label.Click += label_Click;
                    tableLayoutPanel1.Controls.Add(label);
                    hiddenLabels.Add((int)label.Tag); 

                    
                }
            }
            else
            {
                
                for (int i = 0; i < (cardCount * cardCount) / 2; i++)
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

                
                for (int i = 0; i < cardCount * cardCount; i++)
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

                    label.Click += label_Click;
                    tableLayoutPanel1.Controls.Add(label);
                    hiddenLabels.Add((int)label.Tag); 

                }
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
            foreach (int i in hiddenLabels) Console.Write(+i + " ");
            bool pridatFirst = false;
            if (indexFirstLabel == -1)
            {
                if (hiddenLabels.Count > 0)
                {
                    Console.WriteLine("Nahodny prvni obrazek");
                    int indexFirstRandom = rnd.Next(hiddenLabels.Count);
                    indexFirstLabel = hiddenLabels[indexFirstRandom];
                    hiddenLabels.RemoveAt(indexFirstRandom);
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

                    if (hiddenLabels.Count >= 1)
                    {
                        int indexS = rnd.Next(hiddenLabels.Count);
                        
                        indexSecondLabel = hiddenLabels[indexS];
                        hiddenLabels.RemoveAt(indexS);
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
            //Console.WriteLine("First label tag: " + indexFirstLabel);
            //Console.WriteLine("Second label tag: " + indexSecondLabel);
            


            await Task.Delay(1000);
            Console.WriteLine("Tag prvniho je " + indexFirstLabel);
            Console.WriteLine("Tag druheho je " + indexSecondLabel);
            Label firstLabel = null;

            Label secondLabel = null;


            //foreach (Label label in tableLayoutPanel1.Controls.OfType<Label>())
            //{
            //    if (secondLabel == null && (int)label.Tag == indexSecondLabel)
            //    {
            //        secondLabel = label;
            //        Console.WriteLine("Index druheho je " + tableLayoutPanel1.Controls.IndexOf(label));
            //        Console.WriteLine("Tag druheho je " + secondLabel.Tag);


            //    }
            //    else if (firstLabel == null && (int)label.Tag == indexFirstLabel)
            //    {
            //        firstLabel = label;
            //         Console.WriteLine("Index prvniho je " + tableLayoutPanel1.Controls.IndexOf(label));
            //        Console.WriteLine("Tag prvniho je " + firstLabel.Tag);
            //    }

            //if (firstLabel != null && secondLabel != null)
            //{
            //    break;
            //}


            List<Label> labels = tableLayoutPanel1.Controls.OfType<Label>().ToList();

            firstLabel = labels.FirstOrDefault(label => label.Tag is int tag && tag == indexFirstLabel);
            secondLabel = labels.Last(label => label.Tag is int tag && tag == indexSecondLabel);

            if (firstLabel == null || secondLabel == null)
            {
                Console.WriteLine("Error: Could not find one or both labels");
                return;
            }
            firstLabel.Image = cardImages[(int)firstLabel.Tag];
            await Task.Delay(1000);
            
            secondLabel.Image = cardImages[(int)secondLabel.Tag];
            await Task.Delay(1000);
           
            if (indexFirstLabel == indexSecondLabel)
            {
                flippedLabels.RemoveAll(l => l == indexFirstLabel);
                flippedLabels.RemoveAll(l => l == indexSecondLabel);
                score[currentPlayer - 1]++;
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(firstLabel));
                rightFlipped.Add(tableLayoutPanel1.Controls.IndexOf(secondLabel));
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



                firstLabel.Image = backImage;
                secondLabel.Image = backImage;

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
        [Serializable]
        public class GameSave
        {
            public int PlayerNumber { get; set; }
            public int CardNumber { get; set; }
            public bool PCPlayer { get; set; }
            public int Difficulty { get; set; }
            public bool IsSound { get; set; }
            public List<int> CardImagesIds { get; set; }
            public List<string> CardImagePaths { get; set; }
            public List<int> FlippedLabelsIndex { get; set; }
            public int[] Score { get; set; }
            public List<int> HiddenLabelsIndex { get; set; }
            public List<int> RightFlipped { get; set; }
            public List <int> CardPositions { get; set; }

            public int IndexFirstFlipped { get; set; }

        }
        private void SaveGame()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Pexeso Saved Game Files|*.save";
                saveFileDialog.Title = "Uložit hru";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {

                        List<int> cardPositions = new List<int>();
                        foreach (Label label in tableLayoutPanel1.Controls)
                        {
                            cardPositions.Add((int)label.Tag); 
                        }

                        int indexFirstFlipped = tableLayoutPanel1.Controls.IndexOf(first);
                        
                        GameSave gameSave = new GameSave
                        {
                            PlayerNumber = this.playerCount,
                            CardNumber = this.cardCount,
                            Difficulty = this.difficulty,
                            IsSound = this.isSound,
                            Score = this.score,
                            CardImagesIds = cardImagesIds,
                            RightFlipped = rightFlipped,
                            CardPositions = cardPositions,
                            IndexFirstFlipped = indexFirstFlipped,

                        };

                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(fs, gameSave);
                        }

                        MessageBox.Show("Hra byla úspěšně uložena.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Došlo k chybě při ukládání hry: " + ex.Message);
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
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Pexeso Saved Game Files|*.save";
                openFileDialog.Title = "Načíst hru";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            GameSave loadedGame = (GameSave)formatter.Deserialize(fs);

                            
                            this.playerCount = loadedGame.PlayerNumber;
                            this.cardCount = loadedGame.CardNumber;
                            this.difficulty = loadedGame.Difficulty;
                            this.isSound = loadedGame.IsSound;
                            this.score = loadedGame.Score;
                            this.cardImagesIds = loadedGame.CardImagesIds;
                            this.rightFlipped = loadedGame.RightFlipped;
                            
                            
                            SizeOfTable();
                            IconsToPlace();
                            List<int> cardPositions = loadedGame.CardPositions;
                            
                            for (int i = 0; i < cardPositions.Count; i++)
                            {
                                Label label = tableLayoutPanel1.Controls[i] as Label;
                                if (label != null)
                                {
                                    label.Tag = cardPositions[i]; 
                                    label.Image = backImage; 
                                    label.Enabled = true; 
                                }
                            }

                            
                            foreach (int flippedIndex in rightFlipped)
                            {
                                Label label = tableLayoutPanel1.Controls[flippedIndex] as Label;
                                if (label != null)
                                {
                                    int imageId = (int)label.Tag;  
                                    label.Image = cardImages[imageId];  
                                }
                            }
                            if (loadedGame.IndexFirstFlipped != null)
                            {
                                first = (Label)tableLayoutPanel1.Controls[loadedGame.IndexFirstFlipped];
                                first.Image= cardImages[(int)first.Tag];
                            }
                            ShowScore();
                            MessageBox.Show("Hra byla úspěšně načtena.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Došlo k chybě při načítání hry: " + ex.Message);
                    }
                }
            }
        }


    }
}


