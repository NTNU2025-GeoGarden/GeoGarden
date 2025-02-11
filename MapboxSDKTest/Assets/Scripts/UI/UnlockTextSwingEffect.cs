using UnityEngine;

public class UnlockTextSwingEffect : MonoBehaviour
{
    public float amplitude = 1.0f;
    private RectTransform _rt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rt = GetComponent<RectTransform>();
    }
    //test comment
    // Update is called once per frame
    void Update()
    {
        _rt.Rotate(Vector3.forward * (amplitude * Mathf.Sin(Time.time)));
    }
}
