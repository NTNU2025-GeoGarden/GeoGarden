using System.Collections.Generic;
using Stateful;
using UnityEngine;

namespace UI
{
    public class PlaceObjectUnlockCondition : MonoBehaviour
    {
        public int houseLevelRequired;
        public GameObject overlay;
        
        public List<PlaceObjectUI> placeObjects;

        public void Update()
        {
            bool state = GameStateManager.CurrentState.HouseLevel < houseLevelRequired;
            overlay.SetActive(state);

            if (placeObjects == null) return;
            
            foreach (PlaceObjectUI placeObject in placeObjects)
            {
                placeObject.feeTextHolder.SetActive(!state);
            }
        }
    }
}