using Stateful;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FirebaseStatusIcon : MonoBehaviour
    {
        public GameObject cross;
        public GameObject check;

        public TMP_Text uid;

        public GameObject userSettingsScreen;

        public void Update()
        {
            if (FirebaseManager.FirebaseAvailable)
            {
                check.SetActive(true);
                cross.SetActive(false);
                uid.gameObject.SetActive(true);
                
                uid.text = $"UID: {GameStateManager.CurrentState.UID}";
            }
            else
            {
                cross.SetActive(true);
                check.SetActive(false);
                uid.gameObject.SetActive(false);
            }
            
            userSettingsScreen.SetActive(GameStateManager.CurrentState.UID == "");
        }
    }
}
