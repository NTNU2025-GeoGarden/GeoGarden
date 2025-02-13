using System;

namespace Structs
{
    public enum GrowState
    {
        Vacant,
        Seeded,
        Stage2,
        Stage3,
        Complete
    }

    public static class GrowStateTimeMultiplier
    {
        public static float FromState(GrowState state)
        {
            return state switch
            {
                GrowState.Vacant => 1.0f,
                GrowState.Seeded => 1.0f,
                GrowState.Stage2 => 1.2f,
                GrowState.Stage3 => 1.5f,
                GrowState.Complete => 1.7f,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}