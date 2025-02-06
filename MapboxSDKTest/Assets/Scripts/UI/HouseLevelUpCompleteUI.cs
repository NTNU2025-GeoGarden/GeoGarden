using Stateful;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HouseLevelUpCompleteUI : MonoBehaviour
    {
        public TMP_Text levelAText;
        public TMP_Text levelBText;

        public GameObject level2Rewards;
        public GameObject level3Rewards;
        public GameObject level4Rewards;
        public GameObject level5Rewards;
        
        void Update()
        {
            levelAText.text = $"Level {GameStateManager.CurrentState.HouseLevel - 1}";
            levelBText.text = $"Level {GameStateManager.CurrentState.HouseLevel}";

            switch (GameStateManager.CurrentState.HouseLevel)
            {
                case 2:
                    level2Rewards.SetActive(true);
                    break;
                case 3:
                    level3Rewards.SetActive(true);
                    break;
                case 4:
                    level4Rewards.SetActive(true);
                    break;
                case 5:
                    level5Rewards.SetActive(true);
                    break;
            }
        }
    }
}
