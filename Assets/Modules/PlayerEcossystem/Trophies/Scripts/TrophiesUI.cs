using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Character
{
    public class TrophiesUI : MonoBehaviour
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
            Debug.Log("a");
            rectTransform.localPosition = retractionPosition;
            SetTrophyImages();
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
            trophyImages = new Image[transform.childCount];

            void SetTrophy(int index)
            {
                trophyImages[index] = transform.GetChild(index).GetComponent<Image>();
                var trophyColor = trophyImages[index].color;
                trophyColor.a = startingSpriteAlpha;
                trophyImages[index].color = trophyColor;
            }

            if (invertTrophiesOrder)
            {
                for (int i = transform.childCount-1; i >= 0; i--) SetTrophy(i);
            }
            else
            {
                for (int i = 0; i < transform.childCount; i++) SetTrophy(i);
            }

        }
    }
}