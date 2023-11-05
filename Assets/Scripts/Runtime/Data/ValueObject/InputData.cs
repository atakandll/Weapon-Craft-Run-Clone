using System;
using Unity.Mathematics;


namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct InputData
    {
        public float HorizontalInputSpeed;
        public float2 HorizontalInputClampNegativeSides;
        public float HorizontalInputClampStopValue;
    }

    
}