using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


namespace PlayerEcossystem.UI
{
    public class DevicePromptUI : PlayerMonoBehaviour
    {
        [SerializeField] private TMP_Text promptMesh;
        [SerializeField] private string inputRequestText;
        [SerializeField] private string inputConfirmationText;
        [SerializeField] private bool invertPanelMovement;
        [SerializeField] private float movementDuration;
        [SerializeField] private UnityEvent onPanelRetracted;

        private RectTransform rectT;

        private void Start()
        {
            rectT = GetComponent<RectTransform>();
            promptMesh.text = playerIndex == 0 ? inputRequestText : inputConfirmationText;
        }

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
