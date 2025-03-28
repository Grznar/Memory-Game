﻿using System;
using System.Collections.Generic;
using static MainMenu.GameLogic;

namespace MainMenu
{
    [Serializable]
    public class GameSave
    {
        public int PlayerNumber { get; set; }
        public int CardNumber { get; set; }
        public bool PCPlayer { get; set; }
        public int Difficulty { get; set; }
        public bool IsSound { get; set; }
        public List<int> CardImagesIds { get; set; }
        public int[] Score { get; set; }       
        public List<int> CardPositions { get; set; }
        public int IndexFirstFlipped { get; set; }
        public int IndexSecondFlipped { get; set; }
        public Dictionary<int, int> MatchedPairs { get; set; }
        public string[] Names { get; set; }
        public int CurrentPlayerOnTurn { get; set; }
        
        public List<int>FlippedLabels { get; set; }
        public GameState GameStateSave { get; set; }
        public List<int> HiddenLabels { get; set; }

    }
}
