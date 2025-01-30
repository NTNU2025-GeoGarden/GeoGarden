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
        public delegate void RegisterCollectResource(LatitudeLongitude latLng);
        public static RegisterCollectResource OnRegisterCollectResource;
    
        public GameObject mapObject;
        public GameObject resourcePrefab;
        public Transform player;
        public List<SpawnerCluster> clusters;
    
        private MapboxMapBehaviour _map;
        private int _todaysSeed;
        private Dictionary<int, List<SerializableSpawner>> _mapResources;
    
        void Start()
        {
            OnRegisterCollectResource += TryRegisterCollectResource;
        
            _map = mapObject.GetComponent<MapboxMapBehaviour>();

            _map.MapServiceReady += _ =>
            {
                AddInGameSpawners();
            };
        }

        public void LoadData(GameState state)
        {
            Debug.Log("<color=cyan>[ResourceManager] Loading spawner data</color>");
            
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
        
            Random random = new(_todaysSeed);

            foreach (SpawnerCluster cluster in clusters)
            {
                if (!(random.NextDouble() > 0.5)) continue;

                SerializableSpawner newResource = new()
                {
                    latLng = cluster.latLng,
                    spawner = cluster.spawners[random.Next(cluster.spawners.Count)],
                    collected = false
                };
            
                _mapResources[_todaysSeed].Add(newResource);
            }
        }

        public void SaveData(ref GameState state)
        {
            state.MapResources = _mapResources;
        }

        private void AddInGameSpawners()
        {
            if (_mapResources == null || !_mapResources.ContainsKey(_todaysSeed))
            {
                Debug.LogError("[MapResourceManager] Map resources tried to generate, but today's seed is not found. This should never happen!");
                LoadingScreen.OnLoadingError();
                return;
            }
        
            foreach (SerializableSpawner resource in _mapResources[_todaysSeed])
            {
                GameObject newObj = Instantiate(resourcePrefab, _map.MapInformation.ConvertLatLngToPosition(resource.latLng),
                    Quaternion.identity);
                
                newObj.GetComponent<SnapObjectToMap>().latLng    = resource.latLng;
                newObj.GetComponent<SnapObjectToMap>().mapObject = mapObject;

                newObj.GetComponent<SpawnerOnMap>().player    = player;
                newObj.GetComponent<SpawnerOnMap>().spawner   = resource.spawner;
                newObj.GetComponent<SpawnerOnMap>().latLng    = resource.latLng;
                newObj.GetComponent<SpawnerOnMap>().collected = resource.collected;
            }
        }

        public void TryRegisterCollectResource(LatitudeLongitude latLng)
        {
            SerializableSpawner resource = _mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng));
            resource.collected = true;
            _mapResources[_todaysSeed].Remove(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)));
            _mapResources[_todaysSeed].Add(resource);
        
            print(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)).collected);
        }
    }
}