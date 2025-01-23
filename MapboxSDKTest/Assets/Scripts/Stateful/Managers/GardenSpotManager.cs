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

        public delegate void DelegateSeedTimerFinished(int id);
        public static DelegateSeedTimerFinished OnSeedTimeout;
        
        public delegate void DelegatePlantWatered(int id);
        public static DelegatePlantWatered OnPlantWater;
        
        public int gardenDepth = 4;
        [FormerlySerializedAs("gardenSpot")] public PlantableSpot plantableSpot;
        private List<SerializableGardenSpot> _serializedSpots;
        private List<PlantableSpot> _inGameSpots;

        public void Start()
        {
            OnPlantSeed += HandlePlantSeed;
            OnSeedTimeout += SeedTimeOut;
            OnPlantWater += PlantWatered;
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
            
                newSpot.spotID = count;
                
                SetPlantableSpotData(spot, newSpot);

                count++;
            }
        }

        private static void SetPlantableSpotData(SerializableGardenSpot serializedSpot, PlantableSpot plantableSpot)
        {
            plantableSpot.completionTime = serializedSpot.stateCompletionTime;;
            plantableSpot.state = serializedSpot.state;
            plantableSpot.seedID = serializedSpot.seedID;
            
            plantableSpot.growingStage1.SetActive(false);
            plantableSpot.growingStage2.SetActive(false);
            plantableSpot.growingStage3.SetActive(false);
            plantableSpot.growingStage4.SetActive(false);
            plantableSpot.perimeter.SetActive(false);
            plantableSpot.statusSymbolAddPlant.SetActive(false);
            plantableSpot.statusSymbolFinished.SetActive(false);
            plantableSpot.statusSymbolNeedsWater.SetActive(false);
            plantableSpot.statusSymbolTimer.gameObject.SetActive(false);
            
            switch(serializedSpot.state)
            {
                case GrowState.Vacant:
                    plantableSpot.perimeter.SetActive(true);
                    plantableSpot.statusSymbolAddPlant.SetActive(true);
                    plantableSpot.completionTime = DateTime.MinValue;
                    break;
                case GrowState.Seeded:
                    plantableSpot.growingStage1.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    break;
                case GrowState.Stage2:
                    plantableSpot.growingStage2.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    break;
                case GrowState.Stage3:
                    plantableSpot.growingStage3.SetActive(true);
                    
                    plantableSpot.completionTime = serializedSpot.stateCompletionTime;
                    plantableSpot.statusSymbolTimer.gameObject.SetActive(true);
                    break;
                case GrowState.Complete:
                    plantableSpot.growingStage4.SetActive(true);
                    plantableSpot.perimeter.SetActive(true);
                    plantableSpot.statusSymbolFinished.SetActive(true);
                    
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

        private void HandlePlantSeed(int spotID, int seedID)
        {
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spotID];
            updatedSerializedSpot.stateCompletionTime = DateTime.Now.AddSeconds(5); //TODO get proper time
            updatedSerializedSpot.state = GrowState.Seeded;
            updatedSerializedSpot.seedID = seedID;

            _serializedSpots[spotID] = updatedSerializedSpot;
            
            SetPlantableSpotData(updatedSerializedSpot, _inGameSpots[spotID]);
        }
        
        private void SeedTimeOut(int spotID)
        {
            _inGameSpots[spotID].perimeter.SetActive(true);
            _inGameSpots[spotID].statusSymbolNeedsWater.SetActive(true);
            _inGameSpots[spotID].statusSymbolTimer.gameObject.SetActive(false);
        }

        private void PlantWatered(int spotID)
        {
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spotID];
            updatedSerializedSpot.stateCompletionTime = DateTime.Now.AddSeconds(5); //TODO get proper time
            updatedSerializedSpot.state += 1;

            _serializedSpots[spotID] = updatedSerializedSpot;
            
            SetPlantableSpotData(_serializedSpots[spotID], _inGameSpots[spotID]);
        }
    }
}
