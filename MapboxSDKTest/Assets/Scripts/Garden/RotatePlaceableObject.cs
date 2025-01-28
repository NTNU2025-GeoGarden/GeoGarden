using System;
using UnityEngine;

namespace Garden
{
    public class RotatePlaceableObject : MonoBehaviour
    {
        public bool isSelected;
        public float speed;

        private void Update()
        {
            if(isSelected)
                transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
    }
}