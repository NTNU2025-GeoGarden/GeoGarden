using System.Collections.Generic;

namespace Stateful
{
    [System.Serializable]
    public class GameState
    {
        public int Version;
        public List<SerializableInventoryEntry> Inventory;
        public List<SerializableGardenSpot> GardenSpots;
        public List<SerializableObject> Objects;
        public Dictionary<int, List<SerializableSpawner>> MapResources;

        public GameState()
        {
            Version      = 0;
            MapResources = new Dictionary<int, List<SerializableSpawner>>();
            GardenSpots  = new List<SerializableGardenSpot>();
            Objects      = new List<SerializableObject>();
            Inventory    = new List<SerializableInventoryEntry>();
        }
    }
}
