using Map;
using Mapbox.BaseModule.Data.Vector2d;

namespace Stateful
{
    [System.Serializable]
    public struct SerializableSpawner
    {
        public LatitudeLongitude latLng;
        public Spawner spawner;
        public bool collected;
    }
}
