using System;
using System.Collections.Generic;
using Garden;
using Structs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Stateful.Managers
{
    public class GardenSpotManager : MonoBehaviour, IUsingGameState
    {
        public int gardenDepth = 4;
        [FormerlySerializedAs("gardenSpot")] public PlantableSpot plantableSpot;
        private List<SerializableGardenSpot> _gardenSpots;

        private void PlaceGardenSpots()
        {
            int count = 0;
            foreach (SerializableGardenSpot spot in _gardenSpots)
            {
                PlantableSpot newSpot = Instantiate(plantableSpot, transform);

                int row = (int)Math.Floor((float)count / gardenDepth);
            
                newSpot.transform.localPosition = new Vector3(
                    0.3f + 0.8f * row, 
                    0, 
                    0.3f + 0.8f * (count - row * gardenDepth)
                );
            
                newSpot.ID = count;
            
                switch(spot.state)
                {
                    case GrowState.Vacant:
                        newSpot.perimeter.SetActive(true);
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

        public void LoadData(Stateful.GameState state)
        {
            _gardenSpots = state.GardenSpots;
            PlaceGardenSpots();
        }

        public void SaveData(ref Stateful.GameState state)
        {
            state.GardenSpots = _gardenSpots;
        }
    }
}
