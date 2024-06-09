using UnityEngine;

namespace Character
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        protected PlayerManager manager;

        public T SetManager<T> (PlayerManager manager) where T : PlayerBehaviour
        {
            if (this.manager != null)
            {
                Debug.LogWarning("Manager has already been set. It is " + manager.name);
                return this as T;
            }
            this.manager = manager;
            return this as T;
        }
    }
}