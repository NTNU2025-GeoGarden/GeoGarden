using System;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    private Vector2 _previousPosition;
    private double _previousPinchDistance;
    public float speed = 1;
    public float pinchSpeed = 1;
    public GameObject map;
    private MapboxMapBehaviour _mapboxMapBehaviour;

    private void Start()
    {
        _mapboxMapBehaviour = map.GetComponent<MapboxMapBehaviour>();
    }

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
                    _previousPosition = currentPosition;
                }
                    
            }
            else if (Input.touchCount == 2)
            {
                Touch first = Input.GetTouch(0);
                Touch second = Input.GetTouch(1);

                if (second.phase == TouchPhase.Began)
                {
                    _previousPinchDistance = Math.Sqrt(Math.Pow(first.position.x - second.position.x, 2) +
                                                       Math.Pow(first.position.x - second.position.x, 2));
                }

                if (second.phase == TouchPhase.Moved)
                {
                    double pinchDistance = Math.Sqrt(Math.Pow(first.position.x - second.position.x, 2) +
                                                     Math.Pow(first.position.x - second.position.x, 2));

                    double delta = pinchSpeed * (pinchDistance - _previousPinchDistance) / 100;
                    ZoomMap(delta);
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
            
            if (Input.mouseScrollDelta.y != 0)
            {
                ZoomMap(Input.mouseScrollDelta.y * pinchSpeed);
            }
        }
    }

    private void ZoomMap(double delta)
    {
        MapInformation info = _mapboxMapBehaviour.MapInformation;

        if (info.Zoom < 6 && delta < 0)
            delta = 0;
        else if (info.Zoom > 15 && delta > 0)
            delta = 0;
                        
        //f(n)=1.5625⋅2(15−n)

        float newZoom = info.Zoom + (float)delta;
                        
        _mapboxMapBehaviour.MapInformation.SetInformation(info.LatitudeLongitude,
            newZoom, info.Pitch, info.Bearing, (float)1.5625*(float)Math.Pow(2, 15 - newZoom));
    }
}