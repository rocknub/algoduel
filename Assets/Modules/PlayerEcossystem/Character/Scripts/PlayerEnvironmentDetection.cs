using UnityEngine;
using UnityEngine.Serialization;

namespace Character
{
    public class PlayerEnvironmentDetection : MonoBehaviour
    {
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private LayerMask obstacleMask;
        [Header("Debug")] 
        [SerializeField] private bool showGizmos = true;
        [SerializeField] private float obstacleCheckDebugDistance;

        public bool IsAboveGround()
        {
            return Physics.Raycast(transform.position, -transform.up, 100, groundMask);
        }

        public bool CheckForObstacles(float distance)
        {
            return Physics.Raycast(transform.position, transform.forward, distance, obstacleMask);
        }
        
        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
                return;
            Gizmos.color = IsAboveGround() ? Color.blue : Color.red;
            Gizmos.DrawLine(transform.position, transform.position - transform.up * 100f);
            Gizmos.color = CheckForObstacles(obstacleCheckDebugDistance) ? Color.red : Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * obstacleCheckDebugDistance);
        }
    }
}