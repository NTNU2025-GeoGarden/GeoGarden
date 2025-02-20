using System;
using System.Collections.Generic;
using Stateful;
using UnityEngine;

namespace UI
{
    public class DailiesUI : MonoBehaviour, IUsingGameState
    {
        public List<DailyChallenge> days;

        private bool _loaded;

        public void OnEnable()
        {
            LoadData(GameStateManager.CurrentState);
        }

        public void LoadData(GameState state)
        {
            // For every login bonus that we have
            for (int i = 0; i < days.Count; i++)
            {
                // If the login bonus we are looking it is one of the ones unlocked
                // by the user logging in
                if (i < GameStateManager.CurrentState.DaysLoggedIn)
                {
                    // The login bonus is available if the user has NOT claimed it yet
                    days[i].available = !GameStateManager.CurrentState.DaysClaimed[i];
                    
                    // It is already collected if the user has claimed it
                    days[i].collected = GameStateManager.CurrentState.DaysClaimed[i];
                }
                else
                {
                    // If it is one of the days not yet unlocked, then it is not available.
                    days[i].available = false;
                }
            }
        }

        public void SaveData(ref GameState state)
        {
            throw new NotImplementedException();
        }
    }
}
