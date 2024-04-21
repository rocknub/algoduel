using UnityEngine;

namespace Character
{
    public class PlayerFire : SingleTaskComponent
    {
        [SerializeField] private Transform fireOrigin;
        [SerializeField] private GameObject projectilePrefab;
        
        public override void DoIt()
        {
            Debug.Log($"{transform.parent.name} Should Fire!");
            // GameObject.Instantiate(projectilePrefab, fireOrigin, false);
        }
    }
}