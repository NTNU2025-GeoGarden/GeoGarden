using Mapbox.Examples;
using UnityEngine;

public class ResourceUIHandler : MonoBehaviour
{
    public void HandleGatherResource()
    {
        CharacterMovement.OnCollectResource();
    }
}