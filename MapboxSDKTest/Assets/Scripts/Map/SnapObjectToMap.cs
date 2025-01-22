using Mapbox.BaseModule.Data.Vector2d;
using Mapbox.BaseModule.Map;
using Mapbox.Example.Scripts.Map;
using UnityEngine;

namespace Map
{
    public class SnapObjectToMap : MonoBehaviour
    {
        public LatitudeLongitude latLng;
        public GameObject mapObject;
        private MapboxMapBehaviour _map;

        private void Start()
        {
            _map = mapObject.GetComponent<MapboxMapBehaviour>();
        
            _map.MapInformation.WorldScaleChanged += MapInformationChanged;
            _map.MapInformation.LatitudeLongitudeChanged += MapInformationChanged;
        }
    
        void MapInformationChanged(IMapInformation info)
        {
            transform.position = info.ConvertLatLngToPosition(latLng);
        }
    }
}