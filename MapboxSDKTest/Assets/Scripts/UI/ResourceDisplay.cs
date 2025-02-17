using System;
using System.Globalization;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        public enum ShowResourceType
        {
            Coins,
            Energy,
            Water,
            Distance
        }
        
        public ShowResourceType typeToDisplay;
        public TMP_Text maxText;
        private TMP_Text _text;
    
        public void Start()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void Update()
        {
            _text.text = typeToDisplay switch
            {
                ShowResourceType.Coins    => GameStateManager.CurrentState.Coins.ToString(),
                ShowResourceType.Energy   => GameStateManager.CurrentState.Energy.ToString(),
                ShowResourceType.Water    => GameStateManager.CurrentState.Water.ToString(),
                ShowResourceType.Distance => $"{GameStateManager.CurrentState.DistanceWalked.ToString(CultureInfo.InvariantCulture)}km",
                _ => throw new ArgumentOutOfRangeException()
            };
            
            maxText.text = typeToDisplay switch
            {
                ShowResourceType.Coins    => $"/{GameStateManager.CurrentState.CoinCap.ToString()}",
                ShowResourceType.Energy   => $"/{GameStateManager.CurrentState.EnergyCap.ToString()}",
                ShowResourceType.Water    => $"/{GameStateManager.CurrentState.WaterCap.ToString()}",
                ShowResourceType.Distance => $"Goal: {HouseUpgrades.WalkingRequirementPerLevel[GameStateManager.CurrentState.HouseLevel].ToString()}km",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
