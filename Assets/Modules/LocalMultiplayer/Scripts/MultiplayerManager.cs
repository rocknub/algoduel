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
    
    private void Start()
    {
        SetDevicesAndSchemes();
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

        if (playerInputs[playerIndex].devices.Count > 0)
        {
            if ((prioritizeGamepad && foundBinding.Value.path.ToLower().Contains("gamepad") == false) == false)
            {
                return;
            }
        }
        InputUser.PerformPairingWithDevice(control.device, playerInputs[playerIndex].user,
            InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        playerInputs[playerIndex].user.ActivateControlScheme(foundBinding.Value.groups);
        playerIndex++;

        if (playerIndex > playerInputs.Length)
            playerIndex = 0;
    }
}
