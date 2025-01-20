using System;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using Unity.Mathematics;
using UnityEngine;

public class HomeCameraRotation : MonoBehaviour
{
    public float speed = 1;
    
    private Vector2 _previousPosition;
    private double _previousPinchDistance;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    _previousPosition = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 currentPosition = touch.position;
                    Vector2 delta = currentPosition - _previousPosition;
                    transform.Rotate(Vector3.up, delta.x * speed);
                    transform.Rotate(Vector3.left, delta.y * speed);
                    transform.Rotate(0, 0, -transform.rotation.eulerAngles.z);
                    _previousPosition = currentPosition;
                }
                    
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousPosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 currentPosition = Input.mousePosition;
                Vector2 delta = currentPosition - _previousPosition;
                transform.Rotate(Vector3.up, delta.x * speed);
                _previousPosition = currentPosition;
            }
        }
    }
}