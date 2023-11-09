using System;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerMovementData MovementData;
    }

    public struct PlayerMovementData
    {
        public float ForwardSpeed;
        public float SidewaysSpeed;
    }
}