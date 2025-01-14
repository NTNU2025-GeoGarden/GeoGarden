using System;
using System.Collections.Generic;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject mapObject;
    public GameObject resourcePrefab;
    public List<LatitudeLongitude> resourceLocations;

    private MapboxMapBehaviour _map;

    void Start()
    {
        _map = mapObject.GetComponent<MapboxMapBehaviour>();


        foreach (LatitudeLongitude latLong in resourceLocations)
        {
            GameObject newObj = Instantiate(resourcePrefab, _map.MapInformation.ConvertLatLngToPosition(latLong),
                Quaternion.identity);

            newObj.GetComponent<SnapResourceToMap>().latLong = latLong;
            newObj.GetComponent<SnapResourceToMap>().mapObject = mapObject;
        }
    }
}
