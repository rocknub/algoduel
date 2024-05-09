using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using DG.Tweening;
using UnityEngine;

public class VictoryPanel : PlayerMonoBehaviour
{
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private float tweenDuration;
    [SerializeField] private Ease tweenEase;
    
    private Vector2 targetPosition => Vector2.zero;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localPosition = hiddenPosition;
    }

    private void Start()
    {
        PlayerManager.VictoryCounter.OnFinalVictory.AddListener(TryDisplayVictoryMessage);
    }

    public void TryDisplayVictoryMessage(int entryIndex)
    {
        if (entryIndex != PlayerManager.PlayerIndex)
            return;
        DisplayVictoryMessage();
    }
    
    public void DisplayVictoryMessage()
    {
        rectTransform.DOLocalMove(targetPosition, tweenDuration).SetEase(tweenEase);
    }
}
