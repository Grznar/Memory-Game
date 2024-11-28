using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static MainMenu.NewGame;
namespace MainMenu
{
    public partial class NewGame : Form
    {
        Label first, second;
        public int[] score;
        private int playerNumber;
        private int playerCurrent = 1;
        private int cardNumber;
        private bool pcPlayer;
        private int difficulty;
        private bool playerRound;
        private string[] names;
        private bool isSound;

        private List<Label> flippedLabels = new List<Label>();
        private List<Image> cardImages = new List<Image>();
        private Image backImage;
        public List<int> cardImagesIds = new List<int>();
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
            for (int i = 0; i < (cardNumber * cardNumber) / 2; i++)
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
            if (firstLabel == null)
            {
                int indexF = rnd.Next(hiddenLabels.Count);
                firstLabel = hiddenLabels[indexF];
                hiddenLabels.Remove(firstLabel);
                flippedLabels.Add(firstLabel);
                if (chance)
                {
                    foreach (Label label in flippedLabels)
                    {
                        if ((int)label.Tag == (int)firstLabel.Tag && firstLabel != label)
                        {
                            secondLabel = label;

                        }
                    }
                }

                if (secondLabel == null)
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

            if ((int)firstLabel.Tag == (int)secondLabel.Tag)
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
            public List<bool> LabelVisibility { get; set; }
           
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
                        GameSave gameSave = new GameSave
                        {
                            PlayerNumber = this.playerNumber,
                            CardNumber = this.cardNumber,
                            PCPlayer = this.pcPlayer,
                            Difficulty = this.difficulty,
                            IsSound = this.isSound,
                            Score = this.score,
                            HiddenLabelsIndex = this.hiddenLabels.Select(h => (int)h.Tag).ToList(),
                            FlippedLabelsIndex = this.flippedLabels.Select(f => (int)f.Tag).ToList(),
                            CardImagesIds = this.cardImagesIds,
                            CardImagePaths = this.cardImages.Select(img => img.ToString()).ToList(),
                            LabelVisibility = new List<bool>()



                        };
                        foreach (Label label in tableLayoutPanel1.Controls)
                        {
                            if ((int)label.Tag == backImageId)
                            {
                                gameSave.LabelVisibility.Add(false);
                            }
                            else gameSave.LabelVisibility.Add(true);
                        }

                        using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(fs, gameSave);
                        }

                        MessageBox.Show("Hra byla uložena");
                    }
                    catch (Exception ex) { MessageBox.Show("Špatně uložené detaily hry!!!"); }
                    
                }
            }
        }
    }

}
