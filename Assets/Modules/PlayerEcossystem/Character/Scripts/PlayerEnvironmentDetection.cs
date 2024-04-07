using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerEnvironmentDetection : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private Vector3 positionOffset;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private float obstacleCheckDebugDistance;

        public Vector3 originPosition => transform.position + positionOffset;

        public bool IsAboveGround()
        {
            return Physics.Raycast(originPosition, -transform.up, 100, groundMask);
        }

        public bool CheckForObstacles(float distance)
        {
            return Physics.Raycast(originPosition, transform.forward, distance, obstacleMask);
        }

        public bool CheckForObstacles(Vector3 target)
        {
            Vector3 anormalDir = target - originPosition;
            float distance = anormalDir.magnitude;
            return Physics.Raycast(originPosition, anormalDir/distance, distance, obstacleMask);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
                return;
            Gizmos.color = IsAboveGround() ? Color.blue : Color.red;
            Gizmos.DrawLine(originPosition, originPosition - transform.up * 100f);
            Gizmos.color = CheckForObstacles(obstacleCheckDebugDistance) ? Color.red : Color.blue;
            Gizmos.DrawLine(originPosition, originPosition + transform.forward * obstacleCheckDebugDistance);
        }
    }
}