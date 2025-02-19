using System;
using Stateful;
using UnityEngine;

namespace UI
{
    public class DailiesHideOnTutorial : MonoBehaviour
    {
        public GameObject holder;

        public TutorialUI tutorial;

        private bool _shouldShow;
        private bool _fixTutorial;

        public void Start()
        {
            if (GameStateManager.CurrentState.LastLogin.Date >= DateTime.Today.Date)
            {
                gameObject.SetActive(false);
                return;
            }
            
            GameStateManager.CurrentState.DaysLoggedIn++;
            GameStateManager.CurrentState.LastLogin = DateTime.Today.Date;
            
            _shouldShow = true;
        }

        public void Update()
        {
            if (!holder.activeSelf && !tutorial.showTutorial && _shouldShow)
                holder.SetActive(true);

            if (tutorial.showTutorial && !_fixTutorial)
            {
                gameObject.SetActive(false);

                if (_shouldShow)
                {
                    GameStateManager.CurrentState.LastLogin = DateTime.Today - TimeSpan.FromDays(1);
                    GameStateManager.CurrentState.DaysLoggedIn--;
                }
                
                _fixTutorial = true;
            }
        }
    }
}
