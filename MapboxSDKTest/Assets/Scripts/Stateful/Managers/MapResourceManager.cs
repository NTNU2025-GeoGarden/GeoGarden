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

        foreach (SerializableSpawner resource in _mapResources[_todaysSeed])
        {
            Vector3 spawnPosition = _map.MapInformation.ConvertLatLngToPosition(resource.latLng);
            spawnPosition.y = 45f; // ✅ Set a fixed Y-value for all spawners

            GameObject newObj = Instantiate(resourcePrefab, spawnPosition, Quaternion.identity);
            
            newObj.GetComponent<SnapObjectToMap>().latLng    = resource.latLng;
            newObj.GetComponent<SnapObjectToMap>().mapObject = mapObject;

            newObj.GetComponent<SpawnerOnMap>().player    = player;
            newObj.GetComponent<SpawnerOnMap>().spawner   = resource.spawner;
            newObj.GetComponent<SpawnerOnMap>().latLng    = resource.latLng;
            newObj.GetComponent<SpawnerOnMap>().collected = resource.collected;
        }
       }

        /*

        public void TryRegisterCollectResource(LatitudeLongitude latLng)
        {
            Debug.Log("<color=lime>[MapResourceManager] Trying to register that the player collected a resource</color>");
            SerializableSpawner resource = _mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng));
            resource.collected = true;
            _mapResources[_todaysSeed].Remove(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)));
            _mapResources[_todaysSeed].Add(resource);
        
            print(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)).collected);
        } */

     
       public async void TryRegisterCollectResource(LatitudeLongitude latLng)
    {
        Debug.Log("<color=lime>[MapResourceManager] Trying to register that the player collected a resource</color>");

            // ✅ Check if _mapResources is null
        if (_mapResources == null)
        {
            Debug.Log("⏳ Waiting for _mapResources to be initialized...");
            await Task.Delay(500); // ✅ Wait for 100 milliseconds before checking again
        }

        // ✅ Check if _todaysSeed exists
        if (!_mapResources.ContainsKey(_todaysSeed))
        {
            Debug.LogError($"❌ ERROR: _mapResources does NOT contain the key '{_todaysSeed}'!");
            return;
        }

        // ✅ Check if the list is null
        if (_mapResources[_todaysSeed] == null)
        {
            Debug.LogError($"❌ ERROR: _mapResources[{_todaysSeed}] is NULL!");
            return;
        }

        // ✅ Find the index
        int index = _mapResources[_todaysSeed].FindIndex(p => p.latLng.Equals(latLng));

        if (index == -1)
        {
            Debug.LogError($"❌ ERROR: No resource found at latLng {latLng} in _mapResources[{_todaysSeed}]!");
            return;
        }

        // ✅ Mark the resource as collected
        _mapResources[_todaysSeed][index].collected = true;
        Debug.Log($"✅ Resource at {latLng} marked as collected.");
        return;
    }

    }
}