using MainMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MainMenu.GameLogic;

namespace MainMenu
{
    public class GameLogic
    {
        private GameBoard gameBoard;
        private GameScoreManager scoreManager;
        private SoundManager soundManager;
        public Label first, second;
        

        private bool locked = false;
        private bool playerRound = true;

        public int currentPlayer = 1;
        private int playerCount;
        private int cardCount;
        private bool pcPlayer;
        private int difficulty;
        private bool isSound;
        private bool isLoading;
        private int backImageId = -1;

        private Random rnd = new Random();

        public event Action GameEnded;
        public event Action ScoreUpdated;
        

        private Dictionary<int, bool> alreadyFlipped;
        public List<int> flippedLabels = new List<int>();


        public GameState gameState = GameState.Idle;

        
        public GameLogic(GameBoard board,GameScoreManager scoreMgr,int playerCount,
            int cardCount,bool pcPlayer,int difficulty,bool isSound,bool isLoading = false)
        {
            this.gameBoard = board;
            this.scoreManager = scoreMgr;
            this.playerCount = playerCount;
            this.cardCount = cardCount;
            this.pcPlayer = pcPlayer;
            this.difficulty = difficulty;
            this.isSound = isSound;
            this.isLoading = isLoading;
            
            alreadyFlipped = new Dictionary<int, bool>();
            for (int i = 0; i < gameBoard.tableLayoutPanel.Controls.Count; i++)
            {
                alreadyFlipped.Add(i, false);
            }

            soundManager=new SoundManager(isSound);
        }
        public int CurrentPlayer => currentPlayer;
        public enum GameState
        {
            Idle,           
            OneCardFlipped, 
            Processing,
            ProcessingForComputer
        }


