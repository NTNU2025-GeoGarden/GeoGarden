using System.Collections.Generic;

namespace Stateful
{
    [System.Serializable]
    public class GameState
    {
        public List<(int id, int amount)> Inventory;
        public List<SerializableGardenSpot> GardenSpots;
        public Dictionary<int, List<SerializableSpawner>> MapResources;

        public GameState()
        {
            MapResources = new Dictionary<int, List<SerializableSpawner>>();
            GardenSpots = new List<SerializableGardenSpot>();
            Inventory = new List<(int id, int amount)>();
        }
    }
}
