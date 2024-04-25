using UnityEngine;

namespace Character
{
    public abstract class PlayerBehaviour : MonoBehaviour
    {
        protected PlayerManager manager;
        public abstract bool IsActing();

        public void SetManager(PlayerManager manager)
        {
            if (this.manager != null)
            {
                Debug.LogWarning("Manager has already been set. It is " + manager.name);
                return;
            }

            this.manager = manager;
        }
    }
}