using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject inventoryCanvas;
    
    public void HandleMapButtonClick()
    {
        SceneManager.LoadSceneAsync("Map");
    }

    public void HandleHomeButtonClick()
    {
        SceneManager.LoadSceneAsync("Home");
    }

    public void HandleInventoryButton()
    {
        inventoryCanvas.SetActive(true);
    }

    public void HandleInventoryCloseButton()
    {
        inventoryCanvas.SetActive(false);
    }
}
