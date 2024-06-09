using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class PlayerFire : PlayerBehaviour, IPlayerActor
    {
        [SerializeField] private Transform fireOrigin;
        [SerializeField] private float fireForce;
        [SerializeField] private ForceMode forceMode;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float fireDelay;
        [SerializeField] private float reloadDuration;
        [SerializeField] private bool showGizmos;
        [Header("Pooling")]
        [SerializeField] private float poolingDelay;
        [Min(1)][SerializeField] private int projectilePoolSize;
        [Header("Events")]
        [SerializeField] private UnityEvent onFireStart;
        [SerializeField] private UnityEvent<Projectile> onProjectileFired;
        [SerializeField] private UnityEvent onReload;

        private bool isReloading;
        private bool isFiring;
        private int unqueuedProjectileCount = 0;
        private Queue<Projectile> projectilePool;

        private Vector3 fireDirection => fireOrigin.forward;

        public UnityEvent OnFireStart => onFireStart;
        public UnityEvent<Projectile> OnProjectileFired => onProjectileFired;
        public UnityEvent OnReload => onReload;

        private void Awake()
        {
            projectilePool = new Queue<Projectile>(projectilePoolSize);
        }
        
        public bool IsActing() => isReloading || isFiring;

        public void Fire()
        {
            StartCoroutine(FireCoroutine());
        }

        public IEnumerator FireCoroutine()
        {
            isFiring = true;
            onFireStart.Invoke();
            yield return new WaitForSeconds(fireDelay);
            
            var projectile = InstantiateOrReuseProjectile();
            projectile.transform.position = fireOrigin.position;
            projectile.transform.SetParent(null);
            projectile.Rigidbody.AddForce(fireDirection * fireForce, forceMode);
            isFiring = false;
            
            StartCoroutine(Reload());
            onProjectileFired.Invoke(projectile);
            StartCoroutine(PoolOnTime(projectile));
        }
        
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
                projectileRef.OnProjectileHit.AddListener(_ => TryFreezeAndAddToPool(projectileRef));
                projectileRef.OnProjectileHit.AddListener(manager.TryConcludeAttackSuccess);
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