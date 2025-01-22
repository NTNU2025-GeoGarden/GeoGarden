using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class GrowSpotManager : MonoBehaviour, IPersistence
{
    private List<SavedGardenSpot> _gardenSpots;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData(GameState state)
    {
        _gardenSpots = state.GardenSpots;
    }

    public void SaveData(ref GameState state)
    {
        state.GardenSpots = _gardenSpots;
    }
}
