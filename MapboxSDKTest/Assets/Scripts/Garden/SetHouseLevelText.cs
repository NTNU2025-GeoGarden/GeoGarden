using System;
using System.Globalization;
using Stateful;
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
            text.text = 
                GameStateManager.CurrentState.LevelUpTime == DateTime.MinValue ? 
                    $"Level {GameStateManager.CurrentState.HouseLevel}" 
                : (GameStateManager.CurrentState.LevelUpTime - DateTime.Now).ToString(@"d\d\ hh\:mm\:ss", CultureInfo.InvariantCulture);
        }
    }
}
