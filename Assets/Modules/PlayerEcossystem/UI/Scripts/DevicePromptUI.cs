using Character;
using DG.Tweening;
using TMPro;
using UnityEngine;


namespace PlayerEcossystem.UI
{
    public class DevicePromptUI : PlayerMonoBehaviour
    {
        [SerializeField] private TMP_Text promptMesh;
        [SerializeField] private string inputConfirmationText;
        [SerializeField] private bool invertPanelMovement;
        [SerializeField] private float movementDuration;

        private RectTransform rectT;

        private void Start()
        {
            rectT = GetComponent<RectTransform>();
        }

        public void TryChangeTextToConfirmInteraction(int secretKey)
        {
            if (secretKey != playerIndex) return;
            promptMesh.text = inputConfirmationText;
        }

        public void DetractPanel()
        {
            rectT.DOMoveX(rectT.position.x + Screen.width / 2 * (invertPanelMovement ? -1 : 1), movementDuration);
        }
    }
}
