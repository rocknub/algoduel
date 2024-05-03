using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Character
{
    public class PlayerRendering : MonoBehaviour
    {
        [SerializeField] private Transform playerModel;
        [SerializeField] private float materialTweenPeriod;
        [SerializeField] private int materialTweenLoopCount;
        [SerializeField] private Color materialTweenColor;
        
        private List<Material> playerMaterials;

        private void Start()
        {
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
        
        public void TweenMaterials(Action action)
        {
            var materialSeq = DOTween.Sequence();
            foreach (var material in playerMaterials)
            {
                materialSeq.Join(material.DOColor(materialTweenColor, materialTweenPeriod).SetLoops(materialTweenLoopCount, LoopType.Yoyo).SetInverted());
            }
            materialSeq.OnComplete(action.Invoke);
            materialSeq.Play();
        }
    }
}