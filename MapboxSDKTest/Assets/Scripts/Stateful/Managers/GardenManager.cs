using System;
using System.Collections.Generic;
using Garden;
using Structs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stateful.Managers
{
    public class GardenManager : MonoBehaviour, IUsingGameState
    {
        public delegate void DelegatePlantSeed(PlantableSpot spot, int seedID);
        public static DelegatePlantSeed OnPlantSeed;

        public delegate void DelegatePlantInteraction(PlantableSpot spot);
        public static DelegatePlantInteraction OnSeedTimeout;
        public static DelegatePlantInteraction OnPlantWater;
        public static DelegatePlantInteraction OnPlantHarvested;

        public GardenCamera gardenCamera;
        public GameObject editUI;
        public EditableObject editableObjectPrefab;
        private List<EditableObject> _objects;
        private List<SerializableGardenSpot> _serializedSpots;
        private List<PlantableSpot> _inGameSpots;

        public void Start()
        {
            SetDelegates();
            
            SceneManager.sceneLoaded += (_, _) =>
            {
                SetDelegates();
            };
        }

        private void SetDelegates()
        {
            OnPlantSeed      = HandlePlantSeed;
            OnSeedTimeout    = SeedTimeOut;
            OnPlantWater     = PlantWatered;
            OnPlantHarvested = PlantHarvested;
        }

        private void PlaceGardenSpots()
        {
            Debug.Log("<color=lime>[GardenManager] Generating growing spots</color>");
            int count = 0;
            
            if(_inGameSpots != null)
                foreach(PlantableSpot obj in _inGameSpots)
                    Destroy(obj.gameObject);
            
            _inGameSpots = new List<PlantableSpot>();
            
            if(_objects != null)
                _objects.Clear();
            else
                _objects = new List<EditableObject>();
            
            foreach (SerializableGardenSpot spot in _serializedSpots)
            {
                EditableObject newObj = Instantiate(editableObjectPrefab, transform);
                newObj.type = EditableObjectType.Spot;
                newObj.transform.localPosition = new Vector3(spot.X, spot.Y, spot.Z);
                newObj.editControls.transform.Translate(new Vector3(0, -1, 0));
                newObj.gardenCamera = gardenCamera;
                
                _objects.Add(newObj);
                
                PlantableSpot newSpot = newObj.spot;
                _inGameSpots.Add(newSpot);
                
                newSpot.spotID = count;
                SetPlantableSpotData(spot, newSpot);

                newObj.GetComponent<BoxCollider>().enabled = false;
                
                count++;
            }
        }

        public void ObjectChanged(EditableObject obj)
        {
            SerializableGardenSpot updatedSerializedObj = _serializedSpots[obj.spot.spotID];
            updatedSerializedObj.X = obj.transform.localPosition.x;
            updatedSerializedObj.Y = obj.transform.localPosition.y;
            updatedSerializedObj.Z = obj.transform.localPosition.z;

            _serializedSpots[obj.spot.spotID] = updatedSerializedObj;
        }
        
        private static void SetPlantableSpotData(SerializableGardenSpot serializedSpot, PlantableSpot plantableSpot)
        {
            Debug.Log($"<color=lime>[GardenManager] Updating spot data (ID {plantableSpot.spotID})</color>");
            
            plantableSpot.completionTime = serializedSpot.stateCompletionTime;
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

            plantableSpot.boxCollider.center = new Vector3(0, 0, -0.61f);
            plantableSpot.boxCollider.size   = new Vector3(0.59f, 0.58f, 1.13f);
            
            switch(plantableSpot.state)
            {
                case GrowState.Vacant:
                    plantableSpot.boxCollider.center = new Vector3(0, 0, -0.1f);
                    plantableSpot.boxCollider.size   = new Vector3(1, 1, 0.1f);
                    
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
            Debug.Log("<color=cyan>[GardenManager] Loading data</color>");
            _serializedSpots = state.GardenSpots;
            PlaceGardenSpots();
        }

        public void SaveData(ref GameState state)
        {
            Debug.Log("<color=cyan>[GardenManager] Saving data</color>");
            state.GardenSpots = _serializedSpots;
        }

        private void HandlePlantSeed(PlantableSpot spot, int seedID)
        {
            Debug.Log($"<color=lime>[GardenManager] Planting a seed (ID {seedID}) in spot (ID {spot.spotID})</color>");
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spot.spotID];
            updatedSerializedSpot.stateCompletionTime = DateTime.Now.AddSeconds(5); //TODO get proper time
            updatedSerializedSpot.state = GrowState.Seeded;
            updatedSerializedSpot.seedID = seedID;

            _serializedSpots[spot.spotID] = updatedSerializedSpot;
            
            SetPlantableSpotData(updatedSerializedSpot, spot);
        }
        
        private void SeedTimeOut(PlantableSpot spot)
        {
            spot.perimeter.SetActive(true);
            spot.statusSymbolNeedsWater.SetActive(true);
            spot.statusSymbolTimer.gameObject.SetActive(false);
        }

        private void PlantWatered(PlantableSpot spot)
        {
            Debug.Log($"<color=lime>[GardenManager] Watered spot (ID {spot.spotID})</color>");
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spot.spotID];
            updatedSerializedSpot.stateCompletionTime = DateTime.Now.AddSeconds(5); //TODO get proper time
            updatedSerializedSpot.state += 1;

            _serializedSpots[spot.spotID] = updatedSerializedSpot;
            
            SetPlantableSpotData(_serializedSpots[spot.spotID], spot);
        }

        private void PlantHarvested(PlantableSpot spot)
        {
            Debug.Log($"<color=lime>[GardenManager] Harvested spot (ID {spot.spotID})</color>");
            
            SerializableGardenSpot updatedSerializedSpot = _serializedSpots[spot.spotID];
            updatedSerializedSpot.stateCompletionTime = DateTime.MinValue;
            updatedSerializedSpot.state = 0;

            GameStateManager.OnAddInventoryItem(Seeds.FromID(updatedSerializedSpot.seedID).ProductItemID);
            
            updatedSerializedSpot.seedID = 0;
            _serializedSpots[spot.spotID] = updatedSerializedSpot;
            
            SetPlantableSpotData(_serializedSpots[spot.spotID], spot);
        }
    }
}
