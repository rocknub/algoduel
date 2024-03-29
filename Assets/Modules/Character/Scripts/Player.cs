using System;
using UnityEditor;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {
        //TODO(Marlus - Maybe) Create a grid to calculate distance
        public float movingDistance;
        public float rotationTarget = 90f;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;

        private Vector3 lastFramePosition;

        public bool IsMoving => lastFramePosition != transform.position;

        private void Awake()
        {
            lastFramePosition = transform.position;
        }

        public void Move()
        {
            if (IsMoving)
            {
                return;
            }
            
        }

        public void RotateClockwise()
        {
            Debug.Log($"{name} should rotate {rotationTarget} units");
        }
        
        public void RotateCounterClockwise()
        {
            Debug.Log($"{name} should rotate {-rotationTarget} units");
        }

        public Vector3 GetTargetPosition()
        {
            return transform.position + transform.forward * movingDistance;
        }

        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
                return;
            Gizmos.color = Color.red;
            Vector3 TargetPosition = GetTargetPosition();
            Gizmos.DrawLine(TargetPosition, TargetPosition + Vector3.up * 100f);
        }
    }
}