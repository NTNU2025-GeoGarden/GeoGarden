using UnityEngine;

namespace Garden
{
    public class SetProgressBar : MonoBehaviour
    {
        public float progress;
        public Vector3 leftMostPosition = new(0.5f, 0, 0);
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            transform.localPosition = leftMostPosition;
            transform.localScale = new Vector3(progress, 1, 1);
        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition = leftMostPosition - new Vector3(progress * 0.5f, 0, 0);
            transform.localScale = new Vector3(progress, 1, 1);
        }
    }
}
