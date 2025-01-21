using System;
using System.Net;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using Unity.Mathematics;
using UnityEngine;

public class HomeCameraRotation : MonoBehaviour
{
    public float speed = 1;
    
    private Vector2 _previousPosition;
    private double _previousPinchDistance;
    private const float SCALE_FACTOR = 0.0001f;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount != 1) return;
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _previousPosition = touch.position;
                    break;
                case TouchPhase.Moved:
                {
                    Vector2 currentPosition = touch.position;
                    Vector2 delta = currentPosition - _previousPosition;
                    
                    transform.Translate(new Vector3((delta.y + delta.x) * speed * SCALE_FACTOR, 0f, (delta.y - delta.x) * speed * SCALE_FACTOR));

                    switch (transform.position.x)
                    {
                        case > 3f:
                            transform.Translate(new Vector3(3f - transform.position.x, 0f, 0f));
                            break;
                        case < -0.25f:
                            transform.Translate(new Vector3(-0.25f - transform.position.x, 0f, 0f));
                            break;
                    }
                    
                    switch (transform.position.z)
                    {
                        case > 2.6f:
                            transform.Translate(new Vector3(0f, 0f, 2.6f - transform.position.z));
                            break;
                        case < -0.6f:
                            transform.Translate(new Vector3(0f, 0f, -0.6f - transform.position.z));
                            break;
                    }
                    
                    _previousPosition = currentPosition;
                    break;
                }
                default:
                case TouchPhase.Stationary:
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    break;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _previousPosition = Input.mousePosition;
            }

            if (!Input.GetMouseButton(0)) return;
            
            Vector2 currentPosition = Input.mousePosition;
            Vector2 delta = currentPosition - _previousPosition;
            transform.Rotate(Vector3.up, delta.x * speed);
            _previousPosition = currentPosition;
        }
    }
}