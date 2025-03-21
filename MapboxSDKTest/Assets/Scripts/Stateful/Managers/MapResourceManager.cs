using System;
using System.Collections.Generic;
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
        private List<GameObject> _spawnedObjects = new List<GameObject>();
        private bool _isInitialized;
        private static Random random = new Random();
        public MapboxMapBehaviour map;

        void Start()
        {
            _map = mapObject.GetComponent<MapboxMapBehaviour>();
            _todaysSeed = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
            _random = new Random(_todaysSeed);

            LoadData(GameStateManager.CurrentState);
            _map.MapServiceReady += OnMapReady;
        }

        private void OnMapReady(MapService mapService)
        {
            if (!_isInitialized)
            {
                InitializeCoordinates();
            }

            ClearExistingSpawners();
            AddInGameSpawners();
        }

        private int GetWeightedRandomItemId()
        {
            double roll = _random.NextDouble();

            if (roll < 0.8)
                return 0;   // 80% chance common
            else if (roll < 0.99)
                return 1;  // 19% chance uncommon
            else if (roll < 0.9995)
                return 2;  // 0.95% chance rare
            else
                return 3;  // 0.05% chance legendary
        }

        private void InitializeCoordinates()
        {
            if (_mapResources.ContainsKey(_todaysSeed))
                return;

            LatitudeLongitude playerPosition = _map.MapInformation.ConvertPositionToLatLng(player.position);

            List<LatitudeLongitude> spawnerLocs = CoordinateGenerator.GenerateUniquePoints(
                playerPosition.Latitude,
                playerPosition.Longitude,
                5000,
                2000,
                0.3
            );

            clusters = new List<SpawnerCluster>();

            foreach (LatitudeLongitude pos in spawnerLocs)
            {
                AddSpawnerCluster(pos.Latitude, pos.Longitude);
            }

            GenerateDailyResources();

            Debug.Log($"<color=cyan>[MapResourceManager] Initialized {spawnerLocs.Count} spawner clusters for new day</color>");

            _isInitialized = true;
        }

        private void GenerateDailyResources()
        {
            _mapResources[_todaysSeed] = new List<SerializableSpawner>();

            foreach (SpawnerCluster cluster in clusters)
            {
                SerializableSpawner newResource = new()
                {
                    latLng = cluster.latLng,
                    spawner = cluster.spawners[0],
                    collected = false
                };

                if (_random.NextDouble() > 0.5)
                    _mapResources[_todaysSeed].Add(newResource);
            }

            GameStateManager.CurrentState.MapResources = _mapResources;

            Debug.Log($"<color=cyan>[MapResourceManager] Generated {_mapResources[_todaysSeed].Count} resources for today</color>");
        }

        public void AddSpawnerCluster(double latitude, double longitude)
        {
            List<Spawner> testSpawners = new List<Spawner>();

            testSpawners.Add(new Spawner
            {
                itemId = GetWeightedRandomItemId(),
                minAmount = 1,
                maxAmount = 1
            });

            SpawnerCluster newCluster = new SpawnerCluster
            {
                latLng = new LatitudeLongitude(latitude, longitude),
                spawners = testSpawners
            };

            clusters.Add(newCluster);
        }

        private void ClearExistingSpawners()
        {
            foreach (var obj in _spawnedObjects)
            {
                if (obj != null) Destroy(obj);
            }
            _spawnedObjects.Clear();
        }

        private void AddInGameSpawners()
        {
            if (_mapResources == null || !_mapResources.ContainsKey(_todaysSeed))
            {
                Debug.Log("<color=red>[MapResourceManager] No resources available for today</color>");
                LoadingScreen.OnLoadingError();

                return;
            }

            int count = 0;
            foreach (SerializableSpawner resource in _mapResources[_todaysSeed])
            {
                Vector3 spawnPosition = _map.MapInformation.ConvertLatLngToPosition(resource.latLng);
                spawnPosition.y = 45f;

                GameObject newObj = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);

                newObj.GetComponent<SnapObjectToMap>().latLng = resource.latLng;
                newObj.GetComponent<SnapObjectToMap>().mapObject = mapObject;

                var spawner = newObj.GetComponent<SpawnerOnMap>();
                spawner.player = player;
                spawner.spawner = resource.spawner;
                spawner.spawnerId = count;
                spawner.latLng = resource.latLng;
                spawner.collected = resource.collected;
                spawner.resourceManager = this;

                _spawnedObjects.Add(newObj);
                count++;
            }
        }

        public void LoadData(GameState state)
        {
            Debug.Log("<color=cyan>[MapResourceManager] Loading data</color>");
            _mapResources = state.MapResources;

            if (_mapResources == null)
            {
                _mapResources = new Dictionary<int, List<SerializableSpawner>>();
            }
        }

        public void SaveData(ref GameState state)
        {
            Debug.Log("<color=cyan>[MapResourceManager] Saving data</color>");
            state.MapResources = _mapResources;
        }

        public void Collect(SpawnerOnMap mapSpawner)
        {
            if (_mapResources == null || !_mapResources.ContainsKey(_todaysSeed))
            {
                Debug.LogError("[MapResourceManager] Map resources not initialized!");
                return;
            }

            _mapResources[_todaysSeed][mapSpawner.spawnerId].collected = true;

            SerializableInventoryEntry newEntry = new()
            {
                Id = mapSpawner.spawner.itemId,
                Amount = random.Next(mapSpawner.spawner.minAmount, mapSpawner.spawner.maxAmount + 1)
            };

            int waterGained = 15 + random.Next(-5, 6);
            GameStateManager.AddInventoryItem(newEntry);
            GameStateManager.CurrentState.Water += waterGained;

            string rarity = newEntry.Id switch
            {
                < 24 => "Common",
                < 40 => "Uncommon",
                < 47 => "Rare",
                _ => "Legendary"
            };

            string message = $"Found {newEntry.Amount} {rarity} seed(s) and {waterGained} water!";
            NotificationManager.Instance?.ShowNotification(message);
        }
    }


}