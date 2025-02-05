using Map;
using Mapbox.BaseModule.Data.Vector2d;
using UnityEngine;

namespace Stateful
{
    [System.Serializable]
    public class SerializableSpawner
    {
        public LatitudeLongitude latLng;
        public Spawner spawner;
        public bool collected;
    }
}
