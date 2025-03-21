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
        public GameObject SecondaryUICanvas;
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

        public void HandlePlantSeed()
        {
            PlantSeedUI.OnPlayerPlantedSeed();
            UICanvas.SetActive(false);
            SecondaryUICanvas.SetActive(true);
        }
        
        public void HandleSell(){
            ShopUI.OnPlayerSoldItem();
        }
        
        public void HandleTryRestartMap()
        {
            SceneManager.LoadScene("Home");
            SceneManager.LoadScene("Map");
        }

        public void HandleEditMode()
        {
            UICanvas.SetActive(false);
            SecondaryUICanvas.SetActive(true);
        }

        public void HandleStartGame()
        {
            FirebaseManager.TelemetryRecordLogin();
            GameStateManager.OnForceSaveGame();
            SceneManager.LoadSceneAsync("Home");
        }

        public void HandleUpgradeHouse()
        {
            UICanvas.SetActive(false);
            HouseLevelUpUI.OnHouseLevelUp();
        }
    }
}
