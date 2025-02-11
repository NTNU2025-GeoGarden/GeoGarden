using System;
using System.Globalization;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class SetHouseLevelText : MonoBehaviour
    {
        public TMP_Text text;
    
        // Update is called once per frame
        public void Update()
        {
            if (GameStateManager.CurrentState.LevelUpTime == DateTime.MinValue)
                text.text = GameStateManager.CurrentState.HouseLevel == HouseUpgrades.MaxLevel? 
                    "Max level" 
                    : $"Level {GameStateManager.CurrentState.HouseLevel}";
            else
                text.text = (GameStateManager.CurrentState.LevelUpTime - DateTime.Now).ToString(@"d\d\ hh\:mm\:ss",
                    CultureInfo.InvariantCulture);
        }
    }
}
