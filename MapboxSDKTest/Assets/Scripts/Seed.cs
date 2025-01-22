using System.Collections.Generic;
using Mono.Collections.Generic;

public class Seed
{
    public int ID { get; private set; }
    public int ProductItemID { get; private set; }

    public Seed(int id, int product)
    {
        ID = id;
        ProductItemID = product;
    }
}

public abstract class Seeds
{
    private static List<Seed> _seedList = new(new Collection<Seed>()
    {
        new(0, 4)
    });

    public static Seed FromID(int id)
    {
        return _seedList.Find(x => x.ID == id);
    }
}