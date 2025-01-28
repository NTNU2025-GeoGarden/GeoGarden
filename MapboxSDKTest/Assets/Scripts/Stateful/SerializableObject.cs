using Structs;
using UnityEngine;

namespace Stateful
{
    public struct SerializableObject
    {
        public float X;
        public float Y;
        public float Z;
        public float RotX;
        public float RotY;
        public float RotZ;
        public float RotW;
        public EditableObjectType Type;
    }
}