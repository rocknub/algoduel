using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TrophiesUI : PlayerMonoBehaviour
    {
        [SerializeField] private Image[] trophyImages;
        [SerializeField] private float retractionXOffset;
        [SerializeField] private float retractionTime;
        [SerializeField] private Ease retractionEase;
        [SerializeField] private float trophiesDisplayDuration = 1.0f;
        [SerializeField] private float startingSpriteAlpha;
        [SerializeField] private bool invertTrophiesOrder;        

        public Vector3 retractionPosition => new(retractionXOffset, displayPosition.y , displayPosition.z);

        private RectTransform rectTransform;
        private Vector3 displayPosition;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            displayPosition = rectTransform.localPosition;
            rectTransform.localPosition = retractionPosition;
            SetTrophyImages();
        }

        private void Start()
        {
            PlayerManager.VictoryCounter.OnRoundWon.AddListener(HighlightTrophy);
            PlayerManager.VictoryCounter.OnRoundWon.AddListener(_ => TempShowTrophies());
        }

        private void OnDisable()
        {
            PlayerManager.VictoryCounter.OnRoundWon.RemoveListener(HighlightTrophy);
            PlayerManager.VictoryCounter.OnRoundWon.RemoveListener(_ => TempShowTrophies());
        }

        public void TempShowTrophies()
        {
            var sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOLocalMoveX(displayPosition.x, retractionTime)
                .SetEase(retractionEase));
            sequence.AppendInterval(trophiesDisplayDuration);
            sequence.Append(rectTransform.DOLocalMoveX(retractionXOffset, retractionTime)
                .SetEase(retractionEase));
            sequence.Play();
        }

        private void SetTrophyImages()
        {
            trophyImages = GetComponentsInChildren<Image>();

            void SetTrophy(int index)
            {
                var trophyColor = trophyImages[index].color;
                trophyColor.a = startingSpriteAlpha;
                trophyImages[index].color = trophyColor;
            }

            if (invertTrophiesOrder)
            {
                for (int i = trophyImages.Length-1; i >= 0; i--) SetTrophy(i);
            }
            else
            {
                for (int i = 0; i < trophyImages.Length; i++) SetTrophy(i);
            }
        }

        private void HighlightTrophy(int victories)
        {
            if (victories > trophyImages.Length)
                return;
            int index = victories - 1;
            var trophyColor = trophyImages[index].color;
            trophyColor.a = 1;
            trophyImages[index].color = trophyColor;
        }
        
        public void TryDisable()
        {
            if (PlayerManager.IsPlayerReady == false)
                gameObject.SetActive(false);
        }
    }
}