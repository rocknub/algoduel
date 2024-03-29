using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Command : MonoBehaviour
{
    public UnityEvent OnExecution;

    public void Execute(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false)
            return;
        OnExecution.Invoke();
    }
}
