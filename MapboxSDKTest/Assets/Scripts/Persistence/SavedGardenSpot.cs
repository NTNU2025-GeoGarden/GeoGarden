public enum GrowState
{
    Seeded,
    Stage1,
    Stage2,
    Stage3,
    Complete
}

namespace Persistence
{
    [System.Serializable]
    public struct SavedGardenSpot
    {
        public GrowState State;
        public int SeedID;
    }
}