using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerEnvironmentDetection EnvironmentDetection { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerFire Fire { get; private set; }
        public PlayerRendering Rendering { get; private set; }
        
        public bool IsInvulnerable { get; private set; }
        
        private void Start()
        {
            EnvironmentDetection = GetComponent<PlayerEnvironmentDetection>();
            Rendering = GetComponent<PlayerRendering>();
            Movement = GetComponent<PlayerMovement>().SetManager(this) as PlayerMovement;
            Fire = GetComponent<PlayerFire>().SetManager(this) as PlayerFire;
        }
        
        public void ReceiveHit()
        {

            Movement.ResetTransform();
            if (IsInvulnerable)
                return;
            IsInvulnerable = true;
            Rendering.TweenMaterials(() => IsInvulnerable = false);
        }
        
        public bool CanAct => Movement.IsActing() == false && Fire.IsActing() == false;
    }
}