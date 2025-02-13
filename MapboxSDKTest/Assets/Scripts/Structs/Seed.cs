using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Structs
{
    public class Seed
    {
        public int ID { get; private set; }
        public int ProductItemID { get; private set; }
        public TimeSpan GrowTime { get; private set; }

        public Seed(int id, int product, TimeSpan growTime)
        {
            ID = id;
            ProductItemID = product;
            GrowTime = growTime;
        }
    }

    public abstract class Seeds
    {
        private static List<Seed> _seedList = new(new Collection<Seed>
        {
            new(0, 4, TimeSpan.FromMinutes(3))
        });

        public static Seed FromID(int id)
        {
            return _seedList.Find(x => x.ID == id);
        }
    }
}