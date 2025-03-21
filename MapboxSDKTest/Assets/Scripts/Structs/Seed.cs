using System;
using System.Collections.Generic;
using System.Linq;

namespace Structs
{
    public enum SeedRarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary
    }

    public class Seed
    {
        public int ID { get; private set; }
        public SeedRarity Rarity { get; private set; }
        public TimeSpan GrowTime { get; private set; }
        public int Energy { get; private set; }
        public int Water { get; private set; }
        public int Value { get; private set; }

        private static readonly Random random = new();

        public Seed(int id, SeedRarity rarity, TimeSpan growTime, int energy, int water, int value)
        {
            ID = id;
            Rarity = rarity;
            GrowTime = growTime;
            Energy = energy;
            Water = water;
            Value = value;
        }

        public int GetRandomProduct()
        {
            return Rarity switch
            {
                SeedRarity.Common => random.Next(4, 28),      // Products 4-27 (24 different plants)
                SeedRarity.Uncommon => random.Next(28, 44),   // Products 28-43 (16 different plants)
                SeedRarity.Rare => random.Next(44, 51),       // Products 44-50 (7 different plants)
                SeedRarity.Legendary => random.Next(51, 55),  // Products 51-54 (4 different plants)
                _ => throw new ArgumentException("Invalid rarity")
            };
        }
    }

    public static class Seeds
    {
        private static readonly List<Seed> _seedList = new();

        static Seeds()
        {
            // Common seed - 15 minutes, value 5
            _seedList.Add(new Seed(
                id: 0,
                rarity: SeedRarity.Common,
                growTime: TimeSpan.FromMinutes(2),
                energy: 5,
                water: 10,
                value: 5
            ));

            // Uncommon seed - 1 hour, value 25
            _seedList.Add(new Seed(
                id: 1,
                rarity: SeedRarity.Uncommon,
                growTime: TimeSpan.FromMinutes(10),
                energy: 5,
                water: 10,
                value: 25
            ));

            // Rare seed - 3 hours, value 100
            _seedList.Add(new Seed(
                id: 2,
                rarity: SeedRarity.Rare,
                growTime: TimeSpan.FromMinutes(30),
                energy: 5,
                water: 10,
                value: 100
            ));

            // Legendary seed - 6 hours, value 200
            _seedList.Add(new Seed(
                id: 3,
                rarity: SeedRarity.Legendary,
                growTime: TimeSpan.FromHours(1),
                energy: 5,
                water: 10,
                value: 200
            ));
        }

        public static Seed FromID(int id)
        {
            return _seedList.Find(x => x.ID == id);
        }

        public static Seed GetSeedByRarity(SeedRarity rarity)
        {
            return _seedList.First(s => s.Rarity == rarity);
        }
    }
}