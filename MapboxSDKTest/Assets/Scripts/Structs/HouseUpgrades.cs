using System;
using System.Collections.Generic;

namespace Structs
{
    public static class HouseUpgrades
    {
        public static readonly int MaxLevel = 4;
        public static readonly List<int> CoinCapPerLevel = new() { -1, 700, 2000, 8000, 24000 };
        public static readonly List<int> EnergyCapPerLevel = new() { -1, 100, 200, 400, 10000 };
        public static readonly List<int> WaterCapPerLevel = new() { -1, 50, 100, 200, 400 };
        public static readonly List<int> NewSpotsPerLevel = new() { -1, 2, 2, 2, 2 };

        public static readonly int MaxCoinCap = CoinCapPerLevel[^1];
        public static readonly int MaxEnergyCap = EnergyCapPerLevel[^1];
        public static readonly int MaxWaterCap = WaterCapPerLevel[^1];

        public static readonly List<string> UpgradeTextPerLevel = new()
        {
            "",
            "+ 2 more planting zone\n+ Unlock garden layout",
            "+ 2 more planting zones\n+ Unlock 3 new garden layout objects",
            "+ 2 more planting zones",
            "+ 2 more planting zones"
        };

        public static readonly List<int> PlantRequirementPerLevel = new() { -1, 5, 15, 60, 180 };
        public static readonly List<int> WalkingRequirementPerLevel = new() { -1, 3, 15, 42, 100 };
        public static readonly List<int> UpgradeCost = new() { -1, 350, 1000, 4000, 12000 };

        public static readonly List<TimeSpan> UpgradeTimePerLevel = new() { TimeSpan.Zero, TimeSpan.FromHours(3), TimeSpan.FromHours(6), TimeSpan.FromHours(12), TimeSpan.FromDays(1) };
    }
}