using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Command : MonoBehaviour
{
    public UnityEvent OnExecution;
    public UnityAction<Command> DoLoad;


    public void Execute()
    {
        OnExecution.Invoke();
    }
    
    public void Execute(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false)
            return;
        Execute();
    }

    public void Load(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false)
            return; 
        DoLoad(this);
    }
}
