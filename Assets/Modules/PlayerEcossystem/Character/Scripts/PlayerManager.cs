using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private Transform playerModel;
        [SerializeField] private float materialTweenPeriod;
        [SerializeField] private int materialTweenLoopCount;
        [SerializeField] private Color materialTweenColor;

        public PlayerEnvironmentDetection EnvironmentDetection { get; private set; }
        public PlayerMovement Movement { get; private set; }
        public PlayerFire Fire { get; private set; }
        
        public bool IsInvulnerable { get; private set; }

        private List<Material> playerMaterials; 

        private void Start()
        {
            EnvironmentDetection = GetComponent<PlayerEnvironmentDetection>();
            Movement = GetComponent<PlayerMovement>().SetManager(this) as PlayerMovement;
            Fire = GetComponent<PlayerFire>().SetManager(this) as PlayerFire;
            
            GatherMaterials();
        }

        private void GatherMaterials()
        {
            var childCount = playerModel.childCount;
            playerMaterials = new List<Material>(childCount);
            for (var i = 0; i < childCount; i++)
            {
                var renderer = playerModel.GetChild(i).GetComponent<Renderer>();
                playerMaterials.AddRange(renderer.materials);
            } 
        }

        public void ReceiveHit()
        {

            Movement.ResetTransform();
            if (IsInvulnerable)
                return;
            IsInvulnerable = true;
            TweenMaterials(() => IsInvulnerable = false);
        }

        private void TweenMaterials(Action action)
        {
            var materialSeq = DOTween.Sequence();
            foreach (var material in playerMaterials)
            {
                materialSeq.Join(material.DOColor(materialTweenColor, materialTweenPeriod).SetLoops(materialTweenLoopCount, LoopType.Yoyo).SetInverted());
            }
            materialSeq.OnComplete(action.Invoke);
            materialSeq.Play();
        }
        
        public bool CanAct => Movement.IsActing() == false && Fire.IsActing() == false;
    }
}