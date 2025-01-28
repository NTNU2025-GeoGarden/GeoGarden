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
<<<<<<< HEAD
        
        public void HandleSell(){
            ShopUI.OnPlayerSoldItem();
            Debug.Log("Item sold, but shop UI remains open.");
=======

        public void HandleTryRestartMap()
        {
            SceneManager.LoadScene("Home");
            SceneManager.LoadScene("Map");
>>>>>>> 9bb5a164220aaa268fa102ba5c58113f4662bb27
        }
    }
}
