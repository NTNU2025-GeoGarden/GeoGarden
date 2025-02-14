using System;
using System.Collections.Generic;

namespace Stateful
{
    [Serializable]
    public class GameState
    {
        public int Version;
        public string UID;

        public bool GardenTutorial;
        public bool MapTutorial;
        public bool LayoutTutorial;

        public int HouseLevel;
        public DateTime LevelUpTime;
        
        public int CoinCap;
        public int Coins;

        public int EnergyCap;
        public int Energy;

        public int WaterCap;
        public int Water;

        public int PlantsHarvested;
        public float DistanceWalked;
        
        public List<SerializableInventoryEntry> Inventory;
        public List<SerializableGardenSpot> GardenSpots;
        public List<SerializableObject> Objects;
        public Dictionary<int, List<SerializableSpawner>> MapResources;
        
        public GameState()
        {
            Version        = 0;
            UID            = "";
            GardenTutorial  = false;
            MapTutorial  = false;
            LayoutTutorial = false;
            
            LevelUpTime = DateTime.MinValue;
            
            HouseLevel = 0;
            CoinCap    = 0;
            Coins      = 0;
            EnergyCap  = 0;
            Energy     = 0;
            WaterCap   = 0;
            Water      = 0;
            PlantsHarvested = 0;
            DistanceWalked  = 0;
            
            MapResources = new Dictionary<int, List<SerializableSpawner>>();
            GardenSpots  = new List<SerializableGardenSpot>();
            Objects      = new List<SerializableObject>();
            Inventory    = new List<SerializableInventoryEntry>();
        }
    }
}
