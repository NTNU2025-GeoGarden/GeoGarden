using System.Collections.Generic;

namespace Persistence
{
    [System.Serializable]
    public class GameState
    {
        public List<(int id, int amount)> Inventory;
        public Dictionary<int, List<SavedMapResource>> MapResources;

        public GameState()
        {
            MapResources = new Dictionary<int, List<SavedMapResource>>();
            Inventory = new List<(int id, int amount)>();
        }
    }
}
