using DG.Tweening;
using TMPro;
using UnityEngine;

public class DevicePromptUI : MonoBehaviour
{
    [SerializeField] private TMP_Text promptMesh;
    [SerializeField] private string inputConfirmationText;
    [SerializeField] private int inputKey;
    [SerializeField] private bool invertPanelMovement;
    [SerializeField] private float movementDuration;

    private RectTransform rectT;

    private void Start()
    {
        rectT = GetComponent<RectTransform>();
        // var newRect = rectT.rect;
        // rectT.rect.Set(newRect.x, newRect.y, newRect.width, Screen.height);
    }

    public void TryChangeTextToConfirmInteraction(int secretKey)
    {
        if (secretKey != inputKey) return;
        promptMesh.text = inputConfirmationText;
    }

     public void DetractPanel()
    {
        rectT.DOMoveX(rectT.position.x + Screen.width/2 * (invertPanelMovement ? -1 : 1), movementDuration);
    }
    
    
}
