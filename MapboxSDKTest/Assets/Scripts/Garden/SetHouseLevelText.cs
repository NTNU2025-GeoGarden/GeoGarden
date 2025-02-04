using Stateful;
using TMPro;
using UnityEngine;

namespace Garden
{
    public class SetHouseLevelText : MonoBehaviour
    {
        public TMP_Text text;
    
        // Update is called once per frame
        void Update()
        {
            text.text = $"Level {GameStateManager.CurrentState.HouseLevel}";
        }
    }
}
