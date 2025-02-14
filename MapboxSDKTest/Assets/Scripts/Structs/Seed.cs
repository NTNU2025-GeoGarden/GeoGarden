using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Structs
{
    public class Seed
    {
        public int ID { get; private set; }
        public int ProductItemID { get; private set; }

        public int Energy {get; private set;}

        public int Water {get; private set;}

        public Seed(int id, int product, int energy, int water)
        {
            ID = id;
            ProductItemID = product;
            Energy = energy;
            Water = water;
        }
    }

    public abstract class Seeds
    {
        private static List<Seed> _seedList = new(new Collection<Seed>
        {
            new(0, 4, 15,15)
        });

        public static Seed FromID(int id)
        {
            return _seedList.Find(x => x.ID == id);
        }
    }
}