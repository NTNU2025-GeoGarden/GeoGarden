using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Map;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;
using Random = System.Random;


namespace Stateful.Managers
{
    public class MapResourceManager : MonoBehaviour, IUsingGameState
    {
        public GameObject mapObject;
        public GameObject resourcePrefab;
        public Transform player;
        public List<SpawnerCluster> clusters;
    
        private MapboxMapBehaviour _map;
        private int _todaysSeed;
        private Dictionary<int, List<SerializableSpawner>> _mapResources;
        private Random _random;
    
        void Start()
        {
            _map = mapObject.GetComponent<MapboxMapBehaviour>();

            _map.MapServiceReady += _ =>
            {
                AddInGameSpawners();
            };
        }

        public void LoadData(GameState state)
        {
            Debug.Log("<color=cyan>[MapResourceManager] Loading spawner data</color>");
            
            _todaysSeed = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
            _mapResources = state.MapResources;
        
            if (_mapResources.ContainsKey(_todaysSeed))
            {
                Debug.Log("<color=cyan>----> Spawner data found, skipping generation.</color>");
                return;
            }
            
            Debug.Log("<color=cyan>----> Not generated, proceeding with generation. </color>");
        
            _mapResources.Add(_todaysSeed, new List<SerializableSpawner>());
        
            // No data generated, fix that!
        
            _random = new Random(_todaysSeed);

            foreach (SpawnerCluster cluster in clusters)
            {
                if (!(_random.NextDouble() > 0.5)) continue;

                SerializableSpawner newResource = new()
                {
                    latLng = cluster.latLng,
                    spawner = cluster.spawners[_random.Next(cluster.spawners.Count)],
                    collected = false
                };
            
            
                _mapResources[_todaysSeed].Add(newResource);
            }
        }

        public void SaveData(ref GameState state)
        {
            Debug.Log("<color=cyan>[MapResourceManager] Saving data</color>");
            state.MapResources = _mapResources;
        }

       private void AddInGameSpawners()
       {
            if (_mapResources == null || !_mapResources.ContainsKey(_todaysSeed))
            {
                Debug.Log("<color=red>[MapResourceManager] Tried to generate spawners, but the data was not ready. Was LoadData() called?</color>");
                LoadingScreen.OnLoadingError();
                return;
            }

            int count = 0;

            foreach (SerializableSpawner resource in _mapResources[_todaysSeed])
            {
                Vector3 spawnPosition = _map.MapInformation.ConvertLatLngToPosition(resource.latLng);
                spawnPosition.y = 45f; // ✅ Set a fixed Y-value for all spawners

                GameObject newObj = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
                
                newObj.GetComponent<SnapObjectToMap>().latLng    = resource.latLng;
                newObj.GetComponent<SnapObjectToMap>().mapObject = mapObject;

                newObj.GetComponent<SpawnerOnMap>().player    = player;
                newObj.GetComponent<SpawnerOnMap>().spawner   = resource.spawner;
                newObj.GetComponent<SpawnerOnMap>().spawnerId = count;
                newObj.GetComponent<SpawnerOnMap>().latLng    = resource.latLng;
                newObj.GetComponent<SpawnerOnMap>().collected = resource.collected;
                newObj.GetComponent<SpawnerOnMap>().resourceManager = this;

                count++;
            }
       }

        public void Collect(SpawnerOnMap mapSpawner)
        {
            Debug.Log("<color=lime>[MapResourceManager] Trying to register that the player collected a resource</color>");

            // ✅ Check if _mapResources is null
            if (_mapResources == null)
            {
                LoadData(GameStateManager.CurrentState);
            }
            
            _mapResources![_todaysSeed][mapSpawner.spawnerId].collected = true;

            Spawner spawner = mapSpawner.spawner;
            
            SerializableInventoryEntry newEntry = new()
            {
                Id = spawner.itemId,
                Amount = _random.Next(spawner.minAmount, spawner.maxAmount)
            };

            GameStateManager.AddInventoryItem(newEntry);
            int randomWaterAmount = _random.Next(-5, 6); // Random amount between -5 and 5
            GameStateManager.CurrentState.Water += 15 + randomWaterAmount;
        }
    }
}