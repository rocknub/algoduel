using Character;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerEcossystem.UI
{
    public class PlayerIcon : PlayerMonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private Sprite damageSprite;
        [SerializeField] private Sprite victorySprite;
        [SerializeField] private Sprite loseSprite;
        [SerializeField] private float commandTransitionDuration;
        [SerializeField] private float targetValue = 1f;
        [Min(0)][SerializeField] private int loopAmount = 2;
        [SerializeField] private LoopType loopType;
        [SerializeField] private Ease flashEase;
        [SerializeField] private Ease finalEase;

        private Tween flashTween;

        private void OnEnable()
        {
            PlayerManager.OnPlayerDamaged.AddListener(FlashDamageSprite);
        }

        private void OnDisable()
        {
            PlayerManager.OnPlayerDamaged.RemoveListener(FlashDamageSprite);
        }

        public void FlashDamageSprite(int index)
        {
            if (playerIndex != index)
            {
                return;
            }
            Sprite originalSprite = panel.sprite;
            panel.sprite = damageSprite;
            flashTween = panel.DOFade(targetValue, commandTransitionDuration)
                .SetLoops(loopAmount, loopType)
                .SetEase(flashEase)
                .OnComplete(() => panel.sprite = originalSprite);
        }

        public void PresentFinalSprites(int index)
        {
            flashTween.Kill();
            panel.sprite = index == playerIndex ? victorySprite : loseSprite;
            flashTween = panel.DOFade(1, 0.5f).SetEase(finalEase);
        }

    }
}