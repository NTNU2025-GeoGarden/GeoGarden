using System.Collections.Generic;

namespace Stateful
{
    [System.Serializable]
    public class GameState
    {
        public int Version;

        public int HouseLevel;
        
        public int CoinCap;
        public int Coins;

        public int EnergyCap;
        public int Energy;

        public int WaterCap;
        public int Water;
        
        public List<SerializableInventoryEntry> Inventory;
        public List<SerializableGardenSpot> GardenSpots;
        public List<SerializableObject> Objects;
        public Dictionary<int, List<SerializableSpawner>> MapResources;
        
        public GameState()
        {
            Version    = 0;
            
            HouseLevel = 0;
            CoinCap    = 0;
            Coins      = 0;
            EnergyCap  = 0;
            Energy     = 0;
            WaterCap   = 0;
            Water      = 0;
            
            MapResources = new Dictionary<int, List<SerializableSpawner>>();
            GardenSpots  = new List<SerializableGardenSpot>();
            Objects      = new List<SerializableObject>();
            Inventory    = new List<SerializableInventoryEntry>();
        }
    }
}
