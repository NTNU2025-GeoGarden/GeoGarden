using System.Collections.Generic;

namespace Persistence
{
    [System.Serializable]
    public class GameState
    {
        public List<InventoryItem> inventory;
        public Dictionary<int, List<SavedMapResource>> mapResources;

        public GameState()
        {
            mapResources = new Dictionary<int, List<SavedMapResource>>();
            inventory = new List<InventoryItem>();
            inventory.Add(new InventoryItem
            {
                type = ResourceType.Seed,
                quality = Quality.Common,
                amount = 3
            });
        }
    }
}
