using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Structs
{
    public enum ItemType
    {
        Seed,
        Water,
        Fertilizer,
        Generic
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Legendary,
        Special
    }

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
        public ItemType Type      { get; private set; }
        public Rarity Rarity      { get; private set; }
        public int AppendID       { get; private set; }
    
        public Item(int id, string name, string description, ItemType type, Rarity rarity, int appendID)
        {
            ID = id;
            Name = name;
            Description = description;
            Type = type;
            Rarity = rarity;
            AppendID = appendID;
        }
    }

    public static class Items
    {
        private static List<Item> _itemList = new(
            new Collection<Item>
            {
                new(0, "Debug Seed", "Variant of common",    ItemType.Seed, Rarity.Common, 0),
                new(1, "Debug Seed", "Variant of uncommon",  ItemType.Seed, Rarity.Uncommon, 0),
                new(2, "Debug Seed", "Variant of rare",      ItemType.Seed, Rarity.Rare, 0),
                new(3, "Debug Seed", "Variant of legendary", ItemType.Seed, Rarity.Legendary, 0),
                new(4, "Debug Item", "Some product?",        ItemType.Generic, Rarity.Common, -1)
            });

        public static Item FromID(int id)
        {
            return _itemList.Find(x => x.ID == id);
        }
    }
}