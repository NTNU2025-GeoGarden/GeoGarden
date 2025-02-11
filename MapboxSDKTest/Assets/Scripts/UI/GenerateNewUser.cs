using Firebase;
using Stateful;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GenerateNewUser : MonoBehaviour
    {
        public TMP_InputField uid;
        public Button start;
        public GameObject canvas;
        
        public void HandleGenerateNewUser()
        {
            uid.text = GenerateNewUID();
        }

        public void HandleStart()
        {
            GameStateManager.CurrentState.UID = uid.text;
            FirebaseManager.CreateNewUserDocument();
            canvas.SetActive(false);
        }
        
        public void Update()
        {
            if(start) start.interactable = uid.text.Length == 7;
        }

        private string GenerateNewUID()
        {
            int num1 = UnityEngine.Random.Range(1000, 9999);
            int num2 = UnityEngine.Random.Range(10, 99);
            return $"{num1}-{num2}";
        }
    }
}