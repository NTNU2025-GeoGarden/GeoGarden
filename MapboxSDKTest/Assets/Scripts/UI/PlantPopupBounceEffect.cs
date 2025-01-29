using System;
using UnityEngine;

namespace UI
{
    public class PlantPopupBounceEffect : MonoBehaviour
    {
        private Vector3 _basePosition;
        public float speed = 1.0f;
    
        void Start()
        {
            _basePosition = transform.localPosition;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.localPosition = new Vector3(_basePosition.x, _basePosition.y, _basePosition.z + (float)Math.Sin(Time.time) * Time.deltaTime * speed);
        }
    }
}
