using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Character
{
    public abstract class PlayerMonoBehaviour : MonoBehaviour
    {
        [CanBeNull][SerializeField] private PlayerManager playerManager = null;

        protected PlayerManager PlayerManager
        {
            get
            {
                if (playerManager != null) return playerManager;
                SetPlayerManager();
                return playerManager;
            }
        }

        [ContextMenu("Set Player Manager")]
        private void SetPlayerManager()
        {
            playerManager = GetComponentInParent<PlayerManager>();
        }

        //TODO: Chamar getready por metodo abstrato
        // protected void Start()
        // {
        //     PlayerManager.
        // }

        // protected abstract void SetPlayer(); 

        protected int playerIndex  => PlayerManager.PlayerIndex;

    }
}