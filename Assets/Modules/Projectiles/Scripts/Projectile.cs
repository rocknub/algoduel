using System;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider> onProjectileHit;
        [SerializeField] private UnityEvent<Projectile> onBlowEffectFinished; 

        private Rigidbody rb;

        public UnityEvent<Collider> OnProjectileHit => onProjectileHit;

        public Rigidbody Rigidbody
        {
            get
            {
                if (rb == null)
                    rb = GetComponent<Rigidbody>();
                return rb; 
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            onProjectileHit.Invoke(GetComponent<Collider>());
            onBlowEffectFinished.Invoke(this);
        }
    }
}