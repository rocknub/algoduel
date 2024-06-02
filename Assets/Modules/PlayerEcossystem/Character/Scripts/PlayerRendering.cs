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
        [SerializeField] private RendererTweener[] tweeners;
        
        private List<Material[]> opaqueMaterials;
        private List<List<Material>> toBeTransparentMaterials;
        
        

        private void Start()
        {
            GatherMaterials();
        }

        private void GatherMaterials()
        {
            tweeners = playerModel.GetComponentsInChildren<RendererTweener>();
            foreach (var tweener in tweeners)
            {
                tweener.materialTweenColor = materialTweenColor;
                tweener.materialTweenPeriod = materialTweenPeriod;
                tweener.materialTweenLoopCount = materialTweenLoopCount;
                tweener.GatherMaterials();
            }
        }
        
        public void TweenMaterials(Action action)
        {
            var materialSeq = DOTween.Sequence();
            foreach (var tweener in tweeners)
            {
                var materialTweeners = tweener.TweenMaterials();
                foreach (var t in materialTweeners)
                {
                    materialSeq.Join(t);

                }
            }
            materialSeq.OnComplete(action.Invoke);
            materialSeq.Play();
        }
    }
}