using System;
using Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace PlayerEcossystem.UI
{
    public class PlayerIcon : PlayerMonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private Sprite damageSprite;
        [SerializeField] private Sprite victorySprite;
        [SerializeField] private float commandTransitionDuration;
        [SerializeField] private float targetValue = 1f;
        [Min(0)][SerializeField] private int loopAmount = 2;
        [SerializeField] private LoopType loopType;

        private void Start()
        {
            PlayerManager.OnPlayerDamaged.AddListener(FlashPanels);

        }

        public void FlashPanels(int index)
        {
            Sprite effectSprite = index == playerIndex ? damageSprite : victorySprite;
            Sprite originalSprite = panel.sprite;
            panel.sprite = effectSprite;
            panel.DOFade(targetValue, commandTransitionDuration)
                .SetLoops(loopAmount, loopType).OnComplete(() => panel.sprite = originalSprite);
        }

    }
}