using System;
using Structs;
using UnityEngine;

namespace Stateful.Managers
{
    public class HouseLevelUpManager : MonoBehaviour
    {
        public GameObject levelUpcompleteUI;

        public void Update()
        {
            if (GameStateManager.CurrentState.LevelUpTime == DateTime.MinValue) return;
            if (GameStateManager.CurrentState.LevelUpTime > DateTime.Now) return;
            
            GameStateManager.CurrentState.LevelUpTime = DateTime.MinValue;
            GameStateManager.CurrentState.HouseLevel++;
            GameStateManager.CurrentState.CoinCap =
                HouseUpgrades.CoinCapPerLevel[GameStateManager.CurrentState.HouseLevel];
            GameStateManager.CurrentState.EnergyCap =
                HouseUpgrades.EnergyCapPerLevel[GameStateManager.CurrentState.HouseLevel];
            GameStateManager.CurrentState.WaterCap =
                HouseUpgrades.WaterCapPerLevel[GameStateManager.CurrentState.HouseLevel];
            
            levelUpcompleteUI.SetActive(true);
        }
    }
}