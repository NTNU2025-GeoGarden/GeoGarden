using System.Collections.Generic;

namespace Stateful
{
    [System.Serializable]
    public class GameState
    {
        public List<SerializableInventoryEntry> Inventory;
        public List<SerializableGardenSpot> GardenSpots;
        public Dictionary<int, List<SerializableSpawner>> MapResources;

        public int Coins;

        public GameState()
        {
            MapResources = new Dictionary<int, List<SerializableSpawner>>();
            GardenSpots = new List<SerializableGardenSpot>();
            Inventory = new List<SerializableInventoryEntry>();
            Coins = 100;
        }
    }
}
