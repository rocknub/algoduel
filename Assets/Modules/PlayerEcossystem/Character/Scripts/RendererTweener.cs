using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RendererTweener : MonoBehaviour
{
    public float materialTweenPeriod;
    public int materialTweenLoopCount;
    public Color materialTweenColor;

    private Renderer renderer;
    private List<Material> opaqueMaterials;
    private List<Material> toBeTransparentMaterials;

    public void GatherMaterials()
    {
        renderer = GetComponent<Renderer>();
        opaqueMaterials = new List<Material>(1);
        toBeTransparentMaterials = new List<Material>(1);
        for (var i = 0; i < renderer.materials.Length; i++)
        {
            opaqueMaterials.Add(renderer.materials[i]);
            var transparentMat = new Material(renderer.materials[i]);
            transparentMat.SetFloat("_Surface", 1.0f);
            transparentMat.SetFloat("_Blend", 0.0f);
            toBeTransparentMaterials.Add(transparentMat);
        }
    }
    
    public Tween[] TweenMaterials()
    {
        // var materialSeq = DOTween.Sequence();
        var length = toBeTransparentMaterials.Count; 
        var tweens = new Tween[length];
        for (int i = 0; i < length; i++)
        {
            // renderer.SetMaterials(toBeTransparentMaterials);
            // tweens[i] = renderer.materials[i].DOFade(0.0f, materialTweenPeriod)
            tweens[i] = renderer.materials[i].DOColor(materialTweenColor, materialTweenPeriod)
                .SetLoops(materialTweenLoopCount, LoopType.Yoyo).SetInverted();
                // .OnComplete(() => renderer.materials[i] = opaqueMaterials[i]);
        }

        return tweens;
    }
}