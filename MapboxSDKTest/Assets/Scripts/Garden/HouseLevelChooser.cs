using Stateful;
using UnityEngine;

namespace Garden
{
    public class HouseLevelChooser : MonoBehaviour
    {
        public GameObject level1;
        public GameObject level2;
        public GameObject level3;
        public GameObject level4;
        public GameObject level5;
        private int _lastHouseLevel;

        public void Update()
        {
            if (GameStateManager.CurrentState.HouseLevel != _lastHouseLevel)
            {
                level1.SetActive(false);
                level2.SetActive(false);
                level3.SetActive(false);
                level4.SetActive(false);
                level5.SetActive(false);
                
                switch (GameStateManager.CurrentState.HouseLevel)
                {
                    case 1:
                        level1.SetActive(true);
                        break;
                    case 2:
                        level2.SetActive(true);
                        break;
                    case 3:
                        level3.SetActive(true);
                        break;
                    case 4:
                        level4.SetActive(true);
                        break;
                    case 5:
                        level5.SetActive(true);
                        break;
                }  
            }
            _lastHouseLevel = GameStateManager.CurrentState.HouseLevel;
        }
    }
}
