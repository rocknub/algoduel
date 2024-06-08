using System.Linq;
using UnityEngine.InputSystem;

public static class InputMethods
{
    public static bool TryGetBinding(InputControl control, InputAction action, out InputBinding? foundBinding)
    {
        foundBinding = null;
        var parsedPathComponents = new InputControlPath.ParsedPathComponent[action.bindings.Count];
        foreach (var binding in action.bindings)
        {
            parsedPathComponents = InputControlPath.Parse(binding.path).ToArray();
            if (parsedPathComponents[1].name.Equals(control.name) == false) 
                continue;

            foundBinding = binding;
            break;
        }
        return foundBinding.HasValue;
    }
}