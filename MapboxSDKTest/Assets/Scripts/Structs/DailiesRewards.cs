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

        public static List<Reward> Rewards = new(new []
        {
            new Reward { Coins = 5,  Water = 0,  ItemID = -1},
            new Reward { Coins = 5,  Water = 0,  ItemID = -1},
            new Reward { Coins = 10, Water = 5,  ItemID = -1},
            new Reward { Coins = 10, Water = 0,  ItemID = -1},
            new Reward { Coins = 5,  Water = 5,  ItemID =  1},
            new Reward { Coins = 5,  Water = 10, ItemID = -1},
            new Reward { Coins = 10, Water = 15, ItemID =  2},
            new Reward { Coins = 5,  Water = 0,  ItemID = -1},
            new Reward { Coins = 10, Water = 0,  ItemID = -1},
            new Reward { Coins = 10, Water = 5,  ItemID = -1},
            new Reward { Coins = 10, Water = 5,  ItemID = -1},
            new Reward { Coins = 10, Water = 10, ItemID =  1},
            new Reward { Coins = 10, Water = 10, ItemID = -1},
            new Reward { Coins = 20, Water = 15, ItemID =  3}
        });
    }
}