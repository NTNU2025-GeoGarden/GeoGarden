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
        public delegate void DelegatePlantSeed(int id, int seedId);
        public static DelegatePlantSeed OnPlantSeed;
        
        public int gardenDepth = 4;
        [FormerlySerializedAs("gardenSpot")] public PlantableSpot plantableSpot;
        private List<SerializableGardenSpot> _gardenSpots;

        public void Start()
        {
            OnPlantSeed += HandlePlantSeed;
        }

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
                        newSpot.completionTime = DateTime.MinValue;
                        break;
                    case GrowState.Seeded:
                        newSpot.growingStage1.SetActive(true);
                        newSpot.completionTime = spot.stateCompletionTime;
                        break;
                    case GrowState.Stage2:
                        newSpot.growingStage2.SetActive(true);
                        newSpot.completionTime = spot.stateCompletionTime;
                        break;
                    case GrowState.Stage3:
                        newSpot.growingStage3.SetActive(true);
                        newSpot.completionTime = spot.stateCompletionTime;
                        break;
                    case GrowState.Complete:
                        newSpot.growingStage4.SetActive(true);
                        newSpot.completionTime = DateTime.MinValue;
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

        private void HandlePlantSeed(int spot, int seedId)
        {
            Debug.Log($"Got notice that player planted a seed. The seed is {seedId} @ spot {spot}.");
        }
    }
}
