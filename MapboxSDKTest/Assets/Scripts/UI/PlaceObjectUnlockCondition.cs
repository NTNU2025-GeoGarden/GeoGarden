using Stateful;
using UnityEngine;

namespace UI
{
    public class PlaceObjectUnlockCondition : MonoBehaviour
    {
        public int houseLevelRequired;
        public GameObject obj;

        public void Update()
        { 
            obj.SetActive(GameStateManager.CurrentState.HouseLevel < houseLevelRequired);
        }
    }
}