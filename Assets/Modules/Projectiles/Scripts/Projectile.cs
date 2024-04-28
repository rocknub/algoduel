using Character;
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

        public void ToggleFreeze(bool value)
        {
            Rigidbody.isKinematic = value;
            GetComponent<MeshRenderer>().enabled = !value;
        }

        private void OnCollisionEnter(Collision collision)
        {
            onProjectileHit.Invoke(GetComponent<Collider>());
            onBlowEffectFinished.Invoke(this);

            if (collision.transform.parent.TryGetComponent(out PlayerMovement collidedPlayer))
            {
                collidedPlayer.ResetTransform();
            }
        }
    }
}