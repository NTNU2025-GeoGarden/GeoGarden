using System;
using Structs;

namespace Stateful
{
    [Serializable]
    public struct SerializableGardenSpot
    {
        public GrowState state;
        public int seedID;
        public DateTime stateCompletionTime;
        public float X;
        public float Y;
        public float Z;
    }
}