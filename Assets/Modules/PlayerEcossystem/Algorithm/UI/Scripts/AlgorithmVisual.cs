using System.Collections.Generic;
using Algorithm;
using UnityEngine;
using UnityEngine.UI;

public class AlgorithmVisual : MonoBehaviour
{
    [SerializeField] private Transform commandSlotsParent;
    [SerializeField] private GameObject commandSlotModel;

    private List<Transform> commandSlots;

    private void GetAllSlots()
    {
        commandSlots ??= new List<Transform>(commandSlotsParent.childCount);
        for (int i = 0; i < commandSlotsParent.childCount; i++)
        {
            commandSlots.Add(commandSlotsParent.GetChild(i));
        }
    }

    // public void RedefineSlotQuantity(int quantity)
    // {
    //     int i = 0;
    //     while (i < quantity)
    //     {
    //         if (i >= commandSlotsParent.childCount)
    //         {
    //             GameObject commandSlot = Instantiate(commandSlotModel, commandSlotsParent, false);
    //         }
    //         i++;
    //     }
    //
    //     while (i < commandSlotsParent.childCount)
    //     {
    //         
    //     }        
    // }

    public void EnableCommandVisualization(Command command, int position)
    {
        Debug.Log($"Enabling visualization for {command.name} in position: {position}");
        Image commandIcon = commandSlotsParent.GetChild(position).GetCompone<Image>(true);
        Debug.Log(commandIcon.name);
        commandIcon.sprite = command.Icon;
        commandIcon.gameObject.SetActive(true);
    }

    public void DisableAllSlotIcons()
    {
        for (int i = 0; i < commandSlotsParent.childCount; i++)
        {
            commandSlotsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }
}
