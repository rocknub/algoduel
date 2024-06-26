﻿using System;
using Character;
using UnityEngine;
using UnityEngine.Events;

namespace Projectiles
{
    public class Projectile : MonoBehaviour
    {
        /// <summary>
        /// Item1 = This projectile, Item2 = Collider this project has hit
        /// </summary>
        [SerializeField] private UnityEvent<Collider> onProjectileHit;
        [SerializeField] private UnityEvent<Projectile> onBlowEffectFinished; 

        private Rigidbody rb;
        private Renderer renderer;
        private Collider collider;

        public UnityEvent<Collider> OnProjectileHit => onProjectileHit;
        public UnityEvent<Projectile> OnBlowEffectFinished => onBlowEffectFinished;

        public Rigidbody Rigidbody
        {
            get
            {
                if (rb == null)
                    rb = GetComponent<Rigidbody>();
                return rb; 
            }
        }
        
        public Renderer Renderer
        {
            get
            {
                if (renderer == null)
                    renderer = GetComponent<Renderer>();
                return renderer; 
            }
        }
        
        public Collider Collider
        {
            get
            {
                if (collider == null)
                    collider = GetComponent<Collider>();
                return collider; 
            }
        }

        public void ToggleFreeze(bool value)
        {
            Rigidbody.isKinematic = value;
            Renderer.enabled = !value;
            Collider.enabled = !value;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (!collider.transform.parent.TryGetComponent(out PlayerManager hitPlayer))
            {
                return;
            }
            if (hitPlayer.IsInvulnerable)
            {
                return;
            }
            
            hitPlayer.ReceiveHit();
            onProjectileHit.Invoke(collider);
            onBlowEffectFinished.Invoke(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            onProjectileHit.Invoke(collision.collider);
        }
    }
}