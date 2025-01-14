using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(Resource.Seed);
        }
    }
}