        public async Task OnCardClicked(Label clickedLabel)
        {
            
            if (gameState == GameState.Processing || gameState==GameState.ProcessingForComputer)
                return;

            if (locked) return;

            if (first != null && second != null) return;

            if (clickedLabel == null) return;
            if ((int)clickedLabel.Tag == backImageId) return;


            if (gameState == GameState.OneCardFlipped && clickedLabel == first)
                return;




            if (gameState == GameState.Idle)
            {
                locked = true;
                gameState = GameState.OneCardFlipped;
                first = clickedLabel;

                
                gameBoard.FlipCardFront(first);
                soundManager.PlayFlipCardSound();
                
                int idxF = gameBoard.tableLayoutPanel.Controls.IndexOf(first);
                if (alreadyFlipped[idxF] == false)
                {
                    alreadyFlipped[idxF] = true;
                    flippedLabels.Add((int)first.Tag);
                    Console.WriteLine(gameBoard.tableLayoutPanel.Controls.IndexOf(first) + "Přidán to flipped");
                }
                gameBoard.HiddenLabels.Remove((int)first.Tag);
                await Task.Delay(1000);

                locked = false;
                return;
            }


            if (gameState == GameState.OneCardFlipped)
            {
                locked = true;
                
                second = clickedLabel;
                
                
                
                gameBoard.FlipCardFront(second);
                
                soundManager.PlayFlipCardSound();
                await Task.Delay(1000);
                int idxS = gameBoard.tableLayoutPanel.Controls.IndexOf(second);
                if (alreadyFlipped[idxS]==false)
                {
                    alreadyFlipped[idxS] = true;
                    flippedLabels.Add((int)second.Tag);
                    Console.WriteLine(gameBoard.tableLayoutPanel.Controls.IndexOf(second) + "Přidán to flipped");
                }
                gameBoard.HiddenLabels.Remove((int)second.Tag);

                gameState = GameState.Processing;
            }


            int tag1 = (int)first.Tag;
            int tag2 = (int)second.Tag;
            if (tag1 * 100 == tag2 || tag2 * 100 == tag1)
            {
                soundManager.PlayMatchedCorrect();
                scoreManager.AddScore(currentPlayer - 1, 1);


                flippedLabels.RemoveAll(u => u == (int)first.Tag);
                flippedLabels.RemoveAll(u => u == (int)second.Tag);
                Console.WriteLine((int)first.Tag + "Odebrán z flipped");
                Console.WriteLine((int)second.Tag + "Odebrán z flipped");

                int idxFirst = gameBoard.tableLayoutPanel.Controls.IndexOf(first);
                int idxSecond = gameBoard.tableLayoutPanel.Controls.IndexOf(second);
                Console.WriteLine(idxFirst + "Odebrán z flipped");
                Console.WriteLine(idxSecond + "Odebrán z flipped");
                gameBoard.MatchedPairs[idxFirst] = (int)first.Tag;
                gameBoard.MatchedPairs[idxSecond] = (int)second.Tag;

                
                first.Tag = backImageId;
                second.Tag = backImageId;
                first = null;
                second = null;

                locked = false;
                ScoreUpdated?.Invoke();

                WinnerCheck();
                gameState = GameState.Idle; 

            }
            else
            {
                soundManager.PlayMatchedWrong();
                await Task.Delay(1000);
                currentPlayer = (currentPlayer % playerCount) + 1;
                ScoreUpdated?.Invoke();
                OnTimer1Tick();
                

               
            }
        }
        public void OnTimer1Tick()
        {
            
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
            locked = false;

            gameState = GameState.Idle;
            if (pcPlayer && currentPlayer == 2)
            {
                ComputerTurn(); 
            }
        }
        public async void ComputerTurn()
        {
            


            if(gameBoard.HiddenLabels.Count + flippedLabels.Count <= 0)
            {
                return;
            }
            if (gameState == GameState.Processing)
                return;

            gameState = GameState.Processing;
            locked = true;
            
            await Task.Delay(1000);

            bool chance = (GetRight() >= rnd.Next(100));
            int indexFirstLabel = -1;
            int indexSecondLabel = -1;

            
            if (chance)
            {
                Console.WriteLine(chance);
                bool found = false;
                for (int i = 0; i < flippedLabels.Count && !found; i++)
                {
                    for (int j = i + 1; j < flippedLabels.Count && !found; j++)
                    {
                        if (flippedLabels[i] * 100 == flippedLabels[j] || flippedLabels[j] * 100 == flippedLabels[i])
                        {
                            indexFirstLabel = flippedLabels[i];
                            indexSecondLabel = flippedLabels[j];
                            
                            found = true;
                            
                        }
                    }
                }
            }
            bool pickFirstFromHidden = false;
            bool pickSecondFromHidden = false;
            if (indexFirstLabel == -1)
            {
                 
                if (gameBoard.HiddenLabels.Count > 0)
                {
                    int r = rnd.Next(gameBoard.HiddenLabels.Count);
                    indexFirstLabel = gameBoard.HiddenLabels[r];
                   
                    gameBoard.HiddenLabels.RemoveAt(r);
                    pickFirstFromHidden = true;
                    
                   
                }
                else
                {
                    int r = rnd.Next(flippedLabels.Count);
                    indexFirstLabel = flippedLabels[r];

                    
                }

                bool foundSecond = false;
                if (chance && indexSecondLabel == -1)
                {
                    for (int k = 0; k < flippedLabels.Count && !foundSecond; k++)
                    {
                        if (flippedLabels[k] * 100 == indexFirstLabel || indexFirstLabel * 100 == flippedLabels[k])
                        {
                            indexSecondLabel = flippedLabels[k];
                            foundSecond = true;
                            
                        }
                    }
                }
                if (indexSecondLabel == -1)
                {
                    if (gameBoard.HiddenLabels.Count >= 1)
                    {
                        int r2 = rnd.Next(gameBoard.HiddenLabels.Count);
                        indexSecondLabel = gameBoard.HiddenLabels[r2];
                        gameBoard.HiddenLabels.RemoveAt(r2);
                        flippedLabels.Add(indexSecondLabel);
                        
                        pickSecondFromHidden = true;
                        
                    }
                    else
                    {
                        int r2 = rnd.Next(flippedLabels.Count);
                        indexSecondLabel = flippedLabels[r2];
                        
                    }
                }
                if (pickFirstFromHidden) flippedLabels.Add(indexFirstLabel); Console.WriteLine((int)indexFirstLabel + "Přidán do flipped");
            }

            await Task.Delay(1000);
            Label firstLabel = null;
            Label secondLabel = null;
            foreach (Label label in gameBoard.tableLayoutPanel.Controls)
            {
                if((int)label.Tag== indexFirstLabel&& firstLabel==null)
                {
                   
                    
                    firstLabel = label;
                    
                    
                }
                else if((int)label.Tag==indexSecondLabel && secondLabel==null)
                    {
                    
                        secondLabel = label;
                    
                }
            }

            if(pickFirstFromHidden)
            {
                int index = gameBoard.tableLayoutPanel.Controls.IndexOf(firstLabel);
                Console.WriteLine(index + "pridan do flipped");
                alreadyFlipped[index] = true;
            }
            if (pickSecondFromHidden)
            {
                int index = gameBoard.tableLayoutPanel.Controls.IndexOf(secondLabel);
                Console.WriteLine(index + "pridan do flipped");
                alreadyFlipped[index] = true;
            }


            if (firstLabel == null || secondLabel == null)
            {
                locked = false;
                return;
            }

            gameBoard.FlipCardFront(firstLabel);
            soundManager.PlayFlipCardSound();
            await Task.Delay(1000);

            gameBoard.FlipCardFront(secondLabel);
            soundManager.PlayFlipCardSound();
            await Task.Delay(1000);

            if (indexFirstLabel * 100 == indexSecondLabel || indexSecondLabel * 100 == indexFirstLabel)
            {
                soundManager.PlayMatchedCorrect();
                flippedLabels.RemoveAll(u => u == indexFirstLabel);
                flippedLabels.RemoveAll(u => u == indexSecondLabel) ;


                scoreManager.AddScore(currentPlayer - 1, 1);

                int idxF = gameBoard.tableLayoutPanel.Controls.IndexOf(firstLabel);
                int idxS = gameBoard.tableLayoutPanel.Controls.IndexOf(secondLabel);
                gameBoard.MatchedPairs[idxF] = (int)firstLabel.Tag;
                gameBoard.MatchedPairs[idxS] = (int)secondLabel.Tag;
                ScoreUpdated();

                firstLabel.Tag = backImageId;
                secondLabel.Tag = backImageId;

                gameState = GameState.ProcessingForComputer;
                WinnerCheck();
                ComputerTurn();
            }
            else
            {
                soundManager.PlayMatchedWrong();
                gameBoard.FlipCardBack(firstLabel);
                gameBoard.FlipCardBack(secondLabel);
                

                currentPlayer = (currentPlayer % playerCount) + 1;
                ScoreUpdated();
                locked = false;

                gameState = GameState.Idle;
                
            }
        }
        private int GetRight()
        {
            switch (difficulty)
            {
                case 1: return 30;
                case 2: return 60;
                case 3: return 100;
                default: return 60;
            }
        }
        private void WinnerCheck()
        {
            

            int totalCards = cardCount * cardCount;
            int sum = gameBoard.HiddenLabels.Count + flippedLabels.Count;
            if (sum == 0)
            {
                
                scoreManager.EndScore();
                soundManager.Dispose();
                GameEnded?.Invoke();
            }
        }
        

        
    }
    
}
