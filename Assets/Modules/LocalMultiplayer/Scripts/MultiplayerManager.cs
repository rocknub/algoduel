using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class MultiplayerManager : MonoBehaviour
{
    public PlayerInput[] playerInputs;
    public InputActionReference inputAction;
    public bool prioritizeGamepad;

    private int playerIndex = 0;
    private InputDevice keyboardDevice;
    
    private void Start()
    {
        SetDevicesAndSchemes();
    }

    private void Update()
    {
        if (keyboardDevice != null)
        {
            // if (keyboardDevice.device.)
            // {
            //     Debug.Log("Keyboard used!");
            // }
        }
    }

    public void SetDevicesAndSchemes()
    {
        foreach (var inputUser in InputUser.all)
        {
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
            return;

        InputBinding? foundBinding = null;
        var parsedPathComponents = new InputControlPath.ParsedPathComponent[inputAction.action.bindings.Count];
        foreach (var binding in inputAction.action.bindings)
        {
            parsedPathComponents = InputControlPath.Parse(binding.path).ToArray();
            if (parsedPathComponents[1].name.Equals(control.name) == false) continue;
            foundBinding = binding;
            break;
        }

        if (foundBinding == null)
            return;

        var input = playerInputs[playerIndex];
        //To be used if gamepads are somehow meant to be prioritized
        // if (input.devices.Count > 0)
        // {
        //     if ((prioritizeGamepad && foundBinding.Value.path.ToLower().Contains("gamepad") == false) == false)
        //     {
        //         return;
        //     }
        // }

        var user = input.user;
        InputUser.PerformPairingWithDevice(control.device, user,
            InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        user.ActivateControlScheme(foundBinding.Value.groups);

        if (foundBinding.Value.path.ToLower().Contains("keyboard") && keyboardDevice == null)
        {
            keyboardDevice = user.pairedDevices[0];
        }

        if (++playerIndex <= playerInputs.Length - 1) return;
        
        playerIndex = 0;
        Debug.Log(playerIndex);
    }
}
