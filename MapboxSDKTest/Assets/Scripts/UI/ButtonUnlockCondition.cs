using Stateful;
using UnityEngine;

namespace UI
{
    public class ButtonUnlockCondition : MonoBehaviour
    {
        public int houseLevelRequired;
        public GameObject button;

        public void Update()
        { 
            button.SetActive(GameStateManager.CurrentState.HouseLevel >= houseLevelRequired);
        }
    }
}