using Mapbox.BaseModule.Map;
using Mapbox.BaseModule.Utilities;
using Mapbox.Example.Scripts.Map;
using Mapbox.LocationModule;
using UnityEngine;

namespace Mapbox.Example.Scripts.LocationBehaviours
{
    public class SnapTransformToLocationProvider : MonoBehaviour
    {
        public Transform _transform;
        
        [SerializeField]
        private LocationProviderFactory _locationProvider;
        [SerializeField] private MapBehaviourCore _mapCore;
        private MapboxMap _map;
        
        private void Start()
        {
            if(_transform == null)
                _transform = transform;
        
            if(_locationProvider == null)
                Debug.Log("_locationProvider null");
        
            if(_locationProvider.DefaultLocationProvider == null)
                Debug.Log("DefaultLocationProvider null");
        
            if(!enabled || _locationProvider == null || _locationProvider.DefaultLocationProvider == null)
                return;
            
            _mapCore.Initialized += (map) =>
            {
                _map = map;
                map.OnFirstViewCompleted += () =>
                {
                    _locationProvider.DefaultLocationProvider.OnLocationUpdated += OnDefaultLocationProviderOnOnLocationUpdated;
                };
            };
        }

        private void OnDefaultLocationProviderOnOnLocationUpdated(Location s)
        {
            if (_map.Status >= InitializationStatus.ReadyForUpdates && enabled)
            {
                _transform.position = _map.mapInformation.ConvertLatLngToPosition(s.LatitudeLongitude);
            }
        }
    }
}
