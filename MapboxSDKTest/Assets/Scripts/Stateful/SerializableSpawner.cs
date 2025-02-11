using Map;
using Mapbox.BaseModule.Data.Vector2d;

namespace Stateful
{
    public class SerializableSpawner
    {
        public LatitudeLongitude latLng;
        public Spawner spawner;
        public bool collected;
    }
}
