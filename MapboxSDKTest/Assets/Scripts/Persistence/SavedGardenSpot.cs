public enum GrowState
{
    Seed,
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
    }
}