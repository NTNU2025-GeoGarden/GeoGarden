using Mapbox.BaseModule.Data.Vector2d;

namespace Persistence
{
    [System.Serializable]
    public struct SavedMapResource
    {
        public LatitudeLongitude latLng;
        public ResourceSpawn spawner;
        public bool collected;
    }
}
