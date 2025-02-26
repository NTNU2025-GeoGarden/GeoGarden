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
            // Initialize test coordinates first
            InitializeTestCoordinates();
            
            // Then load the data which will process the clusters
            LoadData(GameStateManager.CurrentState);
            _map.MapServiceReady += _ =>
            {
                AddInGameSpawners();
            };
        }
        private void InitializeTestCoordinates()
        {
            TextAsset coordFile = Resources.Load<TextAsset>("points");
            if (coordFile == null)
            {
                Debug.LogError("[MapResourceManager] Could not load coordinates.txt file!");
                return;
            }

            string[] lines = coordFile.text.Split('\n');
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                
                string[] parts = line.Trim().Split(',');
                if (parts.Length == 2 && 
                    double.TryParse(parts[0], out double lat) && 
                    double.TryParse(parts[1], out double lng))
                {
                    AddTestSpawnerCluster(lat, lng);
                }
            }
            
            Debug.Log($"<color=cyan>[MapResourceManager] Added {lines.Length} test spawner clusters from file</color>");
        }
        public void AddTestSpawnerCluster(double latitude, double longitude)
        {
            if (clusters == null)
            {
                clusters = new List<SpawnerCluster>();
            }

            List<Spawner> testSpawners = new List<Spawner>
            {
                new Spawner { itemId = 1, minAmount = 1, maxAmount = 3 },  // Common
                new Spawner { itemId = 2, minAmount = 2, maxAmount = 4 },  // Uncommon
                new Spawner { itemId = 3, minAmount = 1, maxAmount = 2 },  // Rare
                new Spawner { itemId = 4, minAmount = 1, maxAmount = 1 }   // Legendary
            };

            SpawnerCluster newCluster = new SpawnerCluster
            {
                latLng = new LatitudeLongitude(latitude, longitude),
                spawners = testSpawners
            };

            clusters.Add(newCluster);
            Debug.Log($"<color=cyan>[MapResourceManager] Added test spawner cluster at {latitude}, {longitude}</color>");
        }
        public void LoadData(GameState state)
{
    Debug.Log("<color=cyan>[MapResourceManager] Loading spawner data</color>");
    
    _todaysSeed = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
    _mapResources = state.MapResources ?? new Dictionary<int, List<SerializableSpawner>>();
    
    // Clear or create new list for today
    _mapResources[_todaysSeed] = new List<SerializableSpawner>();
    
    Debug.Log("<color=cyan>----> Generating resources from clusters</color>");
    
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

    // Update the state's reference
    state.MapResources = _mapResources;
    
    Debug.Log($"<color=cyan>[MapResourceManager] Generated {_mapResources[_todaysSeed].Count} resources for today</color>");
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