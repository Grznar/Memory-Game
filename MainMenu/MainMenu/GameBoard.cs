using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MainMenu
{
    public class GameBoard
    {
        public TableLayoutPanel tableLayoutPanel;
        public List<int> hiddenLabels;
        public List<int> HiddenLabels { get; set; }

        private Image backImage;
        private List<Image> cardImages;
        private int backImageId = -1;
        public List<int> cardImagesIds = new List<int>();

        private int cardCount = 0;
        public Dictionary<int, int> MatchedPairs { get; set; }
        public event EventHandler CardClicked;
        public GameBoard(int cardsCount, TableLayoutPanel panel)
        {
            MatchedPairs = new Dictionary<int, int>();
            this.tableLayoutPanel = panel;
            this.cardCount = cardsCount;
        }
        public void InitializeBoard(bool isLoading, StatusStrip status, ToolStrip strip)
        {

            LoadImages();


            SetupLayout();


            PlaceCards(isLoading);

            tableLayoutPanel.Padding = new Padding(0, 0, 0, status.Height);
            tableLayoutPanel.Padding = new Padding(0, strip.Height, 0, 0);
            HiddenLabels = hiddenLabels;
        }
        public Image GetCardImage(int cardId)
        {
            int index;
            if (cardId >= 100)
                index = (cardId / 100) - 1;
            else
                index = cardId - 1;

            if (index >= 0 && index < cardImages.Count)
            {
                return cardImages[index];
            }
            return null;
        }


        public Image GetBackImage()
        {
            return backImage;
        }


        public void FlipCardFront(Label label)
        {
            if (label.Tag is int cardId)
            {
                label.Image = GetCardImage(cardId);     
            }
        }


        public void FlipCardBack(Label label)
        {
            label.Image = GetBackImage();
        }




        private void LoadImages()
        {
            cardImages = new List<Image>();
            backImageId = -1;


            Assembly assembly = Assembly.GetExecutingAssembly();


            string resourceFolder = "MainMenu.Root.Images";


            string[] resourceNames = assembly.GetManifestResourceNames();


            foreach (string resourceName in resourceNames)
            {
                if (resourceName.StartsWith(resourceFolder) && resourceName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {

                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream != null)
                        {

                            Image image = Image.FromStream(stream);


                            if (resourceName.EndsWith("BackImage.png", StringComparison.OrdinalIgnoreCase))
                            {
                                backImage = image;
                            }
                            else
                            {
                                cardImages.Add(image);
                                cardImagesIds.Add(cardImages.Count - 1);
                            }
                        }
                    }
                }
            }


            if (cardImages.Count < 18)
            {
                MessageBox.Show("Není dost obrázků!");
            }
        }



        private void SetupLayout()
        {

            int rows = cardCount;
            int columns = cardCount;
            tableLayoutPanel.RowCount = rows;
            tableLayoutPanel.ColumnCount = columns;

            tableLayoutPanel.ColumnStyles.Clear();
            tableLayoutPanel.RowStyles.Clear();
            tableLayoutPanel.Dock = DockStyle.Fill;
            tableLayoutPanel.Controls.Clear();

            for (int i = 0; i < columns; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
            }


            for (int i = 0; i < rows; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
            }


        }
        private void PlaceCards(bool isLoading)
        {
            hiddenLabels = new List<int>();


            if (isLoading)
            {

                for (int i = 0; i < cardCount * cardCount; i++)
                {

                    Label label = CreateCardLabel(tagValue: 0);
                    tableLayoutPanel.Controls.Add(label);
                    hiddenLabels.Add((int)label.Tag);
                }
            }
            else
            {

                List<int> icons = new List<int>();
                for (int i = 0; i < (cardCount * cardCount) / 2; i++)
                {
                    icons.Add(i + 1);
                    icons.Add((i + 1) * 100);
                }


                Random rnd = new Random();
                List<int> randIcons = new List<int>();
                while (icons.Count > 0)
                {
                    int randIndex = rnd.Next(icons.Count);
                    randIcons.Add(icons[randIndex]);
                    icons.RemoveAt(randIndex);
                }

                for (int i = 0; i < cardCount * cardCount; i++)
                {
                    Label label = CreateCardLabel(randIcons[i]);
                    tableLayoutPanel.Controls.Add(label);
                    hiddenLabels.Add(randIcons[i]);
                }
            }
        }


        private Label CreateCardLabel(int tagValue)
        {
            Label label = new Label
            {

                Image = backImage,
                Tag = tagValue,
                ImageAlign = ContentAlignment.MiddleCenter,
                AutoSize = false,
                Size = new Size(100, 100),
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Enabled = true,
            };


            label.Click += async (s, e) =>
            {

                CardClicked?.Invoke(s, e);
            };

            return label;
        }
    }
}
