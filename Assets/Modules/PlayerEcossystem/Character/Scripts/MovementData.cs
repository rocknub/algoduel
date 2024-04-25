using System;
using UnityEngine;

namespace Character
{
    [Serializable]
    public class MovementData
    {
        public Vector3 initialValue { get; private set; }
        public Vector3 finalValue { get; private set; } 
        public float duration { get; private set; }

        public MovementData(Vector3 initialValue, Vector3 finalValue, float duration)
        {
            this.initialValue = initialValue;
            this.finalValue = finalValue;
            this.duration = duration;
        }
    }
}