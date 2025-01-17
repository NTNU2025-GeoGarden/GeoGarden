using System.Collections.Generic;

namespace Persistence
{
    [System.Serializable]
    public class GameState
    {
        public int water;
        public int fertilizer;

        public Dictionary<int, List<SavedMapResource>> mapResources;

        public GameState()
        {
            water = 250;
            fertilizer = 75;
            mapResources = new Dictionary<int, List<SavedMapResource>>();
        }
    }
}
