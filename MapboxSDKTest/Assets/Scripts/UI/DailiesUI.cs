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
            for (int i = 0; i < days.Count; i++)
            {
                if (i < GameStateManager.CurrentState.DaysLoggedIn)
                {
                    if (GameStateManager.CurrentState.DaysClaimed[i])
                    {
                        days[i].collected = true;
                        days[i].available = false;
                    }
                    else
                    {
                        days[i].available = true;
                    }
                }
                else
                {
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
