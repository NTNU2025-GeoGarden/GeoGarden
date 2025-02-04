using System;
using Garden;
using Stateful;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class HouseUpgradeUI : MonoBehaviour, IUsingGameState
    {
        public TMP_Text titleText;
        public TMP_Text coinCapText;
        public TMP_Text energyCapText;
        public TMP_Text waterCapText;

        public RectTransform progressBarHolder;
        public RectTransform coinCapBaseProgressBar;
        public RectTransform coinCapExtensionProgressBar;
        
        public RectTransform energyCapBaseProgressBar;
        public RectTransform energyCapExtensionProgressBar;
        
        public RectTransform waterCapBaseProgressBar;
        public RectTransform waterCapExtensionProgressBar;
        
        private GameState _state;

        private float GetRectWidth(float fraction)
        {
            return fraction * progressBarHolder.sizeDelta.x;
        }

        private void SetProgressBar(RectTransform rect, float progress)
        {
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, GetRectWidth(progress));
        }
        
        private void SetUIData()
        {
            Debug.Log("HHHH");
            titleText.text = $"Upgrade to level {_state.HouseLevel + 1}?";

            coinCapText.text   = $"Capacity {_state.CoinCap}+{HouseUpgrades.CoinCapPerLevel[_state.HouseLevel + 1]}";
            energyCapText.text = $"Capacity {_state.EnergyCap}+{HouseUpgrades.EnergyCapPerLevel[_state.HouseLevel + 1]}";
            waterCapText.text  = $"Capacity {_state.WaterCap}+{HouseUpgrades.WaterCapPerLevel[_state.HouseLevel + 1]}";

            SetProgressBar(coinCapBaseProgressBar, (float)_state.CoinCap / HouseUpgrades.MaxCoinCap);
            SetProgressBar(coinCapExtensionProgressBar, (float)HouseUpgrades.CoinCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxCoinCap);
            
            SetProgressBar(energyCapBaseProgressBar, (float)_state.EnergyCap / HouseUpgrades.MaxEnergyCap);
            SetProgressBar(energyCapExtensionProgressBar, (float)HouseUpgrades.EnergyCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxEnergyCap);
            
            SetProgressBar(waterCapBaseProgressBar, (float)_state.WaterCap / HouseUpgrades.MaxWaterCap);
            SetProgressBar(waterCapExtensionProgressBar, (float)HouseUpgrades.WaterCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxWaterCap);
        }

        public void OnEnable()
        {
            if(_state != null)
                SetUIData();
        }

        public void LoadData(GameState state)
        {
            _state = state;
            SetUIData();
        }

        public void SaveData(ref GameState state)
        {
        }
    }
}
