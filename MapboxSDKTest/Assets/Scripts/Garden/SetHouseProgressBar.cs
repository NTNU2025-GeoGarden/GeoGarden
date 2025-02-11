using Stateful;
using Structs;
using UnityEngine;

namespace Garden
{
    public class SetProgressBar : MonoBehaviour
    {
        public Vector3 leftMostPosition = new(0.5f, 0, 0);

        private GameState _state;
        private float _progress;
        
        void Start()
        {
            transform.localPosition = leftMostPosition;
            transform.localScale = new Vector3(_progress, 1, 1);
        }
        
        void Update()
        {
            _state = GameStateManager.CurrentState;

            _progress = Mathf.Clamp01(
                  Mathf.Clamp01((float)_state.PlantsHarvested / HouseUpgrades.PlantRequirementPerLevel[_state.HouseLevel]) * 0.5f
                + Mathf.Clamp01(_state.DistanceWalked / HouseUpgrades.WalkingRequirementPerLevel[_state.HouseLevel]) * 0.5f
                );
            
            transform.localPosition = leftMostPosition - new Vector3(_progress * 0.5f, 0, 0);
            transform.localScale = new Vector3(_progress, 1.05f, 1);
        }
    }
}
