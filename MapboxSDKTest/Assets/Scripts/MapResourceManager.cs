using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using Persistence;
using UnityEngine;
using Random = System.Random;

public class MapResourceManager : MonoBehaviour, IPersistence
{
    public delegate void RegisterCollectResource(LatitudeLongitude latLng);
    public static RegisterCollectResource OnRegisterCollectResource;
    
    public GameObject mapObject;
    public GameObject resourcePrefab;
    public Transform player;
    public List<ResourceCluster> clusters;
    
    private MapboxMapBehaviour _map;
    private int _todaysSeed;
    private Dictionary<int, List<SavedMapResource>> _mapResources;
    
    void Start()
    {
        OnRegisterCollectResource += TryRegisterCollectResource;
        
        _map = mapObject.GetComponent<MapboxMapBehaviour>();

        _map.MapServiceReady += service =>
        {
            AddInGameSpawners();
        };
    }

    public void LoadData(GameState state)
    {
        _todaysSeed = DateTime.Now.Year * 1000 + DateTime.Now.DayOfYear;
        _mapResources = state.mapResources;
        
        if (_mapResources.ContainsKey(_todaysSeed))
        {
            print("Resource spawner already has loaded data..");
            return;
        }
        
        _mapResources.Add(_todaysSeed, new List<SavedMapResource>());
        
        // No data generated, fix that!
        
        Random random = new(_todaysSeed);

        foreach (ResourceCluster cluster in clusters)
        {
            if (!(random.NextDouble() > 0.5)) continue;

            SavedMapResource newResource = new()
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
        state.mapResources = _mapResources;
    }

    private void AddInGameSpawners()
    {
        if (_mapResources == null || !_mapResources.ContainsKey(_todaysSeed))
        {
            Debug.LogError("Map resources tried to generate, but today's seed is not found. This should never happen!");
            return;
        }
        
        foreach (SavedMapResource resource in _mapResources[_todaysSeed])
        {
            GameObject newObj = Instantiate(resourcePrefab, _map.MapInformation.ConvertLatLngToPosition(resource.latLng),
                Quaternion.identity);
                
            newObj.GetComponent<SnapResourceToMap>().latLng    = resource.latLng;
            newObj.GetComponent<SnapResourceToMap>().mapObject = mapObject;

            newObj.GetComponent<MapResource>().player    = player;
            newObj.GetComponent<MapResource>().spawner   = resource.spawner;
            newObj.GetComponent<MapResource>().latLng    = resource.latLng;
            newObj.GetComponent<MapResource>().collected = resource.collected;
        }
    }

    public void TryRegisterCollectResource(LatitudeLongitude latLng)
    {
        print($"Tried to collect resource @ {latLng}");

        SavedMapResource resource = _mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng));
        resource.collected = true;
        _mapResources[_todaysSeed].Remove(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)));
        _mapResources[_todaysSeed].Add(resource);
        
        print(_mapResources[_todaysSeed].Find(p => p.latLng.Equals(latLng)).collected);
    }
}