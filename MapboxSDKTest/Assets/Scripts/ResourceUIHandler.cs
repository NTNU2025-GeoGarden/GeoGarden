using Mapbox.Examples;
using Persistence;
using UnityEngine;

public class ResourceUIHandler : MonoBehaviour, IPersistence
{
    private int _waterCount = 0;
    
    public void HandleGatherResource()
    {
        CharacterMovement.OnCollectResource();
    }

    public void LoadData(GameState state)
    {
        _waterCount = state.water;
        print($"Got water {_waterCount}");
    }

    public void SaveData(ref GameState state)
    {
    }
}