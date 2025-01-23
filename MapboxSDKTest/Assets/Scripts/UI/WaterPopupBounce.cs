using System;
using UnityEngine;

public class WaterPopupBounce : MonoBehaviour
{
    private Vector3 _basePosition;
    public float speed = 1.0f;
    
    void Start()
    {
        _basePosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(_basePosition.x, _basePosition.y + (float)Math.Sin(Time.time) * Time.deltaTime * speed, _basePosition.z);
    }
}
