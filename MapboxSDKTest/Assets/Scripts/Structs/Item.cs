using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Structs
{
    public enum ItemType
    {
        Seed,
        Produce
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
        public int Value          { get; private set; } // New property for the item's value
    
        public Item(int id, string name, string description, ItemType type, Rarity rarity, int appendID, int value = 300)
        {
            ID = id;
            Name = name;
            Description = description;
            Type = type;
            Rarity = rarity;
            AppendID = appendID;
            Value = value; // Assign value with default of 300
        }
    }

    public static class Items
    {
        private static List<Item> _itemList = new(
            new Collection<Item>
            {
                new(0, "Common seed", "Variant of common",    ItemType.Seed, Rarity.Common, 0, 5),
                new(1, "Uncommon seed", "Variant of uncommon",  ItemType.Seed, Rarity.Uncommon, 0, 25),
                new(2, "Rare seed", "Variant of rare",      ItemType.Seed, Rarity.Rare, 0, 100),
                new(3, "Legendary seed", "Variant of legendary", ItemType.Seed, Rarity.Legendary, 0 , 200),

                new(4, "Tomato", "Tomato", ItemType.Produce, Rarity.Common, -1, 20),
                new(5, "Cucumber", "Cucumber", ItemType.Produce, Rarity.Common, -1, 15),
                new(6, "Potato", "Potato", ItemType.Produce, Rarity.Common, -1, 17),
                new(7, "Lettuce", "Lettuce", ItemType.Produce, Rarity.Common, -1, 19),
                new(8, "Celery", "Celery", ItemType.Produce, Rarity.Common, -1, 22),
                new(9, "Dill", "Dill", ItemType.Produce, Rarity.Common, -1, 21),
                new(10, "Leek", "Leek", ItemType.Produce, Rarity.Common, -1, 21),
                new(11, "Onion", "Onion", ItemType.Produce, Rarity.Common, -1, 22),
                new(12, "Spring Onion", "Spring Onion", ItemType.Produce, Rarity.Common, -1, 23),
                new(13, "Cauliflower", "Cauliflower", ItemType.Produce, Rarity.Common, -1, 18),
                new(14, "Broccoli", "Broccoli", ItemType.Produce, Rarity.Common, -1, 18),
                new(15, "Artichoke", "Artichoke", ItemType.Produce, Rarity.Common, -1, 16),
                new(16, "Daikon", "Daikon", ItemType.Produce, Rarity.Common, -1, 16),
                new(17, "Rice", "Rice", ItemType.Produce, Rarity.Common, -1, 14),
                new(18, "Sugarcane", "Sugarcane", ItemType.Produce, Rarity.Common, -1, 23),
                new(19, "Soybean", "Soybean", ItemType.Produce, Rarity.Common, -1, 15),
                new(20, "Wheat", "Wheat", ItemType.Produce, Rarity.Common, -1, 16),
                new(21, "Cotton", "Cotton", ItemType.Produce, Rarity.Common, -1, 18),
                new(22, "Sweet Potato", "Sweet Potato", ItemType.Produce, Rarity.Common, -1, 20),
                new(23, "Cabbage", "Cabbage", ItemType.Produce, Rarity.Common, -1, 16),
                new(24, "Spinach", "Spinach", ItemType.Produce, Rarity.Common, -1, 14),
                new(25, "Coffee Bean", "Coffee Bean", ItemType.Produce, Rarity.Common, -1, 25),
                new(26, "Corn", "Corn", ItemType.Produce, Rarity.Common, -1, 12),
                new(27, "Eggplant", "Eggplant", ItemType.Produce, Rarity.Common, -1, 20),

                new(28, "Apple", "Apple", ItemType.Produce, Rarity.Uncommon, -1, 150),
                new(29, "Strawberry", "Strawberry", ItemType.Produce, Rarity.Uncommon, -1, 130),
                new(30, "Blackberry", "Blackberry", ItemType.Produce, Rarity.Uncommon, -1, 100),
                new(31, "Lingonberry", "Lingonberry", ItemType.Produce, Rarity.Uncommon, -1, 85),
                new(32, "Raspberry", "Raspberry", ItemType.Produce, Rarity.Uncommon, -1, 93),
                new(33, "Blackcurrant", "Blackcurrant", ItemType.Produce, Rarity.Uncommon, -1, 100),
                new(34, "Apricot", "Apricot", ItemType.Produce, Rarity.Uncommon, -1, 95),
                new(35, "Dragonfruit", "Dragonfruit", ItemType.Produce, Rarity.Uncommon, -1, 155),
                new(36, "Mango", "Mango", ItemType.Produce, Rarity.Uncommon, -1, 160),
                new(37, "Lime", "Lime", ItemType.Produce, Rarity.Uncommon, -1, 88),
                new(38, "Kiwi", "Kiwi", ItemType.Produce, Rarity.Uncommon, -1, 76),
                new(39, "Edamame", "Edamame", ItemType.Produce, Rarity.Uncommon, -1, 78),
                new(40, "Orange", "Orange", ItemType.Produce, Rarity.Uncommon, -1, 92),
                new(41, "Papaya", "Papaya", ItemType.Produce, Rarity.Uncommon, -1, 67),
                new(42, "Pear", "Pear", ItemType.Produce, Rarity.Uncommon, -1, 87),
                new(43, "Pineapple", "Pineapple", ItemType.Produce, Rarity.Uncommon, -1, 170),

                new(44, "Wasabi", "Wasabi", ItemType.Produce, Rarity.Rare, -1, 500),
                new(45, "Saffron", "Saffron", ItemType.Produce, Rarity.Rare, -1, 650),
                new(46, "Vanilla", "Vanilla", ItemType.Produce, Rarity.Rare, -1, 450),
                new(47, "Cinnamon", "Cinnamon", ItemType.Produce, Rarity.Rare, -1, 400),
                new(48, "Anise", "Anise", ItemType.Produce, Rarity.Rare, -1, 600),
                new(49, "Nutmeg", "Nutmeg", ItemType.Produce, Rarity.Rare, -1, 600),
                new(50, "Cumin", "Cumin", ItemType.Produce, Rarity.Rare, -1, 540),

                new(51, "Pumpkin", "Pumpkin", ItemType.Produce, Rarity.Legendary, -1, 1900),
                new(52, "Watermelon", "Watermelon", ItemType.Produce, Rarity.Legendary, -1, 2200),
                new(53, "Coconut", "Coconut", ItemType.Produce, Rarity.Legendary, -1, 2500),
                new(54, "Durian", "Durian", ItemType.Produce, Rarity.Legendary, -1, 2300),

            });

        public static Item FromID(int id)
        {
            return _itemList.Find(x => x.ID == id);
        }
    }
}
