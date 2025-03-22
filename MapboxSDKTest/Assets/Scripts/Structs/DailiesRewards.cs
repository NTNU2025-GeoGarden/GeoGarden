using System.Collections.Generic;

namespace Structs
{
    public static class DailiesRewards
    {
        public struct Reward
        {
            public int Coins;
            public int Water;
            public int ItemID;
        }

        public static List<Reward> Rewards = new(new[]
        {
            new Reward { Coins = 10,  Water = 5,  ItemID = -1},
            new Reward { Coins = 20,  Water = 5,  ItemID = -1},
            new Reward { Coins = 20,  Water = 5,  ItemID = -1},
            new Reward { Coins = 40,  Water = 5,  ItemID = -1},
            new Reward { Coins = 40,  Water = 10, ItemID =  1},
            new Reward { Coins = 80,  Water = 10, ItemID = -1},
            new Reward { Coins = 80,  Water = 10, ItemID =  2},
            new Reward { Coins = 160, Water = 10, ItemID = -1},
            new Reward { Coins = 160, Water = 15, ItemID = -1},
            new Reward { Coins = 320, Water = 15, ItemID = -1},
            new Reward { Coins = 320, Water = 15, ItemID = -1},
            new Reward { Coins = 640, Water = 15, ItemID =  1},
            new Reward { Coins = 640, Water = 20, ItemID = -1},
            new Reward { Coins = 1280,Water = 20, ItemID =  3}
        });
    }
}