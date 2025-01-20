using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    public void HandleMapButtonClick()
    {
        SceneManager.LoadSceneAsync("Map");
    }

    public void HandleHomeButtonClick()
    {
        SceneManager.LoadSceneAsync("Home");
    }
}
