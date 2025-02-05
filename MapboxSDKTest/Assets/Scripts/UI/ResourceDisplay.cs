using System;
using Stateful;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourceDisplay : MonoBehaviour
    {
        public enum ShowResourceType
        {
            Coins,
            Energy,
            Water
        };

        public ShowResourceType typeToDisplay;
        private TMP_Text _text;
    
        public void Start()
        {
            _text = GetComponent<TMP_Text>();
        }

        public void Update()
        {
            _text.text = typeToDisplay switch
            {
                ShowResourceType.Coins  => GameStateManager.CurrentState.Coins.ToString(),
                ShowResourceType.Energy => GameStateManager.CurrentState.Energy.ToString(),
                ShowResourceType.Water  => GameStateManager.CurrentState.Water.ToString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
