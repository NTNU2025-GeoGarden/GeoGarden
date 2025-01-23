using Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIButtons : MonoBehaviour
    {
        public GameObject UICanvas;
    
        public void HandleMapButtonClick()
        {
            SceneManager.LoadSceneAsync("Map");
        }

        public void HandleHomeButtonClick()
        {
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
          
        public void HandleGatherResource()
        {
            PlayerMovement.OnCollectResource();
        }
    }
}
