using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;
using Random = System.Random;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject mapObject;
    public GameObject resourcePrefab;
    public List<ResourceCluster> clusters;

    private MapboxMapBehaviour _map;

    void Start()
    {
        _map = mapObject.GetComponent<MapboxMapBehaviour>();

        Random random = new(DateTime.Today.DayOfYear);

        foreach (ResourceCluster cluster in clusters)
        {
            if (!(random.NextDouble() > 0.5)) continue;
            
            GameObject newObj = Instantiate(resourcePrefab, _map.MapInformation.ConvertLatLngToPosition(cluster.latLng),
                Quaternion.identity);
                
            newObj.GetComponent<SnapResourceToMap>().latLong = cluster.latLng;
            newObj.GetComponent<SnapResourceToMap>().mapObject = mapObject;

            int spawnerIndex = random.Next(cluster.spawners.Count);

            newObj.GetComponent<MapResource>().spawner = cluster.spawners[spawnerIndex];
        }
    }
}