using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GraphicsFader : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType;
    [SerializeField] private int loopCount;
    [SerializeField] private LoopType loopType;
    [SerializeField] private Graphic[] graphics;
    [Range(0, 1)] [SerializeField] private float[] targetAlphas;
    [SerializeField] private bool fadeOnAwake;
    [SerializeField] private bool unscaledTime;

    private float[] originalAlpha;
    
    public Tween[] Tween { get; private set; } 

    private void Awake()
    {
        if (fadeOnAwake)
        {
            StartFade();
        }
    }

    [ContextMenu("Start Fade")]
    public void StartFade()
    {
        originalAlpha = new float[graphics.Length];
        Tween = new Tween[graphics.Length];
        for (var i = 0; i < graphics.Length; i++)
        {
            var graphic = graphics[i];
            originalAlpha[i] = graphic.color.a;
            Tween[i] = graphic.DOFade(targetAlphas[i], duration).SetLoops(loopCount, loopType).SetEase(easeType);
            if (unscaledTime)
            {
                Tween[i].SetUpdate(UpdateType.Normal, true);
            }
        }
    }

    [ContextMenu("Use The First Alpha For All")]
    private void UseAlphaForAll()
    {
        var targetAlpha = targetAlphas[0];
        if (targetAlphas.Length != graphics.Length) targetAlphas = new float[graphics.Length];
        for (var index = 0; index < targetAlphas.Length; index++)
        {
            targetAlphas[index] = targetAlpha;
        }
    }

    public void CeaseTween()
    {
        for (var i = 0; i < graphics.Length; i++)
        {
            Tween[i].Kill();
            var graphic = graphics[i];
            graphic.DOFade(originalAlpha[i], 0);
        }
    }
}
