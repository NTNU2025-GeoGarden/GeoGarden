using Structs;

namespace Stateful
{
    [System.Serializable]
    public struct SerializableGardenSpot
    {
        public GrowState state;
        public int seedID;
    }
}