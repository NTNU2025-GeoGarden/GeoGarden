using System;
using System.Collections.Generic;
using Persistence;
using UnityEngine;

public class GardenSpotManager : MonoBehaviour, IPersistence
{
    public int gardenDepth = 4;
    public GardenSpot gardenSpot;
    private List<SavedGardenSpot> _gardenSpots;

    private void PlaceGardenSpots()
    {
        int count = 0;
        foreach (SavedGardenSpot spot in _gardenSpots)
        {
            GardenSpot newSpot = Instantiate(gardenSpot, transform);

            int row = (int)Math.Floor((float)count / gardenDepth);
            
            newSpot.transform.localPosition = new Vector3(
                0.3f + 0.8f * row, 
                0, 
                0.3f + 0.8f * (count - row * gardenDepth)
                );
            
            newSpot.ID = count;
            
            switch(spot.State)
            {
                case GrowState.Vacant:
                    newSpot.perimiter.SetActive(true);
                    newSpot.statusSymbolAddPlant.SetActive(true);
                    break;
                case GrowState.Seeded:
                    newSpot.growingStage1.SetActive(true);
                    break;
                case GrowState.Stage2:
                    newSpot.growingStage2.SetActive(true);
                    break;
                case GrowState.Stage3:
                    newSpot.growingStage3.SetActive(true);
                    break;
                case GrowState.Complete:
                    newSpot.growingStage4.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            count++;
        }
    }

    public void LoadData(GameState state)
    {
        _gardenSpots = state.GardenSpots;
        PlaceGardenSpots();
    }

    public void SaveData(ref GameState state)
    {
        state.GardenSpots = _gardenSpots;
    }
}
