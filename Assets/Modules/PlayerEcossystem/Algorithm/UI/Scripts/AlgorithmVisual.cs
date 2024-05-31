using System.Collections.Generic;
using Algorithm;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmVisual : MonoBehaviour
{
    [SerializeField] private Transform commandSlotsParent;
    [SerializeField] private Image[] bgPanels;
    [SerializeField] private float commandTransitionDuration;
    [SerializeField] private float defaultSlotAlpha;
    [SerializeField] private float selectedSlotAlpha;

    private List<Transform> commandSlots;
    private Image currentActiveSlot;

    public void EmphasizeSlot(int index)
    {
        if (currentActiveSlot != null)
        {
            currentActiveSlot.DOFade(defaultSlotAlpha, commandTransitionDuration);
        }
        currentActiveSlot = commandSlotsParent.GetChild(index).GetComponent<Image>();
        currentActiveSlot.DOFade(selectedSlotAlpha, commandTransitionDuration);
    }

    public void DisableSlotFocus()
    {
        if (currentActiveSlot != null)
        {
            currentActiveSlot.DOFade(defaultSlotAlpha, commandTransitionDuration);
            currentActiveSlot = null;
        }
    }

    public void FlashPanels()
    {
        foreach (var panel in bgPanels)
        {
            panel.DOFade(1, commandTransitionDuration).SetLoops(2, LoopType.Yoyo);
        }
    }

    public void EnableCommandVisualization(Command command, int position)
    {
        Image commandIcon = commandSlotsParent.GetChild(position).GetChild(0).GetComponent<Image>();
        commandIcon.sprite = command.IconData.icon;
        commandIcon.transform.localScale = command.IconData.presentationScale;
        commandIcon.DOFade(0, 0);
        commandIcon.gameObject.SetActive(true);
        commandIcon.DOFade(1, commandTransitionDuration);
    }

    public void DisableCommandVisualization(int position)
    {
        Image commandIcon = commandSlotsParent.GetChild(position).GetChild(0).GetComponent<Image>();
        commandIcon.DOFade(1, 0);
        commandIcon.gameObject.SetActive(false);
        commandIcon.DOFade(0, commandTransitionDuration);

    }

    public void DisableAllSlotIcons()
    {
        for (int i = 0; i < commandSlotsParent.childCount; i++)
        {
            commandSlotsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void AdjustSlotsVisibility(int quantity)
    {
        for (int i = 0; i < commandSlotsParent.childCount; i++)
        {
            commandSlotsParent.GetChild(i).gameObject.SetActive(i < quantity);            
        }
    }
}
