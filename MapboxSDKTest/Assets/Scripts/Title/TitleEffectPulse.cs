using System;
using UnityEngine;

namespace Title
{
    public class TitleEffectPulse : MonoBehaviour
    {

        public float alphaOffset;
        public float pulseSpeed;
        
        
        private CanvasGroup _cg;

        public void Start()
        {
            _cg = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {
            _cg.alpha = Math.Clamp((float)((Math.Sin(Time.time * pulseSpeed) + 1) / 2) + alphaOffset, 0, 1);
        }
    }
}
