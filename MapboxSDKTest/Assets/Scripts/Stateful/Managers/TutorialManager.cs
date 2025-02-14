using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stateful.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public TutorialUI introTutorial;
        public TutorialUI layoutTutorial;

        public void Update()
        {
            if (GameStateManager.CurrentState.HouseLevel == 1 && GameStateManager.CurrentState.GardenTutorial == false)
            {
                introTutorial.showTutorial = true;
                GameStateManager.CurrentState.GardenTutorial = true;
            }
            
            if (SceneManager.GetActiveScene().name == "Map" && GameStateManager.CurrentState.MapTutorial == false)
            {
                introTutorial.showTutorial = true;
                GameStateManager.CurrentState.MapTutorial = true;
            }
            
            /*if (GameStateManager.CurrentState.HouseLevel == 2 && GameStateManager.CurrentState.LayoutTutorial == false)
            {
                layoutTutorial.showTutorial = true;
                GameStateManager.CurrentState.LayoutTutorial = true;
            }*/
        }
    }
}