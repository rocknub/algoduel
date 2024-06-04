using System;
using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace PlayerEcossystem.UI
{
    public class DevicePromptUI : PlayerMonoBehaviour
    {
        [SerializeField] private TMP_Text promptMesh;
        [SerializeField] private Graphic[] gameplayOnlyGraphics;
        [SerializeField] private string inputRequestText;
        [FormerlySerializedAs("inputConfirmationText")] [SerializeField] private string waitingText;
        [SerializeField] private string gameStartText;
        [FormerlySerializedAs("invertPanelMovement")] [SerializeField] private bool invertPanelMovementX;
        [SerializeField] private bool invertPanelMovementY;
        [SerializeField] private float movementDuration;
        [SerializeField] private UnityEvent onPanelRetracted;

        private RectTransform rectT;
        private bool areGameplayGraphicsEnabled;

        private void Start()
        {
            rectT = GetComponent<RectTransform>();
            EnableGameplayGraphics(true);
            promptMesh.text = playerIndex == 0 ? inputRequestText : waitingText;
        }

        private void EnableGameplayGraphics(bool value)
        {
            Array.ForEach(gameplayOnlyGraphics, g => g.enabled = value);
            areGameplayGraphicsEnabled = value;
        }

        [ContextMenu("Toggle Gameplay Graphics")]
        private void ToggleGameplayGraphics() => EnableGameplayGraphics(!areGameplayGraphicsEnabled); 

        public void TryChangeTextToConfirmInteraction(int secretKey)
        {
            if (secretKey == playerIndex)
            {
                promptMesh.text = waitingText;
            }
            else if(secretKey == playerIndex - 1)
            {
                promptMesh.SetText(inputRequestText);
            }
        }

        public void SetConclusionText()
        {
            promptMesh.SetText(gameStartText);
        }

        public void DetractPanel()
        {
            var targetPosition = Vector3.zero;
            targetPosition.x = rectT.position.x + Screen.width / 2 * (invertPanelMovementX ? -1 : 1);
            targetPosition.y = rectT.position.y + Screen.height / 2 * (invertPanelMovementY ? -1 : 1);
            rectT.DOMove(targetPosition, movementDuration)
                .OnComplete(onPanelRetracted.Invoke);
        }
    }
}
