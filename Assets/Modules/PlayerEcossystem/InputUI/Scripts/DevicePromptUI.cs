using System;
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
    }

    public void TryChangeTextToConfirmInteraction(int secretKey)
    {
        if (secretKey != inputKey) return;
        promptMesh.text = inputConfirmationText;
    }

     public void DetractPanel()
    {
        var width = rectT.rect.width;
        rectT.DOMoveX(rectT.position.x + width * (invertPanelMovement ? -1 : 1), movementDuration);
    }
    
    
}
