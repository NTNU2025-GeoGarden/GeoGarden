using Mapbox.Examples;
using Persistence;
using UnityEngine;

public class ResourceUIHandler : MonoBehaviour, IPersistence
{
    public void HandleGatherResource()
    {
        CharacterMovement.OnCollectResource();
    }

    public void LoadData(GameState state)
    {
    }

    public void SaveData(ref GameState state)
    {
    }
}