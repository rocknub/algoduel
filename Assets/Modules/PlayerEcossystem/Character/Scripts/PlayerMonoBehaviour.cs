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
                playerManager = GetComponentInParent<PlayerManager>();
                return playerManager;
            }
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