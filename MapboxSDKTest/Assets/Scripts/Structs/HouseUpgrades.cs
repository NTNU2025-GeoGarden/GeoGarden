using System.Collections.Generic;

namespace Structs
{
    public static class HouseUpgrades
    {
        public static List<int> CoinCapPerLevel   = new(){-1, 100, 175, 300, 500};
        public static List<int> EnergyCapPerLevel = new(){-1,  25,  50,  75, 150};
        public static List<int> WaterCapPerLevel  = new(){-1,  50, 100, 200, 400};

        public static int MaxCoinCap   = CoinCapPerLevel[^1];
        public static int MaxEnergyCap = EnergyCapPerLevel[^1];
        public static int MaxWaterCap  = WaterCapPerLevel[^1];

        public static List<string> UpgradeTextPerLevel = new() {"", "+ 2 new planting zones\n+ 10% faster grow speed\n+ 1x random <color=#ffba19>Legendary</color> seed", "", "", ""};
        
        public static List<int> PlantRequirementPerLevel    = new(){-1, 10, 30, 50, 100};
        public static List<int> KmWalkedRequirementPerLevel = new(){-1,  3, 10, 21,  42};
        
        public static List<int> UpgradeCost = new(){-1, 75, 150, 275, 400};
    }
}