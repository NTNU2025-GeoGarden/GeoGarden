using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<SerializableGardenSpot> _serializedSpots;
        private List<PlantableSpot> _inGameSpots;

        public void Start()
        {
            OnPlantSeed += HandlePlantSeed;
        }

        private void PlaceGardenSpots()
        {
            int count = 0;
            _inGameSpots = new List<PlantableSpot>();
            
            foreach (SerializableGardenSpot spot in _serializedSpots)
            {
                PlantableSpot newSpot = Instantiate(plantableSpot, transform);
                _inGameSpots.Add(newSpot);

                int row = (int)Math.Floor((float)count / gardenDepth);
            
                newSpot.transform.localPosition = new Vector3(
                    0.3f + 0.8f * row, 
                    0, 
                    0.3f + 0.8f * (count - row * gardenDepth)
                );
            
                newSpot.ID = count;
            
                SetPlantableSpotData(spot, newSpot);

                count++;
            }
        }

        private static void SetPlantableSpotData(SerializableGardenSpot serializedSpot, PlantableSpot plantableSpot)
        {
            plantableSpot.completionTime = serializedSpot.stateCompletionTime;;
            plantableSpot.state = serializedSpot.state;
            
            switch(serializedSpot.state)
            {
                case GrowState.Vacant:
                    plantableSpot.growingStage1.SetActive(false);
                    plantableSpot.growingStage2.SetActive(false);
                    plantableSpot.growingStage3.SetActive(false);
                    plantableSpot.growingStage4.SetActive(false);
                    
                    plantableSpot.perimeter.SetActive(true);
                    
                    plantableSpot.statusSymbolAddPlant.SetActive(true);
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(false);
                    
                    plantableSpot.completionTime = DateTime.MinValue;
                    break;
                case GrowState.Seeded:
                    plantableSpot.growingStage1.SetActive(true);
                    plantableSpot.growingStage2.SetActive(false);
                    plantableSpot.growingStage3.SetActive(false);
                    plantableSpot.growingStage4.SetActive(false);
                    
                    plantableSpot.perimeter.SetActive(false);
                    
                    plantableSpot.statusSymbolAddPlant.SetActive(false);
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    break;
                case GrowState.Stage2:
                    plantableSpot.growingStage1.SetActive(false);
                    plantableSpot.growingStage2.SetActive(true);
                    plantableSpot.growingStage3.SetActive(false);
                    plantableSpot.growingStage4.SetActive(false);
                    
                    plantableSpot.perimeter.SetActive(false);
                    
                    plantableSpot.statusSymbolAddPlant.SetActive(false);
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    break;
                case GrowState.Stage3:
                    plantableSpot.growingStage1.SetActive(false);
                    plantableSpot.growingStage2.SetActive(false);
                    plantableSpot.growingStage3.SetActive(true);
                    plantableSpot.growingStage4.SetActive(false);
                    
                    plantableSpot.perimeter.SetActive(false);
                    
                    plantableSpot.statusSymbolAddPlant.SetActive(false);
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    break;
                case GrowState.Complete:
                    plantableSpot.growingStage1.SetActive(false);
                    plantableSpot.growingStage2.SetActive(false);
                    plantableSpot.growingStage3.SetActive(false);
                    plantableSpot.growingStage4.SetActive(true);
                    
                    plantableSpot.perimeter.SetActive(true);
                    
                    plantableSpot.statusSymbolAddPlant.SetActive(false);
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(false);
                    
                    plantableSpot.completionTime = DateTime.MinValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void LoadData(GameState state)
        {
            _serializedSpots = state.GardenSpots;
            PlaceGardenSpots();
        }

        public void SaveData(ref GameState state)
        {
            state.GardenSpots = _serializedSpots;
        }

        private void HandlePlantSeed(int spot, int seedId)
        {
            Debug.Log($"Got notice that player planted a seed. The seed is {seedId} @ spot {spot}.");
            
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spot];
            updatedSerializedSpot.stateCompletionTime = DateTime.Now.AddMinutes(5);
            updatedSerializedSpot.state = GrowState.Seeded;
            updatedSerializedSpot.seedID = seedId;

            _serializedSpots[spot] = updatedSerializedSpot;
            
            SetPlantableSpotData(updatedSerializedSpot, _inGameSpots[spot]);
        }
    }
}
