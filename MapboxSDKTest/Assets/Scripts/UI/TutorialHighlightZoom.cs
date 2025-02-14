using System;
using UnityEngine;

namespace UI
{
    public class TutorialHighlightZoom : MonoBehaviour
    {
        public float amplitude = 0.05f;
        
        public void Update()
        {
            transform.localScale = Vector3.one + Vector3.one * (amplitude * Mathf.Sin(Time.time));
        }
    }
}