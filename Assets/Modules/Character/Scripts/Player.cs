using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        public float movingDistance;
        public float rotationTarget = 90f;

        public void Move()
        {
            Debug.Log($"{name} should move {movingDistance} units");
        }

        public void RotateClockwise()
        {
            Debug.Log($"{name} should rotate {rotationTarget} units");
        }
        
        public void RotateCounterClockwise()
        {
            Debug.Log($"{name} should rotate {-rotationTarget} units");
        }
    }
}