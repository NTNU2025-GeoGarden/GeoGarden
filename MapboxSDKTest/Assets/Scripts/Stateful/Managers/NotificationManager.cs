using UnityEngine;
using TMPro;
using System.Collections;

namespace Stateful.Managers
{
    public class NotificationManager : MonoBehaviour
    {
        public static NotificationManager Instance { get; private set; }
        
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private float displayDuration = 3f;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void ShowNotification(string message)
        {
            if (notificationText == null || canvasGroup == null)
            {
                Debug.LogError("[NotificationManager] Text or CanvasGroup component not assigned!");
                return;
            }

            StopAllCoroutines();
            notificationText.text = message;
            StartCoroutine(FadeNotification());
        }

        private IEnumerator FadeNotification()
        {
            canvasGroup.alpha = 1;
            yield return new WaitForSeconds(displayDuration);
            
            float fadeTime = 0.5f;
            float elapsedTime = 0;
            
            while (elapsedTime < fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 0;
        }
    }
}