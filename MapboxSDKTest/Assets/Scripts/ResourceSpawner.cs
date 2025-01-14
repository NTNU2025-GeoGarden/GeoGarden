using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject mapObject;
    public GameObject resourcePrefab;
    public List<LatitudeLongitude> resourceLocations;

    private MapboxMapBehaviour _map;

    void Start()
    {
        _map = mapObject.GetComponent<MapboxMapBehaviour>();
    }
    
        // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int index = Random.Range(0, resourceLocations.Count);
            Instantiate(resourcePrefab, _map.MapInformation.ConvertLatLngToPosition(resourceLocations[index]),
                Quaternion.identity);
        }
    }
}
