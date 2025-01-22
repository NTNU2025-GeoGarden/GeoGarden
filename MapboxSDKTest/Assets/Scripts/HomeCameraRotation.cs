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
    private bool _tapped;

    private const float SCALE_FACTOR = 0.0001f;
    
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount != 1)
            {
                
            }
            else
            {
                Touch touch = Input.GetTouch(0);
            
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _previousPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
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
                    case TouchPhase.Stationary:
                        if (!_tapped)
                        {
                            Ray ray = Camera.main!.ScreenPointToRay(touch.position);
                            if (Physics.Raycast(ray, out RaycastHit hit, 100))
                            {
                                if (hit.transform.CompareTag("PlantSpot"))
                                {
                                    GrowSpot spot = hit.transform.GetComponent<GrowSpot>();
                                    
                                }
                            }
                            
                            _tapped = true;
                        }
                        break;
                    case TouchPhase.Ended:
                        _tapped = false;
                        break;
                    default:
                    case TouchPhase.Canceled:
                        break;
                }
            }
        }
    }
}