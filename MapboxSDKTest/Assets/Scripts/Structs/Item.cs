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
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Description { get; private set; }
        public ItemType Type { get; private set; }
        public Rarity Rarity { get; private set; }
        public int AppendID { get; private set; }
        public int Value { get; private set; } // New property for the item's value

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
        public static List<Item> ItemList = new(
            new Collection<Item>
            {
                new(0, "Common seed", "A pack of seeds, I wonder what you will grow into..",    ItemType.Seed, Rarity.Common, 0, 1),
                new(1, "Uncommon seed", "A pack of seeds, I wonder what you will grow into..",  ItemType.Seed, Rarity.Uncommon, 0, 5),
                new(2, "Rare seed", "A pack of seeds, I wonder what you will grow into..",      ItemType.Seed, Rarity.Rare, 0, 25),
                new(3, "Legendary seed", "A pack of seeds, I wonder what you will grow into..", ItemType.Seed, Rarity.Legendary, 0 , 100),

                new(4, "Tomato", "Juicy and vibrant, perfect for sauces and salads.", ItemType.Produce, Rarity.Common, -1, 20),
                new(5, "Cucumber", "Cool and crisp, a refreshing garden favorite.", ItemType.Produce, Rarity.Common, -1, 15),
                new(6, "Potato", "A humble tuber with endless culinary possibilities.", ItemType.Produce, Rarity.Common, -1, 17),
                new(7, "Lettuce", "Crisp and leafy, the base of any great salad.", ItemType.Produce, Rarity.Common, -1, 19),
                new(8, "Celery", "Crunchy and aromatic, great for soups or snacks.", ItemType.Produce, Rarity.Common, -1, 22),
                new(9, "Dill", "A fragrant herb that adds a burst of flavor.", ItemType.Produce, Rarity.Common, -1, 9),
                new(10, "Leek", "Mild and onion-like, ideal for hearty dishes.", ItemType.Produce, Rarity.Common, -1, 21),
                new(11, "Onion", "A kitchen essential, bringing depth to any meal.", ItemType.Produce, Rarity.Common, -1, 22),
                new(12, "Spring Onion", "Tender and zesty, a fresh garnish for any dish.", ItemType.Produce, Rarity.Common, -1, 23),
                new(13, "Cauliflower", "A versatile veggie, great roasted or riced.", ItemType.Produce, Rarity.Common, -1, 18),
                new(14, "Broccoli", "A nutritious green packed with crunch and flavor.", ItemType.Produce, Rarity.Common, -1, 18),
                new(15, "Artichoke", "A spiky treasure with a delicious heart.", ItemType.Produce, Rarity.Common, -1, 16),
                new(16, "Daikon", "A crisp, mild radish popular in Asian cuisine.", ItemType.Produce, Rarity.Common, -1, 16),
                new(17, "Rice", "Tiny grains that fuel the world's favorite dishes.", ItemType.Produce, Rarity.Common, -1, 14),
                new(18, "Sugarcane", "Sweet and fibrous, the source of pure sugar.", ItemType.Produce, Rarity.Common, -1, 23),
                new(19, "Soybean", "A protein-rich bean used in countless foods.", ItemType.Produce, Rarity.Common, -1, 15),
                new(20, "Wheat", "Golden grains that make bread, pasta, and more.", ItemType.Produce, Rarity.Common, -1, 16),
                new(21, "Cotton", "Soft and fluffy, but not for eating!", ItemType.Produce, Rarity.Common, -1, 18),
                new(22, "Sweet Potato", "A sweet, starchy root packed with nutrients.", ItemType.Produce, Rarity.Common, -1, 20),
                new(23, "Cabbage", "Layered and crisp, great for slaws and stews.", ItemType.Produce, Rarity.Common, -1, 16),
                new(24, "Spinach", "A leafy green powerhouse of vitamins.", ItemType.Produce, Rarity.Common, -1, 14),
                new(25, "Coffee Bean", "The magic bean that fuels the world.", ItemType.Produce, Rarity.Common, -1, 25),
                new(26, "Corn", "Golden kernels bursting with natural sweetness.", ItemType.Produce, Rarity.Common, -1, 12),
                new(27, "Eggplant", "Glossy and purple, a sponge for flavors.", ItemType.Produce, Rarity.Common, -1, 20),

                new(28, "Apple", "Crisp and sweet, nature's perfect snack.", ItemType.Produce, Rarity.Uncommon, -1, 64),
                new(29, "Strawberry", "Bright red and juicy with a sweet tang.", ItemType.Produce, Rarity.Uncommon, -1, 58),
                new(30, "Blackberry", "A tart and juicy berry bursting with flavor.", ItemType.Produce, Rarity.Uncommon, -1, 68),
                new(31, "Lingonberry", "Tiny, tart berries often made into jam.", ItemType.Produce, Rarity.Uncommon, -1, 69),
                new(32, "Raspberry", "Delicate and sweet with a hint of tartness.", ItemType.Produce, Rarity.Uncommon, -1, 65),
                new(33, "Blackcurrant", "A deep purple berry with a bold taste.", ItemType.Produce, Rarity.Uncommon, -1, 63),
                new(34, "Apricot", "Golden-orange fruit with a smooth sweetness.", ItemType.Produce, Rarity.Uncommon, -1, 63),
                new(35, "Dragonfruit", "Exotic, vibrant, and subtly sweet.", ItemType.Produce, Rarity.Uncommon, -1, 62),
                new(36, "Mango", "Tropical, juicy, and irresistibly sweet.", ItemType.Produce, Rarity.Uncommon, -1, 60),
                new(37, "Lime", "Zesty and sour, a citrusy kick of flavor.", ItemType.Produce, Rarity.Uncommon, -1, 63),
                new(38, "Kiwi", "Fuzzy on the outside, emerald inside.", ItemType.Produce, Rarity.Uncommon, -1, 68),
                new(39, "Edamame", "Green soybeans, a perfect protein snack.", ItemType.Produce, Rarity.Uncommon, -1, 63),
                new(40, "Orange", "A juicy burst of citrusy goodness.", ItemType.Produce, Rarity.Uncommon, -1, 61),
                new(41, "Papaya", "Soft and tropical with a sweet aroma.", ItemType.Produce, Rarity.Uncommon, -1, 55),
                new(42, "Pear", "Smooth, juicy, and naturally sweet.", ItemType.Produce, Rarity.Uncommon, -1, 59),
                new(43, "Pineapple", "Spiky outside, golden sweetness inside.", ItemType.Produce, Rarity.Uncommon, -1, 54),

                new(44, "Wasabi", "Fiery green root, perfect for sushi.", ItemType.Produce, Rarity.Rare, -1, 234),
                new(45, "Saffron", "The world's most precious golden spice.", ItemType.Produce, Rarity.Rare, -1, 257),
                new(46, "Vanilla", "Aromatic pods filled with sweet warmth.", ItemType.Produce, Rarity.Rare, -1, 245),
                new(47, "Cinnamon", "Warm and spicy, a baking essential.", ItemType.Produce, Rarity.Rare, -1, 278),
                new(48, "Anise", "Licorice-like spice with a bold aroma.", ItemType.Produce, Rarity.Rare, -1, 223),
                new(49, "Nutmeg", "A rich and nutty spice for sweet and savory.", ItemType.Produce, Rarity.Rare, -1, 283),
                new(50, "Cumin", "Earthy and warm, a staple in global cuisine.", ItemType.Produce, Rarity.Rare, -1, 235),

                new(51, "Pumpkin", "A festive favorite, perfect for pies.", ItemType.Produce, Rarity.Legendary, -1, 960),
                new(52, "Watermelon", "Massive and hydrating with a sweet crunch.", ItemType.Produce, Rarity.Legendary, -1, 974),
                new(53, "Coconut", "Hard shell, soft heart, tropical delight.", ItemType.Produce, Rarity.Legendary, -1, 1021),
                new(54, "Durian", "The king of fruits—loved and feared alike.", ItemType.Produce, Rarity.Legendary, -1, 1337),

            });

        public static Item FromID(int id)
        {
            return ItemList.Find(x => x.ID == id);
        }
    }
}
