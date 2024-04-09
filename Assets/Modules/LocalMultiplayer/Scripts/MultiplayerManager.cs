using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class MultiplayerManager : MonoBehaviour
{
    public PlayerInput[] playerInputs;
    public InputActionReference inputAction;

    private void Start()
    {
        SetDevicesAndSchemes();
    }

    public void SetDevicesAndSchemes()
    {
        foreach (var inputUser in InputUser.all)
        {
            Debug.Log($"{inputUser.index}");   
            inputUser.UnpairDevices();
        }
        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += UnpairedDeviceResponse;
    }

    [ContextMenu("Debug Controls")]
    public void DebugControls()
    {
        foreach (var binding in inputAction.action.bindings)
        {
            Debug.Log($"Binding Name = {binding.path}");
            var control = InputSystem.FindControl(binding.path);
            var parsed = InputControlPath.Parse(binding.path);
            foreach (var pathPart in parsed)
            {
                Debug.Log(pathPart.name);
            }
            Debug.Log($"Binding Control = {InputSystem.FindControl(binding.path)}");
            // Debug.Log();
        }
    }

    public void UnpairedDeviceResponse(InputControl control, InputEventPtr ptr)
    {
        if (control is not ButtonControl)
        {
            return;
        }

        InputControl foundControl;
        foreach (var binding in inputAction.action.bindings)
        {
            foundControl = InputControlPath.TryFindControl(control, binding.path);
            if (foundControl != null)
                break;
        }
        
        Debug.Log("Found Control Name = " + control.name);
        
        Debug.Log($"Unpaired Device Used: " +
                  $"\n Control Name = {control.name}" +
                  $"\n Device Name = {control.device.name}" +
                  $"\n Parent Name = {control.parent.name}");
    }
}
