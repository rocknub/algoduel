using System.Collections;
using Projectiles;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class PlayerFire : PlayerBehaviour
    {
        [SerializeField] private Transform fireOrigin;
        // [Range(0,1)][SerializeField] private Vector3 fireDirection;
        [SerializeField] private float fireForce;
        [SerializeField] private ForceMode forceMode;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float reloadDuration;
        [SerializeField] private bool showGizmos;
        [Header("Events")] 
        [SerializeField] private UnityEvent<Projectile> onProjectileFired;

        private bool isReloading;

        private Vector3 fireDirection => fireOrigin.forward;
        
        public void Fire()
        {
            var projectile = Instantiate(projectilePrefab, fireOrigin, false);
            projectile.transform.SetParent(null);
            var projComponent = projectile.GetComponent<Projectile>(); 
            projComponent.Rigidbody.AddForce(fireDirection * fireForce, forceMode);
            
            StartCoroutine(Reload());
            onProjectileFired.Invoke(projComponent);
        }

        public override bool IsActing() => isReloading;

        private IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadDuration);
            isReloading = false;
        }
        
        private void OnDrawGizmosSelected()
        {
            if (showGizmos == false)
                return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(fireOrigin.position, fireOrigin.position + fireDirection);
        }
    }
}