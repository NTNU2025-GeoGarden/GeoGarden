using UnityEngine;

public class CameraTouch : MonoBehaviour
{
    public float sensitivity = 1;
    public Transform camSphere;

    private Vector3 dragStart;
    private Camera _camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStart = _camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 rotAmount = dragStart - _camera.ScreenToViewportPoint(Input.mousePosition);
            
            camSphere.Rotate(new Vector3(0, -rotAmount.x * sensitivity, 0));
            
            dragStart = _camera.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}
