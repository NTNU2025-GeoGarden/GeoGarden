using Stateful;
using UnityEngine;

namespace UI
{
    public class DailyChallenge : MonoBehaviour
    {
        public int coinReward;
        public int waterReward;
        public int itemIDReward = -1;
        public int dayNum;

        public bool available;
        public bool collected;

        public GameObject claimed;
        public GameObject check;
        public GameObject highlight;

        public void Start()
        {
            SetTheAbsoluteState();
        }

        public void Collect()
        {
            collected = true;
            available = false;
            GameStateManager.CurrentState.DaysClaimed[dayNum] = true;

            if (GameStateManager.CurrentState.Coins + coinReward < HouseUpgrades.CoinCapPerLevel[GameStateManager.CurrentState.HouseLevel])
                GameStateManager.CurrentState.Coins += coinReward;
            else
                GameStateManager.CurrentState.Coins = HouseUpgrades.CoinCapPerLevel[GameStateManager.CurrentState.HouseLevel];

            if (GameStateManager.CurrentState.Water + waterGained < HouseUpgrades.WaterCapPerLevel[GameStateManager.CurrentState.HouseLevel])
                GameStateManager.CurrentState.Water += waterGained;
            else
                GameStateManager.CurrentState.Water = HouseUpgrades.WaterCapPerLevel[GameStateManager.CurrentState.HouseLevel];


            if (itemIDReward != -1)
                GameStateManager.AddInventoryItem(new SerializableInventoryEntry { Amount = 1, Id = itemIDReward });

            SetTheAbsoluteState();
        }

        private void SetTheAbsoluteState()
        {
            switch (available)
            {
                case true when !collected:
                    highlight.SetActive(true);
                    break;
                case false when collected:
                    highlight.SetActive(false);
                    claimed.SetActive(true);
                    check.SetActive(true);
                    break;
            }
        }
    }
}