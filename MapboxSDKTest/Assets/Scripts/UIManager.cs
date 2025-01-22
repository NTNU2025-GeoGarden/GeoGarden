using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
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
}
