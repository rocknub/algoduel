using System;
using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace PlayerEcossystem.UI
{
    public class DevicePromptUI : PlayerMonoBehaviour
    {
        [SerializeField] private TMP_Text promptMesh;
        [SerializeField] private Graphic[] gameplayOnlyGraphics;
        [SerializeField] private string inputRequestText;
        [SerializeField] private string inputConfirmationText;
        [SerializeField] private bool invertPanelMovement;
        [SerializeField] private float movementDuration;
        [SerializeField] private UnityEvent onPanelRetracted;

        private RectTransform rectT;
        private bool areGameplayGraphicsEnabled;

        private void Start()
        {
            rectT = GetComponent<RectTransform>();
            EnableGameplayGraphics(true);
            promptMesh.text = playerIndex == 0 ? inputRequestText : inputConfirmationText;
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
                promptMesh.text = inputConfirmationText;
            }
            else if(secretKey == playerIndex - 1)
            {
                promptMesh.SetText(inputRequestText);
            }
        }

        public void DetractPanel()
        {
            rectT.DOMoveX(rectT.position.x + Screen.width / 2 * (invertPanelMovement ? -1 : 1), movementDuration)
                .OnComplete(onPanelRetracted.Invoke);
        }
    }
}
