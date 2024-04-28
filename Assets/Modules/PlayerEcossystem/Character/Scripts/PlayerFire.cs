using System;
using System.Collections;
using System.Collections.Generic;
using Projectiles;
using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class PlayerFire : PlayerBehaviour
    {
        [SerializeField] private Transform fireOrigin;
        [SerializeField] private float fireForce;
        [SerializeField] private ForceMode forceMode;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float reloadDuration;
        [Min(1)][SerializeField] private int projectilePoolSize;
        [SerializeField] private bool showGizmos;
        [Header("Events")] 
        [SerializeField] private UnityEvent<Projectile> onProjectileFired;
        [SerializeField] private UnityEvent onReload;

        private bool isReloading;
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
        }

        public override bool IsActing() => isReloading;

        private IEnumerator Reload()
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadDuration);
            isReloading = false;
            yield return new WaitForSeconds(reloadDuration * 1.5f);
            onReload.Invoke();
        }

        private void FreezeAndAddToPool(Projectile projectile)
        {
            if (projectilePool.Count < projectilePoolSize && projectilePool.Contains(projectile) == false) 
            {
                projectile.ToggleFreeze(true);
                projectilePool.Enqueue(projectile);
            }
            else
            {
                Debug.LogWarning("A projectile couldn't be pooled!");
                Destroy(projectile);
            }
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
                // projectileRef.OnBlowEffectFinished.AddListener(FreezeAndAddToPool);
                // onReload.AddListener(() => FreezeAndAddToPool(projectileRef));
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