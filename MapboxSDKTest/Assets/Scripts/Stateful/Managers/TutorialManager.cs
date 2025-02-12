using UI;
using UnityEngine;

namespace Stateful.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public TutorialUI introTutorial;
        public TutorialUI layoutTutorial;

        public void Update()
        {
            if (GameStateManager.CurrentState.HouseLevel == 1 && GameStateManager.CurrentState.IntroTutorial == false)
            {
                introTutorial.showTutorial = true;
                GameStateManager.CurrentState.IntroTutorial = true;
            }
            
            if (GameStateManager.CurrentState.HouseLevel == 2 && GameStateManager.CurrentState.LayoutTutorial == false)
            {
                layoutTutorial.showTutorial = true;
                GameStateManager.CurrentState.LayoutTutorial = true;
            }
        }
    }
}