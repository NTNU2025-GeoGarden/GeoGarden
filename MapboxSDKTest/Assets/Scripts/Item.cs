using System.Collections.Generic;
using Mono.Collections.Generic;

public class InventoryItem
{
    public Item Item { get; private set; }
    public int Amount { get; set; }
    
    public InventoryItem(int id, int amount)
    {
        Item = Items.FromID(id);
        Amount = amount;
    }
}
public class Item
{
    public int ID             { get; private set; }
    public string Name        { get; private set; }
    public string Description { get; private set; }
    public ResourceType Type  { get; private set; }
    public Quality Quality    { get; private set; }
    
    public Item(int id, string name, string description, ResourceType type, Quality quality)
    {
        ID = id;
        Name = name;
        Description = description;
        Type = type;
        Quality = quality;
    }
}

public static class Items
{
    public static List<Item> ItemList = new(
        new Collection<Item>
        {
            new(0, "Debug Seed", "Variant of common",   ResourceType.Seed, Quality.Common),
            new(1, "Debug Seed", "Variant of uncommon", ResourceType.Seed, Quality.Uncommon),
            new(2, "Debug Seed", "Variant of rare",     ResourceType.Seed, Quality.Rare),
            new(3, "Debug Seed", "Variant of legendary",ResourceType.Seed, Quality.Legendary)
        });

    public static Item FromID(int id)
    {
        return ItemList.Find(x => x.ID == id);
    }
}