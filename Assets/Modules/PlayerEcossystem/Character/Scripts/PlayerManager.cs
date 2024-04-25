using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        // private PlayerMovement movement;
        // private PlayerFire fire;
        private List<PlayerBehaviour> playerActors;

        public PlayerEnvironmentDetection EnvironmentDetection { get; private set; }

        public bool CanAct => playerActors.TrueForAll(IsInert);
        
        private void Start()
        {
            EnvironmentDetection = GetComponent<PlayerEnvironmentDetection>();
            playerActors = new List<PlayerBehaviour>(2);
            GetComponents(playerActors);
            playerActors.ForEach(b => b.SetManager(this));
        }

        private static bool IsInert(PlayerBehaviour actor) => !actor.IsActing();
    }
}