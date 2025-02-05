using System;
using Structs;
using UnityEngine;

namespace Stateful.Managers
{
    public class HouseLevelUpManager : MonoBehaviour
    {
        public GameObject levelUpcompleteUI;
        public GardenManager gardenManager;

        public void Update()
        {
            if (GameStateManager.CurrentState.LevelUpTime == DateTime.MinValue) return;
            if (GameStateManager.CurrentState.LevelUpTime > DateTime.Now) return;
            
            GameStateManager.CurrentState.LevelUpTime = DateTime.MinValue;
            
            for (int i = 0; i < HouseUpgrades.NewSpotsPerLevel[GameStateManager.CurrentState.HouseLevel]; i++)
            {
                GameStateManager.CurrentState.GardenSpots.Add(new SerializableGardenSpot
                {
                    state = GrowState.Vacant,
                    X = 1.15f * (i + GameStateManager.CurrentState.GardenSpots.Count),
                    Y = 0,
                    Z = 0.3f
                });
            }
            
            gardenManager.LoadData(GameStateManager.CurrentState);
            
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