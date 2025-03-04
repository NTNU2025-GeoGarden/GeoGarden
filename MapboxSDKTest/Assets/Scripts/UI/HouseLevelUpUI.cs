using System;
using System.Globalization;
using Garden;
using Stateful;
using Stateful.Managers;
using Structs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HouseLevelUpUI : MonoBehaviour, IUsingGameState
    {
        public delegate void DelegateHouseLevelUp();
        public static DelegateHouseLevelUp OnHouseLevelUp;
        
        public TMP_Text titleText;
        public TMP_Text coinCapText;
        public TMP_Text energyCapText;
        public TMP_Text waterCapText;
        public TMP_Text upgradeTimeText; 

        public RectTransform progressBarHolder;
        public RectTransform coinCapBaseProgressBar;
        public RectTransform coinCapExtensionProgressBar;
        public RectTransform energyCapBaseProgressBar;
        public RectTransform energyCapExtensionProgressBar;
        public RectTransform waterCapBaseProgressBar;
        public RectTransform waterCapExtensionProgressBar;

        public TMP_Text upgradeText;

        public TMP_Text plantsRequirementText;
        public Image plantsRequirementImage;
        public TMP_Text walkingRequirementText;
        public Image walkingRequirementImage;

        public Button upgradeButton;
        public TMP_Text upgradeCost;

        public GameObject houseReadytoUpgradeIcon;
        public GameObject houseUI;
        
        private GameState _state;
        private bool _requirementsMet;
        private bool _canUpgrade;

        public void Start()
        {
            OnHouseLevelUp += StartHouseLevelUp;
        }

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
            titleText.text = $"Upgrade to level {_state.HouseLevel + 1}?";

            coinCapText.text   = $"Capacity {_state.CoinCap}+{HouseUpgrades.CoinCapPerLevel[_state.HouseLevel + 1] - _state.CoinCap}";
            energyCapText.text = $"Capacity {_state.EnergyCap}+{HouseUpgrades.EnergyCapPerLevel[_state.HouseLevel + 1] - _state.EnergyCap}";
            waterCapText.text  = $"Capacity {_state.WaterCap}+{HouseUpgrades.WaterCapPerLevel[_state.HouseLevel + 1] - _state.WaterCap}";

            upgradeText.text = HouseUpgrades.UpgradeTextPerLevel[_state.HouseLevel];

            plantsRequirementText.text =
                $"{_state.PlantsHarvested}/{HouseUpgrades.PlantRequirementPerLevel[_state.HouseLevel]}";
            
            walkingRequirementText.text =
                $"{_state.DistanceWalked}/{HouseUpgrades.WalkingRequirementPerLevel[_state.HouseLevel]}km";

            upgradeCost.text = HouseUpgrades.UpgradeCost[_state.HouseLevel].ToString();
            upgradeTimeText.text = HouseUpgrades.UpgradeTimePerLevel[_state.HouseLevel].ToString(@"d\dh\h", CultureInfo.InvariantCulture);
            
            plantsRequirementImage.color = _state.PlantsHarvested >= HouseUpgrades.PlantRequirementPerLevel[_state.HouseLevel] ? new Color(0.690f, 0.972f, 0.741f) : new Color(0.971f, 0.694f, 0.692f);
            walkingRequirementImage.color = _state.DistanceWalked >= HouseUpgrades.WalkingRequirementPerLevel[_state.HouseLevel] ? new Color(0.690f, 0.972f, 0.741f) : new Color(0.971f, 0.694f, 0.692f);
            
            upgradeButton.interactable = _canUpgrade;
            upgradeCost.color = _canUpgrade ? new Color(0.690f, 0.972f, 0.741f) : new Color(0.971f, 0.694f, 0.692f);
            
            SetProgressBar(coinCapBaseProgressBar, (float)_state.CoinCap / HouseUpgrades.MaxCoinCap);
            SetProgressBar(coinCapExtensionProgressBar, (float)HouseUpgrades.CoinCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxCoinCap);
            
            SetProgressBar(energyCapBaseProgressBar, (float)_state.EnergyCap / HouseUpgrades.MaxEnergyCap);
            SetProgressBar(energyCapExtensionProgressBar, (float)HouseUpgrades.EnergyCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxEnergyCap);
            
            SetProgressBar(waterCapBaseProgressBar, (float)_state.WaterCap / HouseUpgrades.MaxWaterCap);
            SetProgressBar(waterCapExtensionProgressBar, (float)HouseUpgrades.WaterCapPerLevel[_state.HouseLevel + 1] / HouseUpgrades.MaxWaterCap);
        }

        public void Update()
        {
            LoadData(GameStateManager.CurrentState);
            
            _requirementsMet = _state.PlantsHarvested >= HouseUpgrades.PlantRequirementPerLevel[_state.HouseLevel]
                               && _state.DistanceWalked  >= HouseUpgrades.WalkingRequirementPerLevel[_state.HouseLevel];
            
            _canUpgrade = _requirementsMet && _state.Coins >= HouseUpgrades.UpgradeCost[_state.HouseLevel];
            
            houseReadytoUpgradeIcon.SetActive(_requirementsMet && _state.LevelUpTime == DateTime.MinValue);
        }
        
        public void LoadData(GameState state)
        {
            _state = state;
            if(_state.HouseLevel != HouseUpgrades.MaxLevel)
                SetUIData();
        }

        public void SaveData(ref GameState state)
        {
        }

        private void StartHouseLevelUp()
        {
            int costOfUpgrade = HouseUpgrades.UpgradeCost[_state.HouseLevel];
            GameStateManager.CurrentState.Coins -= costOfUpgrade;
            FirebaseManager.TelemetryRecordCoinsUsed(costOfUpgrade);
            
            GameStateManager.CurrentState.LevelUpTime =
                DateTime.Now.Add(HouseUpgrades.UpgradeTimePerLevel[_state.HouseLevel]);
            
            LoadData(GameStateManager.CurrentState);
        }
    }
}
