using System;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerFire))]
    [RequireComponent(typeof(PlayerEnvironmentDetection))]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerMovement movement;
        private PlayerFire fire;
        private PlayerEnvironmentDetection environmentDetection;
        
        public bool CanAct { get; private set; }

        private void Start()
        {
            
        }
    }
}