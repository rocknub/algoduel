using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Character
{
    public class PlayerFire : PlayerBehaviour
    {
        [SerializeField] private Transform fireOrigin;
        [SerializeField] private float fireForce;
        [SerializeField] private ForceMode forceMode;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float reloadDuration;
        [SerializeField] private bool showGizmos;
        [Header("Pooling")]
        [SerializeField] private float poolingDelay;
        [Min(1)][SerializeField] private int projectilePoolSize;
        [Header("Events")] 
        [SerializeField] private UnityEvent<Projectile> onProjectileFired;
        [SerializeField] private UnityEvent onReload;

        private bool isReloading;
        private int unqueuedProjectileCount = 0;
        private Queue<Projectile> projectilePool;

        private Vector3 fireDirection => fireOrigin.forward;

        private void Awake()
        {
            projectilePool = new Queue<Projectile>(projectilePoolSize);
        }

        public void Fire()
        {
            var projectile = InstantiateOrReuseProjectile();
            projectile.transform.position = fireOrigin.position;
            projectile.transform.SetParent(null);
            projectile.Rigidbody.AddForce(fireDirection * fireForce, forceMode);
            
            StartCoroutine(Reload());
            onProjectileFired.Invoke(projectile);
            StartCoroutine(PoolOnTime(projectile));
        }

        public override bool IsActing() => isReloading;

        private IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadDuration);
            isReloading = false;
            onReload.Invoke();
        }

        private IEnumerator PoolOnTime(Projectile projectile)
        {
            yield return new WaitForSeconds(poolingDelay);
            TryFreezeAndAddToPool(projectile);
        }

        private void TryFreezeAndAddToPool(Projectile projectile)
        {

            if (projectile == null)
                return;
            if (projectilePool.Contains(projectile))
                return;
            projectile.ToggleFreeze(true);
            projectilePool.Enqueue(projectile);
        }

        private Projectile InstantiateOrReuseProjectile()
        {
            Projectile projectileRef;
            if (projectilePool.Count > 0)
            {
                projectileRef = projectilePool.Dequeue();
                projectileRef.ToggleFreeze(false);
            }
            else
            {
                projectileRef = Instantiate(projectilePrefab, fireOrigin, false)
                    .GetComponent<Projectile>();
                projectileRef.gameObject.name += "_" + unqueuedProjectileCount++;
                // projectileRef.OnProjectileHit.AddListener();
                projectileRef.OnProjectileHit.AddListener(TryFreezeAndAddToPool);
            }
            return projectileRef;
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