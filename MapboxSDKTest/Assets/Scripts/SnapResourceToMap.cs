using System;
using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

public class SnapResourceToMap : MonoBehaviour
{
    public LatitudeLongitude latLong;
    public GameObject mapObject;
    private MapboxMapBehaviour _map;

    private void Start()
    {
        _map = mapObject.GetComponent<MapboxMapBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        _map.MapInformation.WorldScaleChanged += MapInformationChanged;
        _map.MapInformation.LatitudeLongitudeChanged += MapInformationChanged;
        
        return;

        void MapInformationChanged(IMapInformation info)
        {
            transform.position = info.ConvertLatLngToPosition(latLong);
        }
    }
}