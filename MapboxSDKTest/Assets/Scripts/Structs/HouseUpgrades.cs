using System;
using System.Collections.Generic;

namespace Structs
{
    public static class HouseUpgrades
    {
        public static readonly int MaxLevel = 4;
        public static readonly List<int> CoinCapPerLevel   = new(){-1, 100, 175, 300, 500};
        public static readonly List<int> EnergyCapPerLevel = new(){-1,  25,  50,  75, 150};
        public static readonly List<int> WaterCapPerLevel  = new(){-1,  50, 100, 200, 400};
        public static readonly List<int> NewSpotsPerLevel  = new(){-1,   1,   2,   3,   4};

        public static readonly int MaxCoinCap   = CoinCapPerLevel[^1];
        public static readonly int MaxEnergyCap = EnergyCapPerLevel[^1];
        public static readonly int MaxWaterCap  = WaterCapPerLevel[^1];

        public static readonly List<string> UpgradeTextPerLevel = new()
        {
            "", 
            "+ 1 more planting zone\n+ 10% faster growing\n+ Unlock garden layout", 
            "+ 2 more planting zones\n+ Unlock 3 new garden layout objects\n+ Map locations give more loot", 
            "+ 3 more planting zones\n+ 15% faster growing\n+ 1x random <color=#fc4e03>Epic</color> seed",
            "+ 4 more planting zones\n+ 1x random <color=#ffba19>Legendary</color> seed"
        };
        
        public static readonly List<int> PlantRequirementPerLevel    = new(){-1, 10,  30,  50, 100};
        public static readonly List<int> WalkingRequirementPerLevel  = new(){-1,  3,  10,  21,  42};
        public static readonly List<int> UpgradeCost                 = new(){-1, 75, 150, 275, 400};
        
        public static readonly List<TimeSpan> UpgradeTimePerLevel = new(){TimeSpan.Zero, TimeSpan.FromDays(1), TimeSpan.FromHours(31), TimeSpan.FromDays(2), TimeSpan.FromDays(3)};
    }
}