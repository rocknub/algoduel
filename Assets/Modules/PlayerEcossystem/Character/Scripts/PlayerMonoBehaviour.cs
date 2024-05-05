using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Character
{
    public abstract class PlayerMonoBehaviour : MonoBehaviour
    {
        [CanBeNull] private PlayerManager playerManager = null;

        protected PlayerManager PlayerManager
        {
            get
            {
                if (playerManager != null) return playerManager;
                playerManager = GetComponentInParent<PlayerManager>();
                return playerManager;
            }
        }
        
        protected int playerIndex  => PlayerManager.PlayerIndex;

    }
}