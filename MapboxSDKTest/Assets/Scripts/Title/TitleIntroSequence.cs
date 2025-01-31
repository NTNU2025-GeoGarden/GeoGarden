using System.Collections;
using UnityEngine;

namespace Title
{
    public class TitleIntroSequence : MonoBehaviour
    {
        public float introSequenceTotalTime;
        public float introTextAppearTime;

        public float fadeOutSpeed = 1;
        public float textFadeSpeed = 1;
        
        public CanvasGroup introSequence;
        public CanvasGroup introText;

        private float _timer;
        private bool _fadeoutStarted;
        private bool _textFadeStarted;
        
        // Update is called once per frame
        void Update()
        {
            if(introSequence != null && !_fadeoutStarted && _timer >= introSequenceTotalTime)
            {
                StartCoroutine(FadeAway());
                _fadeoutStarted = true;
            }
            
            if(introText != null &&!_textFadeStarted && _timer >= introTextAppearTime)
            {
                StartCoroutine(TextFadeIn());
                _textFadeStarted = true;
            }

            _timer += Time.deltaTime;
        }
    
        private IEnumerator FadeAway()
        {
            while (introSequence.alpha > 0)
            {
                introSequence.alpha -= Time.deltaTime * fadeOutSpeed;
                introText.alpha -= Time.deltaTime * fadeOutSpeed;
                yield return null;
            }
        
            gameObject.SetActive(false);
        }
        
        private IEnumerator TextFadeIn()
        {
            while (introText.alpha < 1)
            {
                introText.alpha += Time.deltaTime * textFadeSpeed;
                yield return null;
            }
        }
    }
}
