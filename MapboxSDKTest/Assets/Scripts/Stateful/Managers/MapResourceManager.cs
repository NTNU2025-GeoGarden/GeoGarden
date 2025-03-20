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
        private int _lastGeneratedDay = -1;
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
                InitializeTestCoordinates();
            }
            ClearExistingSpawners();
            AddInGameSpawners();
        }

        private int GetWeightedRandomItemId()
        {
            double roll = random.NextDouble();
            Debug.Log(roll);

            if (roll < 0.8)
                return random.Next(4, 28);   // 80% chance common
            else if (roll < 0.99)
                return random.Next(28, 44);  // 19% chance uncommon
            else if (roll < 0.9995)
                return random.Next(44, 51);  // 0.95% chance rare
            else
                return random.Next(51, 54);  // 0.05% chance legendary
        }

        private void InitializeTestCoordinates()
        {

            if (_lastGeneratedDay != _todaysSeed)
            {
                LatitudeLongitude playerPosition = _map.MapInformation.ConvertPositionToLatLng(player.position);
                Debug.Log("player position: " + playerPosition.Latitude + ", " + playerPosition.Longitude);

                CoordinateGenerator.GenerateUniquePointsAndWriteToFile(
                    playerPosition.Latitude,
                    playerPosition.Longitude,
                    5000,
                    "Assets/Resources/points.txt",
                    2000,
                    0.3
                );

                //JALLA MEN det funker når vi må være ferdig til i morgen
                // Add retry mechanism for file loading
                TextAsset coordFile = null;
                int maxRetries = 5;
                int currentTry = 0;

                while (coordFile == null && currentTry < maxRetries)
                {
#if UNITY_EDITOR
                    UnityEditor.AssetDatabase.Refresh();
#endif

                    coordFile = Resources.Load<TextAsset>("points");
                    if (coordFile == null)
                    {
                        currentTry++;
                        Debug.Log($"<color=yellow>[MapResourceManager] Waiting for file... Attempt {currentTry}/{maxRetries}</color>");
                        System.Threading.Thread.Sleep(100); // Short delay between attempts
                    }
                }

                if (coordFile == null)
                {
                    Debug.LogError("[MapResourceManager] Could not load coordinates.txt file after multiple attempts!");
                    return;
                }


                clusters = new List<SpawnerCluster>();

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

                GenerateDailyResources();
                Debug.Log($"<color=cyan>[MapResourceManager] Initialized {lines.Length} spawner clusters for new day</color>");
            }

            _isInitialized = true;
        }

        private void GenerateDailyResources()
        {


            _mapResources = GameStateManager.CurrentState.MapResources ?? new Dictionary<int, List<SerializableSpawner>>();
            _mapResources[_todaysSeed] = new List<SerializableSpawner>();

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

            GameStateManager.CurrentState.MapResources = _mapResources;
            _lastGeneratedDay = _todaysSeed;

            Debug.Log($"<color=cyan>[MapResourceManager] Generated {_mapResources[_todaysSeed].Count} resources for today</color>");
        }

        public void AddTestSpawnerCluster(double latitude, double longitude)
        {
            List<Spawner> testSpawners = new List<Spawner>();

            testSpawners.Add(new Spawner
            {
                itemId = GetWeightedRandomItemId(),
                minAmount = 1,
                maxAmount = 1
                //minAmount = random.Next(1, 4),
                //maxAmount = random.Next(3, 6)
            });

            SpawnerCluster newCluster = new SpawnerCluster
            {
                latLng = new LatitudeLongitude(latitude, longitude),
                spawners = testSpawners
            };

            clusters.Add(newCluster);
            //Debug.Log($"<color=cyan>[MapResourceManager] Added test spawner cluster at {latitude}, {longitude} with {numberOfSpawners} spawners</color>");
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
            _lastGeneratedDay = _mapResources?.ContainsKey(_todaysSeed) == true ? _todaysSeed : -1;

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

            var random = _random ?? new Random(_todaysSeed);

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