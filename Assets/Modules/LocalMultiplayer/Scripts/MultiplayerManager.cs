using System;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

public class MultiplayerManager : MonoBehaviour
{
    public PlayerInput[] playerInputs;
    public InputActionReference inputAction;
    public bool recycleDeviceScheme;
    public string[] recycleExceptionSchemes;

    [Header("Game Events")] 
    [SerializeField] private IntGameEvent OnControlUserPrePaired;
    [FormerlySerializedAs("OnBothUsersPrePaired")] [SerializeField] private GameEvent OnPairingConcluded;

    private int playerIndex = 0;
    private InputDevice keyboardDevice;
    private Tuple<InputUser, InputDevice>[] prePairedUsersAndDevices;

    private void Awake()
    {
        SetDevicesAndSchemes();
    }

    private void OnDisable()
    {
        foreach (var inputUser in InputUser.all)
        {
            inputUser.UnpairDevicesAndRemoveUser();
        }
    }

    public void SetDevicesAndSchemes()
    {
        for (var i = 0; i < InputUser.all.Count; i++)
        {
            var inputUser = InputUser.all[i];
            // Debug.Log("A");
            // if (i >= playerInputs.Length)
            // {
            //     inputUser.UnpairDevicesAndRemoveUser();
            //     continue;
            // }
            inputUser.UnpairDevices();
        }
        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += UnpairedDeviceResponse;
        prePairedUsersAndDevices = new Tuple<InputUser, InputDevice>[playerInputs.Length];
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
        }
    }

    public void UnpairedDeviceResponse(InputControl control, InputEventPtr ptr)
    {
        if (control is not ButtonControl)
            return;
        if (playerIndex > playerInputs.Length-1)
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

        if (recycleDeviceScheme == false)
        {
            if (IsBindingWithinExceptions(foundBinding.Value))
            {
                if (IsDevicePrePaired(control.device))
                {
                    return;
                }
            }
            else if (IsBindingUsed(foundBinding.Value))
            {
                return;
            }
        }

        var input = playerInputs[playerIndex];
        var user = input.user;
        PrePairUserAndDevice(control.device, user, foundBinding.Value);
        playerIndex++;
    }

    private void PrePairUserAndDevice(InputDevice device, InputUser user, InputBinding binding)
    {
        prePairedUsersAndDevices[playerIndex] = new Tuple<InputUser, InputDevice>(user, device);
        user.ActivateControlScheme(binding.groups);
        OnControlUserPrePaired.Raise(playerIndex);
        if (playerIndex >= playerInputs.Length - 1)
        {
            ConcludeUserDevicePairing();
        }
    }

    public void ConcludeUserDevicePairing()
    {
        foreach (var userDeviceTuple in prePairedUsersAndDevices)
        {
            InputUser.PerformPairingWithDevice(userDeviceTuple.Item2, userDeviceTuple.Item1,
                InputUserPairingOptions.UnpairCurrentDevicesFromUser);
        }
        InputUser.listenForUnpairedDeviceActivity = 0;
        OnPairingConcluded.Raise();
    }

    private bool IsDevicePrePaired(InputDevice device)
    {
        return prePairedUsersAndDevices
            .Any(userDevicePair => userDevicePair?.Item2.deviceId == device.deviceId);
    }
    
    private bool IsBindingWithinExceptions(InputBinding binding)
    {
        foreach (var exception in recycleExceptionSchemes)
        {
            if (binding.groups.Contains(exception))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsBindingUsed(InputBinding binding)
    {
        foreach (var userDevicePair in prePairedUsersAndDevices)
        {
            if (userDevicePair?.Item1.controlScheme == null)
            {
                continue;
            }

            if (userDevicePair.Item1.controlScheme.Value.bindingGroup == binding.groups)
            {
                return true;
            }
        }

        return false;
    }
}
