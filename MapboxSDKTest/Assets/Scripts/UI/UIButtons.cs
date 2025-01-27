using Map;
using Stateful;
using Stateful.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIButtons : MonoBehaviour
    {
        public GameObject UICanvas;

        public bool openCloseState;
    
        public void HandleMapButtonClick()
        {
            GameStateManager.OnForceSaveGame();
            SceneManager.LoadSceneAsync("Map");
        }

        public void HandleHomeButtonClick()
        {
            GameStateManager.OnForceSaveGame();
            SceneManager.LoadSceneAsync("Home");
        }

        public void HandleUICanvasOpenButton()
        {
            UICanvas.SetActive(true);
        }

        public void HandleUICanvasCloseButton()
        {
            UICanvas.SetActive(false);
        }

        public void HandleUICanvasOpenCloseButton()
        {
            openCloseState = !openCloseState;
            UICanvas.SetActive(openCloseState);
        }
          
        public void HandleGatherResource()
        {
            PlayerMovement.OnCollectResource();
        }

        public void HandlePlantSeed()
        {
            PlantSeedUI.OnPlayerPlantedSeed();
            UICanvas.SetActive(false);
        }
    }
}
