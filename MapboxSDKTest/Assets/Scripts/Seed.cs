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
    public static List<Seed> SeedList = new(new Collection<Seed>()
    {
        new(0, 4)
    });
}