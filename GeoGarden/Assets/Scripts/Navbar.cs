using UnityEngine;
using UnityEngine.SceneManagement;

public class Navbar : MonoBehaviour
{
    // Update is called once per frame
    public void HandleMapButton()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void HandleHomeButton()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
