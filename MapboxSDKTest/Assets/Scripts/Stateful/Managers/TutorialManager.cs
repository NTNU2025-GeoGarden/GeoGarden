using UI;
using UnityEngine;

namespace Stateful.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public TutorialUI introTutorial;

        public void Update()
        {
            if (GameStateManager.CurrentState.HouseLevel == 1 && GameStateManager.CurrentState.IntroTutorial == false)
            {
                introTutorial.showTutorial = true;
                GameStateManager.CurrentState.IntroTutorial = true;
            }
        }
    }
}